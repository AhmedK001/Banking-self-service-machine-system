import java.io.*;
import java.util.*;
import java.io.Serializable;


/*

    * System Overview: (Part 3) *
    * Customer Data Management Base Overview *

    This section focuses on the Customer Data Management Base,
    a crucial component that handles all customer data dynamically and in real-time. It is designed to:

    Automatically load and store customer data in alignment
    with the SSM's requirements, ensuring seamless operation and data integrity.

    * SSM's Dynamic Data Management *
    The SSM's approach to maintaining a fully dynamic system includes:

    - Current Data Storage Methods: Initially utilizing an array list for customer data,
        with plans to transition to a binary tree structure.
        This change aims to significantly reduce search times to approximately O(log n), enhancing efficiency.
    - Dynamic Handling and Integration: Through recursive programming,
        the system achieves remarkable flexibility in real-time data processing,
        eliminating the need for restarts. It also:
            - Ensures data is saved and retrieved as needed.
            - Seamlessly interfaces with both the Organizing Machine and the CustomerDataManagement System.
    - Security and Exception Handling:
    Implements safeguards against misuse and employs sophisticated techniques for managing exceptions, ensuring system integrity and reliability.

    * Continuation:
    As the narrative on system functionalities concludes with Part 3, users are now equipped
    with a comprehensive understanding of the dynamic interactions between the SSM and the Customer Data Management Base,
    highlighting the system's efficiency, security, and adaptability.
     */

class CustomerDataManagementBase implements Serializable {
    @Serial
    private static final long serialVersionUID = 1L;
    //
    // Process helpers
    static int storedNationalID;
    static String storedpassword;

    static int inputNationIdForLogin;
    static String inputtedPasswordForLoggingIn;

    static int inputtedNationalIdToRegister;
    static String inputtedPasswordToRegister;

    static String inputtedFirstNameToRegister;
    static String inputtedSecondNameToRegister;

    static int accountNumberInArray;

    static int storedIdToTrans;
    static int inputtedIdToTrans;

    static int nationalIDforChangingPassword;
    static String passwordForChangingPassword;

    static String newPasswordFirstTime;
    static String newPasswordSecondTime;

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
    static Scanner scanner = new Scanner(System.in);
    static String accountsDataFile = "SSM_System/Data/CustomerAccountsData.ser";
    //
    // Limitations
    static boolean storedFromFile = false;
    static int threeTimesChanceRegisterNationID;
    static int threeTimesChanceLogin;
    static int threeTimesChancePassword;
    static int threeTimesChanceIDforChangingPassword;
    static int threeTimesChanceOldPasswordForChangingPassword;

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
        if (getThreeTimesChanceIDforChangingPassword() < 3) {
            //Create limitation for 3 chances after it exit();
            System.out.println("Enter your Account national ID..");
            try {
                setNationalIDforChangingPassword(scanner.nextInt());
            } catch (Exception e) {
                e.getStackTrace();
                System.out.println("Invalid input.");
                scanner.nextLine();
                inputIDForChangingPassword();
            }
            if ((getNationalIDforChangingPassword() == getInputNationIdForLogin())) {
                inputOldPasswordForChangingPassword();
            } else {
                setThreeTimesChanceIDforChangingPassword(getThreeTimesChanceIDforChangingPassword() + 1);
                System.out.println("National ID Number for the Account is wrong!!");
                inputIDForChangingPassword();
            }
        } else {
            System.out.println("No more chances!!");
            SelfServiceMachine.exit();
            return;
        }
    }

    static private void inputOldPasswordForChangingPassword() throws IOException, InterruptedException, ClassNotFoundException {
        if (getThreeTimesChanceOldPasswordForChangingPassword() < 3) {
            System.out.println("Enter you old password..");
            String oldPassword;
            try {
                oldPassword = scanner.next();
            } catch (Exception e) {
                e.getStackTrace();
                System.out.println("Invalid input.");
                scanner.nextLine();
                inputOldPasswordForChangingPassword();
                return;
            }
            if (Objects.equals(oldPassword, getInputtedPasswordForLoggingIn())) {

                inputNewPasswordForChangingIt();

            } else {
                System.out.println("Invalid password.");
                inputOldPasswordForChangingPassword();
            }
        } else {
            System.out.println("Chances are out.");
            SelfServiceMachine.exit();
        }
    }

    static private void inputNewPasswordForChangingIt() throws IOException, InterruptedException, ClassNotFoundException {

        // Limit the process
        System.out.println("Enter you new password..");
        try {
            newPasswordFirstTime = scanner.next();
        } catch (Exception e) {
            e.getStackTrace();
            System.out.println("Invalid input.");
            scanner.nextLine();
            inputNewPasswordForChangingIt();
            return;
        }
        if (newPasswordFirstTime.length() < 8) {
            System.out.println("Invalid input. Your password should has more than 7 chars.");
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
    }

    static private void inputNewPasswordSecondTime() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter your new password again to confirm..");

        try {
            newPasswordSecondTime = scanner.next();
        } catch (Exception e) {
            e.getStackTrace();
            System.out.println("Invalid input.");
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
        if (!storedFromFile) {
            storedFromFile = true;
            File file = new File(accountsDataFile);
            FileInputStream fIn = null;
            try {
                fIn = new FileInputStream(file);
            } catch (FileNotFoundException e) {
                System.out.println(e);
                file.createNewFile();
                loadAccountsData();
            }
            try (ObjectInputStream objIn = new ObjectInputStream(fIn)) {
                List<CustomerDataManagementBase> deserializedList = null;
                try {
                    deserializedList = (List<CustomerDataManagementBase>) objIn.readObject();
                } catch (ClassNotFoundException | NullPointerException e) {
                    e.getStackTrace();
                }
                accArrayData.clear();
                try {

                    assert deserializedList != null;
                    accArrayData.addAll(deserializedList);
                } catch (NullPointerException e) {
                    e.getStackTrace();
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
                System.out.println("Invalid input,");
                LoginOrRegister();
                break;
        }
    }

    // --- END OF CHOCEEING BETWEEN LOGIN OR REGISTER PROCESS ---
    //
    // --- START OF LOGGING IN PROCESS --
    static private void login() throws InterruptedException, IOException, ClassNotFoundException {
        if (threeTimesChanceLogin < 3) {
            threeTimesChanceLogin++;
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
            System.out.println("Three times chance off");
            SelfServiceMachine.exit();
        }
    }

    private static void getIDInputForLoggingIn() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter your National ID..");
        try {
            setInputNationIdForLogin(scanner.nextInt());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input,");
            getIDInputForLoggingIn();
            return;
        }
    }

    private static void getPasswordInputForLoggingIn() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter your Password..");
        try {
            setInputtedPasswordForLoggingIn(scanner.next());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input,");
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
                registerTheseData = new CustomerDataManagementBase(getInputtedFirstNameToRegister(), getInputtedSecondNameToRegister(), getInputtedNationalIdToRegister(), getInputtedPasswordToRegister());
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
            System.out.println("Invalid input.");
            scanner.nextLine(); // Consume the invalid input
            registerFirstName();
            return;
        }
        if (getInputtedFirstNameToRegister().matches(".*\\d.*")) {
            System.out.println("Invalid input. Please enter letters only!");
            registerFirstName();
        }
    }

    private static void registerSecondName() {
        // limit the process
        System.out.println("Enter you second name..");
        try {
            setInputtedSecondNameToRegister(scanner.next());
        } catch (Exception e) {
            System.out.println("Invalid input,");
            scanner.nextLine();
            registerFirstName();
            return;
        }
        if (getInputtedFirstNameToRegister().matches(".*\\d.*")) {
            System.out.println("Invalid input. Please enter letters only!");
            registerSecondName();
        }
    }

    private static void registerNationID() throws InterruptedException, IOException, ClassNotFoundException {
        if (threeTimesChanceRegisterNationID < 3) {
            threeTimesChanceRegisterNationID++;
            System.out.println("Enter your National ID (exactly 8 digits)..");
            try {
                setInputtedNationalIdToRegister(scanner.nextInt());
            } catch (InputMismatchException e) {
                System.out.println("Invalid input,");
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
            System.out.println("Three times chance off");
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
                System.out.println("You have been registered already! \n");
                login();
                return;
            }
        }
    }

    private static void registerPassword() throws InterruptedException, IOException, ClassNotFoundException {
        if (threeTimesChancePassword < 3) {
            threeTimesChancePassword++;
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
                System.out.println("Invalid input,");
                scanner.nextLine();
                registerPassword();
                return;
            }
        } else {
            System.out.println("Three times chance off");
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

    public static int getThreeTimesChanceRegisterNationID() {
        return threeTimesChanceRegisterNationID;
    }

    public static void setThreeTimesChanceRegisterNationID(int threeTimesChanceRegisterNationID) {
        CustomerDataManagementBase.threeTimesChanceRegisterNationID = threeTimesChanceRegisterNationID;
    }

    public static int getThreeTimesChanceLogin() {
        return threeTimesChanceLogin;
    }

    public static void setThreeTimesChanceLogin(int threeTimesChanceLogin) {
        CustomerDataManagementBase.threeTimesChanceLogin = threeTimesChanceLogin;
    }

    public static int getThreeTimesChancePassword() {
        return threeTimesChancePassword;
    }

    public static void setThreeTimesChancePassword(int threeTimesChancePassword) {
        CustomerDataManagementBase.threeTimesChancePassword = threeTimesChancePassword;
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

    public static int getThreeTimesChanceIDforChangingPassword() {
        return threeTimesChanceIDforChangingPassword;
    }

    public static void setThreeTimesChanceIDforChangingPassword(int threeTimesChanceIDforChangingPassword) {
        CustomerDataManagementBase.threeTimesChanceIDforChangingPassword = threeTimesChanceIDforChangingPassword;
    }

    public static int getThreeTimesChanceOldPasswordForChangingPassword() {
        return threeTimesChanceOldPasswordForChangingPassword;
    }

    public static void setThreeTimesChanceOldPasswordForChangingPassword(int threeTimesChanceOldPasswordForChangingPassword) {
        CustomerDataManagementBase.threeTimesChanceOldPasswordForChangingPassword = threeTimesChanceOldPasswordForChangingPassword;
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

    @Override
    public String toString() {
        return "\n FirstName: " + getFirstName() + "\n  LastName: " + getLastName() + "\nNationalID: " + getNationalID() + "\n   Balance: " + getBalance() + "$\n  Password: " + getPassword() + "\n--------\n";
    }
}
