import java.io.*;
import java.util.*;
import java.io.Serializable;


class CustomerDataManagementBase implements Serializable {
    @Serial
    private static final long serialVersionUID = 1L;
    //
    // Process helpers
    private static int storedNationalID;
    private static String storedpassword;

    private static int inputNationIdForLogin;
    private static String inputtedPasswordForLoggingIn;

    private static int inputtedNationalIdToRegister;
    private static String inputtedPasswordToRegister;

    private static String inputtedFirstNameToRegister;
    private static String inputtedSecondNameToRegister;

    static int storedIdToTrans;
    static int inputtedIdToTrans;

    private static int nationalIDforChangingPassword;
    private static String passwordForChangingPassword;

    private static String newPasswordFirstTime;
    private static String newPasswordSecondTime;

    private static int accountNumberInArray;
    //
    // Constructor data types
    private int nationalID;
    private String password;
    private double balance;
    private String firstName;
    private String lastName;
    //
    // Configurations
    static CustomerDataManagementBase customer = new CustomerDataManagementBase();
    protected static final List<CustomerDataManagementBase> accArrayData = new ArrayList<>();
    private static Scanner scanner = new Scanner(System.in);
    private static String accountsDataFile = "Data/CustomerAccountsData.ser";
    //
    // Limitations
    private static boolean storedFromFileAccountsData = false;
    private static int limitRegisterNationID;
    private static int limitLogin;
    private static int limitPassword;
    private static int limitIDForChangingPassword;
    private static int limitOldPasswordToChangingPassword;
    private static int limitLoginProcess;
    private static int limitNewPasswordForChangingIt;

    //
    //
    public CustomerDataManagementBase(int nationalID, String password) {
        this.nationalID = nationalID;
        this.password = password;
        this.balance = 0.0;
    }

    public CustomerDataManagementBase(String firstName, String lastName, int nationalID, String password) {
        this.nationalID = nationalID;
        this.password = password;
        this.balance = 0.0;
        this.firstName = firstName;
        this.lastName = lastName;
    }

    public CustomerDataManagementBase() {
    }

    //
    // --- START OF UPDATING PASSWORD PROCESS --
    static protected void changePassword() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Here you can change your password");
        inputIDForChangingPassword();
    }

    static private void inputIDForChangingPassword() throws IOException, InterruptedException, ClassNotFoundException {
        if (getLimitIDForChangingPassword() < 3) {
            setLimitIDForChangingPassword(getLimitIDForChangingPassword() + 1);
            System.out.println("Enter your Account national ID..");
            try {
                setNationalIDforChangingPassword(scanner.nextInt());
            } catch (Exception e) {
                e.getStackTrace();
                System.out.println("Invalid input!!");
                scanner.nextLine();
                inputIDForChangingPassword();
            }
            if ((getNationalIDforChangingPassword() == getInputNationIdForLogin())) {
                inputOldPasswordForChangingPassword();
            } else {
                setLimitIDForChangingPassword(getLimitIDForChangingPassword() + 1);
                System.out.println("National ID Number for the Account is wrong!!");
                inputIDForChangingPassword();
            }
        } else {
            System.out.println("Chances are out!!");
            SelfServiceMachine.exit();
            return;
        }
    }

    static private void inputOldPasswordForChangingPassword() throws IOException, InterruptedException, ClassNotFoundException {
        if (getLimitOldPasswordToChangingPassword() < 3) {
            setLimitOldPasswordToChangingPassword(getLimitOldPasswordToChangingPassword() + 1);
            System.out.println("Enter you old password..");
            String oldPassword;
            try {
                oldPassword = scanner.next();
            } catch (Exception e) {
                e.getStackTrace();
                System.out.println("Invalid input!!");
                scanner.nextLine();
                inputOldPasswordForChangingPassword();
                return;
            }
            if (Objects.equals(oldPassword, getInputtedPasswordForLoggingIn())) {

                inputNewPasswordForChangingIt();

            } else {
                System.out.println("Invalid password!!");
                inputOldPasswordForChangingPassword();
            }
        } else {
            System.out.println("Chances are out!!");
            SelfServiceMachine.exit();
        }
    }

    static private void inputNewPasswordForChangingIt() throws IOException, InterruptedException, ClassNotFoundException {
        if (getLimitNewPasswordForChangingIt() < 5) {
            setLimitNewPasswordForChangingIt(getLimitNewPasswordForChangingIt() + 1);

            System.out.println("Enter you new password..");
            try {
                newPasswordFirstTime = scanner.next();
            } catch (Exception e) {
                e.getStackTrace();
                System.out.println("Invalid input!!");
                scanner.nextLine();
                inputNewPasswordForChangingIt();
                return;
            }
            if (newPasswordFirstTime.length() < 8) {
                System.out.println("Invalid input!! Your password should has more than 7 chars.");
                scanner.nextLine();
                inputNewPasswordForChangingIt(); // Repeat
            } else {

                if (!newPasswordFirstTime.equals(getInputtedPasswordForLoggingIn())) {
                    inputNewPasswordSecondTime(); // Next Step
                } else {
                    System.out.println("Cannot use the same old password.");
                    inputNewPasswordForChangingIt();
                }
            }
        } else {
            System.out.println("Five chances out!!");
            SelfServiceMachine.exit();
        }
    }

    static private void inputNewPasswordSecondTime() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter your new password again to confirm..");

        try {
            newPasswordSecondTime = scanner.next();
        } catch (Exception e) {
            e.getStackTrace();
            System.out.println("Invalid input!!");
            scanner.nextLine();
            inputNewPasswordSecondTime();
            return;
        }

        if (Objects.equals(newPasswordFirstTime, newPasswordSecondTime)) {
            changeOldPasswordFinalStep();
        } else {
            System.out.println("Does not match!!");
            scanner.nextLine();
            inputNewPasswordForChangingIt();
        }
    }

    static private void changeOldPasswordFinalStep() throws IOException, InterruptedException, ClassNotFoundException {
        accArrayData.get(accountNumberInArray).setPassword(getNewPasswordSecondTime()); // Change something in the arraylist
        savaAccountsData();
        System.out.println("Changed you password successfully!");
        SelfServiceMachine.semiUI();
    }

    // --- END OF UPDATING PASSWORD PROCESS ---
    //
    // --- START OF UPDATING BALANCES PROCESS ---
    static protected void updateUserBalance(ArrayList<CustomerDataManagementBase> accArrayData, int nationalID, double amount) throws IOException {
        int index = getAccountNumberInArray(); // Assuming you have a method to get the index of the Customer
        CustomerDataManagementBase customer = accArrayData.get(index);
        customer.setBalance(amount);
        savaAccountsData();
    }

    static protected void updateReceiverBalance(ArrayList<CustomerDataManagementBase> accArrayData, int index, double amount) throws IOException {
        CustomerDataManagementBase customer = accArrayData.get(index);
        customer.setBalance(amount);
        savaAccountsData();
    }

    // --- END OF UPDATING BALANCES PROCESS ---
    //
    // --- START OF LOADING, UPDATING DATA PROCESSES ---
    static protected void savaAccountsData() throws IOException {
        File file = new File(accountsDataFile);
        FileOutputStream fOut = new FileOutputStream(file);
        ObjectOutputStream objOut = new ObjectOutputStream(fOut);

        objOut.writeObject(accArrayData);
        objOut.flush();
        objOut.close();
    }

    static protected void loadAccountsData() throws IOException, ClassNotFoundException {
        if (!storedFromFileAccountsData) {
            storedFromFileAccountsData = true;

            File file = new File(accountsDataFile);
            if (!file.exists()) {
                file.getParentFile().mkdirs();
                boolean fileCreated = false;
                try {
                    fileCreated = file.createNewFile();
                } catch (IOException e) {
                    e.printStackTrace();
                }
                if (!fileCreated) {
                    throw new IOException("Failed to create a new file: " + accountsDataFile);
                }
            }
            try (FileInputStream fIn = new FileInputStream(file);
                 ObjectInputStream objIn = new ObjectInputStream(fIn)) {
                List<CustomerDataManagementBase> deserializedList = null;
                try {
                    deserializedList = (List<CustomerDataManagementBase>) objIn.readObject();
                } catch (ClassNotFoundException | NullPointerException e) {
                    e.printStackTrace();
                }
                accArrayData.clear();
                if (deserializedList != null) {
                    accArrayData.addAll(deserializedList);
                }
                System.out.println(accArrayData);

            } catch (EOFException | NullPointerException e) {
                System.out.println("End of file reached while reading from the object stream.");
            }
        }
    }

    // --- END OF LOADING, UPDATING DATA PROCESSES ---
    //
    // --- START OF CHOCEEING BETWEEN LOGIN OR REGISTER PROCESS ---
    protected static void LoginOrRegister() throws InterruptedException, IOException, ClassNotFoundException {
        if (!SelfServiceMachine.isFinalCustomerPassed) {
            if (SelfServiceMachine.rear != null && SelfServiceMachine.front != null) {
                if (OrganizingMachine.getRearMinusFront() >= 0) {
                    if (OrganizingMachine.getRearMinusFront() == 0) {
                        SelfServiceMachine.isFinalCustomerPassed = true;
                    }
                    System.out.println("-----------------\nNext customer, number: " + SelfServiceMachine.front.data + " ");
                    System.out.println("Remaining customers: " + OrganizingMachine.getRearMinusFront() + "\n");

                    getInputForLoginOrRegister();
                }
            } else {
                System.out.println("In order to use our SSM Please pick an order,\nUsing the Organizing Machine.");
            }
        } else {
            System.out.println("In order to use our SSM Please pick an order,\nUsing the Organizing Machine.");
        }
    }

    private static void getInputForLoginOrRegister() throws IOException, InterruptedException, ClassNotFoundException {

        System.out.println("Welcome to our SSM System.\nPress 1 for Log in to your account.");
        System.out.println("Press 2 Register for a new account.");

        int InputForLoginOrRegister;
        try {
            InputForLoginOrRegister = scanner.nextInt();
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            LoginOrRegister();
            return;
        }
        switch (InputForLoginOrRegister) {
            case 1:
                login();
                break;
            case 2:
                register();
                break;
            default:
                System.out.println("Invalid input!!");
                LoginOrRegister();
                break;
        }
    }

    // --- END OF CHOCEEING BETWEEN LOGIN OR REGISTER PROCESS ---
    //
    // --- START OF LOGGING IN PROCESS --
    static private void login() throws InterruptedException, IOException, ClassNotFoundException {
        if (getLimitLogin() < 3) {
            setLimitLogin(getLimitLogin() + 1);
            try {
                loadAccountsData();
            } catch (Exception e) {
                e.printStackTrace();
            }
            System.out.println("Here you can Login.");
            getIDInputForLoggingIn();
            getPasswordInputForLoggingIn();
            checkAccountValidatedisity();

        } else {
            System.out.println("Three chances out!!");
            SelfServiceMachine.exit();
        }
    }

    private static void getIDInputForLoggingIn() throws IOException, InterruptedException, ClassNotFoundException {
        if (getLimitLoginProcess() < 5) {
            // the chances get decreased at the end of the process
            System.out.println("Enter your National ID..");
            try {
                setInputNationIdForLogin(scanner.nextInt());
            } catch (Exception e) {
                e.getStackTrace();
                scanner.nextLine();
                System.out.println("Invalid input!!");
                getIDInputForLoggingIn();
                return;
            }
        } else {
            System.out.println("Five chances out!!");
            SelfServiceMachine.exit();
        }
    }

    private static void getPasswordInputForLoggingIn() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter your Password..");
        try {
            setInputtedPasswordForLoggingIn(scanner.next());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input!!");
            getPasswordInputForLoggingIn();
            return;
        }
    }

    private static void checkAccountValidatedisity() throws IOException, InterruptedException, ClassNotFoundException {
        int p;
        for (p = 0; p < accArrayData.size(); p++) {
            try {
                setStoredNationalID(accArrayData.get(p).getNationalID());
            } catch (NullPointerException ignored) {
            }
            try {
                setStoredPassword(accArrayData.get(p).getPassword());
            } catch (NullPointerException ignored) {
            }

            if (getInputNationIdForLogin() == getStoredNationalID() && getInputtedPasswordForLoggingIn().equals(getStoredPassword())) {
                System.out.println("Successfully logged in!!\n");
                accountNumberInArray = p;
                setAccountNumberInArray(accountNumberInArray);
                customer.setBalance(accArrayData.get(getAccountNumberInArray()).balance);
                SelfServiceMachine.mainUI();
                break;
            }
        }
        if (!(getInputNationIdForLogin() == getStoredNationalID() && getInputtedPasswordForLoggingIn().equals(getStoredPassword()))) {
            System.out.println("National ID or password is incorrect!!\n");
            setLimitLoginProcess(getLimitLoginProcess() + 1); // Chances --;
            login();
        }
    }

    // --- END OF LOGGING IN PROCESS ---
    //
    // --- START OF REGISTRATION PROCESS ---
    private static void register() throws InterruptedException, IOException, ClassNotFoundException {
        try {
            loadAccountsData();
        } catch (IOException | ClassNotFoundException e) {
            System.out.println("Exception in loading the data: " + e.getMessage());
        }
        System.out.println("Here you can quickly REGISTER in our system.");
        CustomerDataManagementBase registerTheseData = null;
        registerFirstName();
        registerSecondName();
        registerNationID();
        if (getInputtedNationalIdToRegister() != getStoredNationalID()) {
            registerPassword();
            try {
                registerTheseData = new CustomerDataManagementBase(getInputtedFirstNameToRegister(), getInputtedSecondNameToRegister(),
                        getInputtedNationalIdToRegister(), getInputtedPasswordToRegister());
            } catch (Exception e) {
                return;
            }
            if (registerTheseData != null) {
                accArrayData.add(registerTheseData);
                //System.out.println(accArrayData);
                savaAccountsData();
                System.out.println("Registered Successfully!!\n");
                login();
            }
        }
    }

    private static void registerFirstName() {
        // limit the process
        System.out.println("Enter your first name..");
        try {
            setInputtedFirstNameToRegister(scanner.next());
        } catch (Exception e) {
            System.out.println("Invalid input!!");
            scanner.nextLine();
            registerFirstName();
            return;
        }
        if (getInputtedFirstNameToRegister().matches(".*\\d.*")) {
            System.out.println("Invalid input!! Please enter letters only!");
            registerFirstName();
        }
    }

    private static void registerSecondName() {
        // limit the process
        System.out.println("Enter you second name..");
        try {
            setInputtedSecondNameToRegister(scanner.next());
        } catch (Exception e) {
            System.out.println("Invalid input!!");
            scanner.nextLine();
            registerFirstName();
            return;
        }
        if (getInputtedFirstNameToRegister().matches(".*\\d.*")) {
            System.out.println("Invalid input!! Please enter letters only!");
            registerSecondName();
        }
    }

    private static void registerNationID() throws InterruptedException, IOException, ClassNotFoundException {
        if (getLimitRegisterNationID() < 3) {
            setLimitRegisterNationID(getLimitRegisterNationID() + 1);
            System.out.println("Enter your National ID (exactly 8 digits)..");
            try {
                setInputtedNationalIdToRegister(scanner.nextInt());
            } catch (InputMismatchException e) {
                System.out.println("Invalid input!!");
                scanner.nextLine();
                registerNationID();
                return;
            }
            if (String.valueOf(getInputtedNationalIdToRegister()).length() != 8) {
                registerNationID();
                return;
            }
            checkIfNationIdUnique();
        } else {
            System.out.println("Three chances out!!");
            SelfServiceMachine.exit();
        }
    }

    private static void checkIfNationIdUnique() throws InterruptedException, IOException, ClassNotFoundException {
        for (int i = 0; i < accArrayData.size(); i++) {
            try {
                setStoredNationalID(accArrayData.get(i).getNationalID());
            } catch (NullPointerException e) {
                return;
            }
            if (getInputtedNationalIdToRegister() == getStoredNationalID()) {
                System.out.println("You have an account already! \n");
                login();
                return;
            }
        }
    }

    private static void registerPassword() throws InterruptedException, IOException, ClassNotFoundException {
        if (getLimitPassword() < 3) {
            setLimitPassword(getLimitPassword());
            System.out.println("Enter Your Password..(Equal to or more than 8 chars.)");
            try {
                setInputtedPasswordToRegister(scanner.next());
            } catch (Exception e) {
                e.getStackTrace();
                scanner.nextLine();
                registerPassword();
                return;
            }
            if (getInputtedPasswordToRegister().length() < 8) {
                System.out.println("Invalid input!!");
                scanner.nextLine();
                registerPassword();
                return;
            }
        } else {
            System.out.println("Three chances out!!");
            SelfServiceMachine.exit();
        }
    }
    // --- END OF REGISTRATION PROCESS ---
    //

    public int getNationalID() {
        return nationalID;
    }

    public String getPassword() {
        return password;
    }


    public double getBalance() {
        return this.balance;
    }

    public void setBalance(double balance) {
        this.balance = balance;
    }


    public static int getAccountNumberInArray() {
        return accountNumberInArray;
    }

    public static void setAccountNumberInArray(int accountNumberInArray) {
        CustomerDataManagementBase.accountNumberInArray = accountNumberInArray;
    }

    public static int getInputNationIdForLogin() {
        return inputNationIdForLogin;
    }

    public static void setInputNationIdForLogin(int inputNationIdForLogin) {
        CustomerDataManagementBase.inputNationIdForLogin = inputNationIdForLogin;
    }

    public static int getInputtedNationalIdToRegister() {
        return inputtedNationalIdToRegister;
    }

    public static void setInputtedNationalIdToRegister(int inputtedNationalIdToRegister) {
        CustomerDataManagementBase.inputtedNationalIdToRegister = inputtedNationalIdToRegister;
    }

    public static String getInputtedPasswordForLoggingIn() {
        return inputtedPasswordForLoggingIn;
    }

    public static void setInputtedPasswordForLoggingIn(String inputtedPasswordForLoggingIn) {
        CustomerDataManagementBase.inputtedPasswordForLoggingIn = inputtedPasswordForLoggingIn;
    }

    public static int getStoredNationalID() {
        return storedNationalID;
    }

    public static void setStoredNationalID(int storedNationalID) {
        CustomerDataManagementBase.storedNationalID = storedNationalID;
    }

    public static String getStoredPassword() {
        return storedpassword;
    }

    public static void setStoredPassword(String storedpassword) {
        CustomerDataManagementBase.storedpassword = storedpassword;
    }

    public static String getInputtedPasswordToRegister() {
        return inputtedPasswordToRegister;
    }

    public static void setInputtedPasswordToRegister(String inputtedPasswordToRegister) {
        CustomerDataManagementBase.inputtedPasswordToRegister = inputtedPasswordToRegister;
    }

    public static int getStoredIdToTrans() {
        return storedIdToTrans;
    }

    public static void setStoredIdToTrans(int storedIdToTrans) {
        SelfServiceMachine.storedIdToTrans = storedIdToTrans;
    }

    public static int getInputtedIdToTrans() {
        return inputtedIdToTrans;
    }

    public static void setInputtedIdToTrans(int inputtedIdToTrans) {
        SelfServiceMachine.inputtedIdToTrans = inputtedIdToTrans;
    }

    public static int getLimitRegisterNationID() {
        return limitRegisterNationID;
    }

    public static void setLimitRegisterNationID(int threeTimesChanceRegisterNationID) {
        CustomerDataManagementBase.limitRegisterNationID = threeTimesChanceRegisterNationID;
    }

    public static int getLimitLogin() {
        return limitLogin;
    }

    public static void setLimitLogin(int threeTimesChanceLogin) {
        CustomerDataManagementBase.limitLogin = threeTimesChanceLogin;
    }

    public static int getLimitPassword() {
        return limitPassword;
    }

    public static void setLimitPassword(int threeTimesChancePassword) {
        CustomerDataManagementBase.limitPassword = threeTimesChancePassword;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String firstName) {
        this.firstName = firstName;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String lastName) {
        this.lastName = lastName;
    }

    public static int getNationalIDforChangingPassword() {
        return nationalIDforChangingPassword;
    }

    public static void setNationalIDforChangingPassword(int nationalIDforChangingPassword) {
        CustomerDataManagementBase.nationalIDforChangingPassword = nationalIDforChangingPassword;
    }

    public static String getPasswordForChangingPassword() {
        return passwordForChangingPassword;
    }

    public static void setPasswordForChangingPassword(String passwordForChangingPassword) {
        CustomerDataManagementBase.passwordForChangingPassword = passwordForChangingPassword;
    }

    public static int getLimitIDForChangingPassword() {
        return limitIDForChangingPassword;
    }

    public static void setLimitIDForChangingPassword(int threeTimesChanceIDforChangingPassword) {
        CustomerDataManagementBase.limitIDForChangingPassword = threeTimesChanceIDforChangingPassword;
    }

    public static int getLimitOldPasswordToChangingPassword() {
        return limitOldPasswordToChangingPassword;
    }

    public static void setLimitOldPasswordToChangingPassword(int threeTimesChanceOldPasswordForChangingPassword) {
        CustomerDataManagementBase.limitOldPasswordToChangingPassword = threeTimesChanceOldPasswordForChangingPassword;
    }

    public void setPassword(String password) {
        this.password = password;
    }

    public static String getNewPasswordSecondTime() {
        return newPasswordSecondTime;
    }

    public static void setNewPasswordSecondTime(String newPasswordSecondTime) {
        CustomerDataManagementBase.newPasswordSecondTime = newPasswordSecondTime;
    }

    public static String getInputtedFirstNameToRegister() {
        return inputtedFirstNameToRegister;
    }

    public static void setInputtedFirstNameToRegister(String inputtedFirstNameToRegister) {
        CustomerDataManagementBase.inputtedFirstNameToRegister = inputtedFirstNameToRegister;
    }

    public static String getInputtedSecondNameToRegister() {
        return inputtedSecondNameToRegister;
    }

    public static void setInputtedSecondNameToRegister(String inputtedSecondNameToRegister) {
        CustomerDataManagementBase.inputtedSecondNameToRegister = inputtedSecondNameToRegister;
    }

    public static int getLimitNewPasswordForChangingIt() {
        return limitNewPasswordForChangingIt;
    }

    public static void setLimitNewPasswordForChangingIt(int fiveTimesChanceForNewPasswordForChangingIt) {
        CustomerDataManagementBase.limitNewPasswordForChangingIt = fiveTimesChanceForNewPasswordForChangingIt;
    }

    public static int getLimitLoginProcess() {
        return limitLoginProcess;
    }

    public static void setLimitLoginProcess(int fiveTimesChanceForLoginProcess) {
        CustomerDataManagementBase.limitLoginProcess = fiveTimesChanceForLoginProcess;
    }

    @Override
    public String toString() {
        return "\n FirstName: " + getFirstName() + "\n  LastName: " + getLastName() + "\nNationalID: " + getNationalID() + "\n   Balance: " + getBalance() + "$\n  Password: " + getPassword() + "\n--------\n";
    }
}
