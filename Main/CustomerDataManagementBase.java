import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;

import java.io.*;
import java.util.*;
import java.io.Serializable;


class CustomerDataManagementBase implements Serializable {
    @Serial
    private static final long serialVersionUID = 1L;
    //
    // Process helpers
    private static int storedNationalID;
    private static String storedPassword;

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
    private static String accountsDataFile = "SSM_System/Data/TreeData.json";
    static final ObjectMapper objectMapper = new ObjectMapper();
    static final String ANSI_GREEN = "\u001B[32m";
    static final String ANSI_RED = "\u001B[31m";
    static final String ANSI_RESET = "\u001B[0m";
    static final String BOLD = "\u001B[1m";
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
    private static int limitRegisterFirstName;
    private static int limitRegisterSecondName;

    //
    //
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
            System.out.println("Enter your Account national ID..\n");
            try {
                setNationalIDforChangingPassword(scanner.nextInt());
            } catch (Exception e) {
                e.getStackTrace();
                System.out.println("Invalid input!!");
                scanner.nextLine();
                inputIDForChangingPassword();
            }
            if ((getNationalIDforChangingPassword() == BinaryTree.getSearchMethodArray().get(0).getNationalID())) {
                inputOldPasswordForChangingPassword(); // continue
            } else {
                setLimitIDForChangingPassword(getLimitIDForChangingPassword() + 1);
                System.out.println("National ID Number for the Account is wrong.");
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
            System.out.println("Enter you old password..\n");
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
            if (Objects.equals(oldPassword, BinaryTree.getSearchMethodArray().get(0).getPassword())) {

                inputNewPasswordForChangingIt(); // continue

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

            System.out.println("Enter you new password..\n");
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

                if (!newPasswordFirstTime.equals(BinaryTree.getSearchMethodArray().get(0).getPassword())) {
                    inputNewPasswordSecondTime(); // Continue
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
        System.out.println("Enter your new password again to confirm..\n");

        try {
            newPasswordSecondTime = scanner.next();
        } catch (Exception e) {
            e.getStackTrace();
            System.out.println("Invalid input!!");
            scanner.nextLine();
            inputNewPasswordSecondTime(); // continue
            return;
        }

        if (Objects.equals(newPasswordFirstTime, newPasswordSecondTime)) {
            changeOldPasswordFinalStep(); // Continue
        } else {
            // Repeat the process
            System.out.println("Does not match!!");
            scanner.nextLine();
            inputNewPasswordForChangingIt();
        }
    }

    static private void changeOldPasswordFinalStep() throws IOException, InterruptedException, ClassNotFoundException {
        // Update password
        BinaryTree.getSearchMethodArray().get(0).setPassword(getNewPasswordSecondTime());
        // Update the new changes
        BinaryTree.updateNewChanges();
        System.out.println(ANSI_GREEN + "\nChanged your password successfully!" + ANSI_RESET + BOLD);
        SelfServiceMachine.semiUI();
    }
    // --- END OF UPDATING PASSWORD PROCESS ---
    //
    // --- START OF LOADING DATA PROCESSES ---

    static protected void loadAccountsData() throws IOException, ClassNotFoundException {
        if (!storedFromFileAccountsData) {
            storedFromFileAccountsData = true;
            handleDataFile(); // make sure of the data file
            objectMapper.enable(SerializationFeature.INDENT_OUTPUT);

            BinaryTree.loadTreeData();
            System.out.println("-----");
            BinaryTree.displayTree();
        }
    }

    private static void handleDataFile() throws IOException {
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
    }

    // --- END OF LOADING DATA PROCESSES ---
    //
    // --- START OF CHOCEEING BETWEEN LOGIN OR REGISTER PROCESS ---
    protected static void LoginOrRegister() throws InterruptedException, IOException, ClassNotFoundException {
        if (SelfServiceMachine.isFinalCustomerPassed) {
            System.out.println("In order to use our SSM Please pick an order,\nFrom our Organizing Machine, First");
            return;
        }

        if (OrganizingMachine.rear == null && OrganizingMachine.front == null) {
            System.out.println("In order to use our SSM Please pick an order,\nUsing the Organizing Machine.");
            return;
        }

        if (OrganizingMachine.getRearMinusFront() < 0) {
            return;
        }

        if (OrganizingMachine.getRearMinusFront() == 0) {
            SelfServiceMachine.isFinalCustomerPassed = true;
        }
        final String BOLD = "\u001B[1m";
        System.out.println(BOLD + "==========================\nNext customer, number: " + OrganizingMachine.front.data + " ");
        System.out.println("Remaining customers: " + OrganizingMachine.getRearMinusFront() + "\n");
        getInputForLoginOrRegister(); // if passed continue
    }

    private static void getInputForLoginOrRegister() throws IOException, InterruptedException, ClassNotFoundException {

        System.out.println("Welcome to our SSM System.");
        System.out.println(ANSI_RED + "Press 1 for Log in to your account.");
        System.out.println("Press 2 Register for a new account.\n" + ANSI_RESET + BOLD);

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
        setLimitLogin(getLimitLogin() + 1);
        if (getLimitLogin() >= 3) {
            System.out.println("Chances out!!");
            SelfServiceMachine.exit();
            return;
        }

        try {
            loadAccountsData();
        } catch (Exception e) {
            e.printStackTrace();
        }
        System.out.println("Here you can Login.");
        getIDInputForLoggingIn(); // continue

    }

    private static void getIDInputForLoggingIn() throws IOException, InterruptedException, ClassNotFoundException {
        setLimitLoginProcess(getLimitLoginProcess() + 1);
        if (getLimitLoginProcess() >= 5) {
            System.out.println("Chances out!!");
            SelfServiceMachine.exit();
            return;
        }
        System.out.println("Enter your National ID..\n");
        try {
            setInputNationIdForLogin(scanner.nextInt());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input!!");
            getIDInputForLoggingIn();
            return;
        }
        getPasswordInputForLoggingIn(); // continue
    }

    private static void getPasswordInputForLoggingIn() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter your Password..\n");
        try {
            setInputtedPasswordForLoggingIn(scanner.next());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input!!");
            getPasswordInputForLoggingIn();
            return;
        }
        // then
        checkAccountValidity(getInputNationIdForLogin(), getInputtedPasswordForLoggingIn());
    }


    private static void checkAccountValidity(int inputNationIdForLogin, String inputPasswordForLogin) throws IOException, InterruptedException, ClassNotFoundException {
        BinaryTree.searchOnTree(inputNationIdForLogin);
        // if national id does not exist
        if (BinaryTree.getSearchMethodArray().isEmpty()) {
            System.out.println("National ID or password is incorrect!\n");
            setLimitLoginProcess(getLimitLoginProcess() + 1); // Chances --;
            getIDInputForLoggingIn();
            return;
        }
        // if national id and password doesn't match
        if (BinaryTree.getSearchMethodArray().get(0).getNationalID() != inputNationIdForLogin ||
                !BinaryTree.getSearchMethodArray().get(0).getPassword().equals(inputPasswordForLogin)) {
            System.out.println("National ID or password is incorrect!\n");
            setLimitLoginProcess(getLimitLoginProcess() + 1); // Chances --;
            getIDInputForLoggingIn();
            return;
        }
        System.out.println(ANSI_GREEN + "\n\nSuccessfully logged in!" + ANSI_RESET + BOLD);
        SelfServiceMachine.mainUI(); // if passed continue
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

        registerFirstName(); // Start
    }

    private static void registerFirstName() throws IOException, InterruptedException, ClassNotFoundException {
        setLimitRegisterFirstName(getLimitRegisterFirstName() + 1);
        if (getLimitRegisterFirstName() > 5) {
            System.out.println("Chances out!!");
            SelfServiceMachine.exit();
        }

        System.out.println("Enter your first name..\n");
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
            return;
        }

        registerSecondName(); // if passed continue
    }

    private static void registerSecondName() throws IOException, InterruptedException, ClassNotFoundException {

        setLimitRegisterSecondName(getLimitRegisterSecondName() + 1); // Chances --
        if (getLimitRegisterSecondName() > 5) {
            System.out.println("Chances out!!");
            SelfServiceMachine.exit();
        }

        System.out.println("Enter you second name..\n");
        try {
            setInputtedSecondNameToRegister(scanner.next());
        } catch (Exception e) {
            System.out.println("Invalid input!!");
            scanner.nextLine();
            registerFirstName();
            return;
        }

        if (getInputtedSecondNameToRegister().matches(".*\\d.*")) {
            System.out.println("Invalid input!! Please enter letters only!");
            registerSecondName();
            return;
        }
        registerNationID(); // if passed continue
    }

    private static void registerNationID() throws InterruptedException, IOException, ClassNotFoundException {
        setLimitRegisterNationID(getLimitRegisterNationID() + 1);
        if (getLimitRegisterNationID() >= 3) {
            System.out.println("Three chances out!!");
            SelfServiceMachine.exit();
            return;
        }

        System.out.println("Enter your National ID (exactly 8 digits)..\n");
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
        checkIfNationIdUnique(); // continue
    }

    private static void checkIfNationIdUnique() throws InterruptedException, IOException, ClassNotFoundException {
        BinaryTree.searchOnTree(getInputtedNationalIdToRegister());
        if (BinaryTree.getSearchMethodArray().isEmpty()) {
            registerPassword(); // continue
            return;
        }

        if (getInputtedNationalIdToRegister() == BinaryTree.getSearchMethodArray().get(0).getNationalID()) {
            System.out.println("You have an account already! \n");
            getInputForLoginOrRegister();
            return;
        }
    }

    private static void registerPassword() throws InterruptedException, IOException, ClassNotFoundException {
        if (getLimitPassword() < 3) {
            setLimitPassword(getLimitPassword());
            System.out.println("Enter Your Password..(Equal to or more than 8 chars.)\n");
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
        completeRegistration();  // if passed continue
    }

    private static void completeRegistration() throws IOException, InterruptedException, ClassNotFoundException {
        //if unique ID continue register password
        CustomerDataManagementBase customerDataManagementBase1 = new CustomerDataManagementBase(
                getInputtedFirstNameToRegister(), getInputtedSecondNameToRegister(), getInputtedNationalIdToRegister(),
                getInputtedPasswordToRegister()
        );
        BinaryTree.insertOnTheTree(customerDataManagementBase1);
        BinaryTree.storeTreeData();
        System.out.println(ANSI_GREEN + "\n\n=========================\nRegistered Successfully!!\n" + ANSI_RESET + BOLD);
        login();
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
        return storedPassword;
    }

    public static void setStoredPassword(String storedpassword) {
        CustomerDataManagementBase.storedPassword = storedpassword;
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

    public static int getLimitRegisterFirstName() {
        return limitRegisterFirstName;
    }

    public static void setLimitRegisterFirstName(int limitRegisterFirstName) {
        CustomerDataManagementBase.limitRegisterFirstName = limitRegisterFirstName;
    }

    public static int getLimitRegisterSecondName() {
        return limitRegisterSecondName;
    }

    public static void setLimitRegisterSecondName(int limitRegisterSecondName) {
        CustomerDataManagementBase.limitRegisterSecondName = limitRegisterSecondName;
    }

    @Override
    public String toString() {
        return "\n FirstName: " + getFirstName() + "\n  LastName: " + getLastName() + "\nNationalID: " + getNationalID() + "\n   Balance: " + getBalance() + "$\n  Password: " + getPassword() + "\n--------\n";
    }
}
