import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;


    /*
    * System Overview: (Part 2) *
    * Integration with Customer Data Management *

    The primary machinery, or SSM, operates in conjunction with the CustomerDataManagement System
    for comprehensive management of customer-related data. This enables the Self Service Machine to offer:

    - A unique, bank-independent operation.
    - A wide range of operations eliminating the need for human intervention.
    - Users can either create a new account or log into an existing one,
          availing themselves of various banking services. Continuous enhancements are underway to include more operations.

    * Enhancing SSM Dynamics *
    To augment the dynamism of the SSM:
    - Current Data Management: Customer data is currently managed using an array list,
           with plans to transition to a binary tree structure to optimize search times to O(log n).
    - Recursion and Real-time Data Handling: The SSM employs recursion for flexibility
           and manages customer data in real-time, necessitating no restarts for data updates.
    - Integration and Security Measures: It seamlessly interacts with both the Organizing Machine
           and the CustomerDataManagementSystem, incorporates measures against misuse,
           and employs advanced exception handling techniques.


                --(* Try it yourself!! *)--


    Next Step
    Please proceed to Part 3 for further exploration of the system’s capabilities and advancements.

*/


class SelfServiceMachine extends CustomerDataManagementBase {

    static double balance;
    //
    // Limitations
    static int semiUILimit = 0;
    static int mainUILimit = 0;
    static int depositeProcessLimit = 0;
    static int withdrawProcessLimit = 0;
    static int transferProcessLimit = 0;
    //
    // Process helpers
    static int mainTurn = 0;
    static boolean isFinalCustomerPassed = false;
    static int searchHelper;
    static int indexForReceiver;

    //
    // Node identifiers
    static Node front;
    static Node rear;
    //
    //
    static SelfServiceMachine selfServiceMachine = new SelfServiceMachine();
    static Scanner scanner = new Scanner(System.in);

    public SelfServiceMachine(int nationalID, String password) {
        super(nationalID, password);
    }

    public SelfServiceMachine() {

    }

    public static void main(String[] args) throws InterruptedException, IOException, ClassNotFoundException {
        OrganizingMachine.loadQueueDataFromFile();
        loadAccountsData();
        LoginOrRegister();
    }

    static void mainUI() throws InterruptedException, IOException, ClassNotFoundException {
        if (mainUILimit < 6) {
            mainUILimit++;
            System.out.println("press 1 for withdraw\npress 2 for deposit");
            System.out.println("press 3 for balance\npress 4 to transfer\n\npress 9 to update password");
            System.out.println("press 0 to exit");
            int yoInput;
            try {
                yoInput = scanner.nextInt();
            } catch (Exception e) {
                e.getStackTrace();
                scanner.nextLine();
                mainUI();
                return;
            }

            switch (yoInput) {
                case 1:
                    withdraw();
                    break;
                case 2:
                    deposit();
                    break;
                case 3:
                    balance();
                    break;
                case 4:
                    transferMoney();
                    break;
                case 9:
                    changePassword();
                    break;
                case 0:
                    exit();
                    break;
                default:
                    System.out.println("Invalid option. Please try again.");
                    mainUI();
                    break;
            }
        } else {
            exit();
        }
    }

    static void semiUI() throws InterruptedException, IOException, ClassNotFoundException {
        if (semiUILimit < 6) {
            semiUILimit++;
            System.out.println("Press 1 return to the main menu\nPress 0 to exit");
            int yoInput;
            try {
                yoInput = scanner.nextInt();
            } catch (Exception e) {
                e.getStackTrace();
                scanner.nextLine();
                mainUI();
                return;
            }
            switch (yoInput) {
                case 1:
                    mainUI();
                    break;
                case 0:
                    exit();
                    break;
                default:
                    System.out.println("Invalid option. Please try again.");
                    semiUI();
            }
        } else {
            exit();
        }
    }

    //
    // --- Start OF TRANSFER PROCESS ---
    static protected void transferMoney() throws IOException, InterruptedException, ClassNotFoundException {
        if (transferProcessLimit < 6) {
            inputIDToTransfer();
        }
    }

    static private void inputIDToTransfer() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter the ID number of the person you want to transfer to");
        try {
            setInputtedIdToTrans(scanner.nextInt());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input,");
            inputIDToTransfer();
            return;
        }
        checkExistsForReceiverAccount();
    }

    static private void checkExistsForReceiverAccount() throws IOException, InterruptedException, ClassNotFoundException {
        int localSearchHelper;
        localSearchHelper = getSearchHelper();
        for (localSearchHelper = 0; localSearchHelper < accArrayData.size(); localSearchHelper++) {
            setStoredIdToTrans(accArrayData.get(localSearchHelper).getNationalID());

            if ((getInputtedIdToTrans() == getStoredIdToTrans()) && (getInputNationIdForLogin() != getInputtedIdToTrans())) {

                setSearchHelper(localSearchHelper);
                System.out.println("Founded Receiver account successfully.");
                continueTransferProcess();
                return;
            } else if (getInputNationIdForLogin() == getInputtedIdToTrans()) {

                System.out.println("Cannot transfer to yourself");
                semiUI();
                return;
            }
        }
        System.out.println("No accounts exists under this ID number");
        semiUI();
    }


    static private void continueTransferProcess() throws IOException, InterruptedException, ClassNotFoundException {
        System.out.println("Enter the amount to transfer to account ID number: " + getInputtedIdToTrans() + " ||");
        double amountToTrans;
        try {
            amountToTrans = scanner.nextDouble();
        } catch (Exception e) {
            e.getStackTrace();
            e.getStackTrace();
            scanner.nextLine();
            transferMoney();
            return;
        }
        double mainCustomerBalance = customer.getBalance();
        if (amountToTrans > 10) {
            if (amountToTrans <= mainCustomerBalance) {
                double toTransCustomerBalance = accArrayData.get(searchHelper).getBalance();
                double balanceAfterDeduct = mainCustomerBalance - amountToTrans;

                updateUserBalance((ArrayList<CustomerDataManagementBase>) accArrayData, getInputNationIdForLogin(), balanceAfterDeduct);
                double balanceAfterAdded = toTransCustomerBalance + amountToTrans;

                searchSenderAccount(); // searchSenderAccount
                updateReceiverBalance((ArrayList<CustomerDataManagementBase>) accArrayData, SelfServiceMachine.getIndexForReceiver(), balanceAfterAdded);

                System.out.println("Transferred successfully.");
            } else {
                System.out.println("Your balance is not enough!");
                System.out.println("Your balance is: " + mainCustomerBalance + "$\n");
            }
        } else {
            System.out.println("Cannot transfer less than 10$");
        }
        semiUI();
        return;
    }

    static private void searchSenderAccount() {
        int i;
        for (i = 0; i < accArrayData.size(); i++) {
            int searchReceiverIndex = 0;
            int receiverID = accArrayData.get(i).getNationalID();
            if (getInputtedIdToTrans() == receiverID) {
                selfServiceMachine.setIndexForReceiver(i);
                //System.out.println("Found Receiver Index ID: " + searchReceiverIndex);
                break;
            }
        }
    }
    // --- END OF TRANSFER PROCESS ---
    // ---------------------------------

    static protected void withdraw() throws InterruptedException, IOException, ClassNotFoundException {
        System.out.println("Enter the amount to withdraw..");
        double withdrawInput;
        try {
            withdrawInput = scanner.nextDouble();
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            withdraw();
            return;
        }

        if (withdrawProcessLimit < 6) {
            withdrawProcessLimit++;
            if (withdrawInput <= customer.getBalance()) {
                if ((withdrawInput % 50 == 0 || withdrawInput % 100 == 0) && withdrawInput != 0) {
                    balance = customer.getBalance();
                    balance = customer.getBalance() - withdrawInput;
                    customer.setBalance(balance);
                    updateUserBalance((ArrayList<CustomerDataManagementBase>) accArrayData, getInputNationIdForLogin(), customer.getBalance());
                    System.out.println("Your new balance is: " + customer.getBalance() + "$");
                    savaAccountsData();
                    semiUI();
                } else {
                    System.out.println("Please enter a number multiples of 50 or 100..");
                    withdraw();
                }
            } else {
                System.out.println("Your balance is not enough!");
                System.out.println("Your balance is: " + customer.getBalance() + "$\n");
                mainUI();
            }
        } else {
            exit();
        }
    }

    static protected void deposit() throws InterruptedException, IOException, ClassNotFoundException {
        System.out.println("Please enter amount to deposit..");
        double depositInput;
        try {
            depositInput = scanner.nextDouble();
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            deposit();
            return;
        }
        depositeProcessLimit++;
        if (depositeProcessLimit < 6) {
            if ((depositInput % 50 == 0 || depositInput % 100 == 0) && depositInput != 0) {
                balance = depositInput + customer.getBalance();
                customer.setBalance(balance);
                updateUserBalance((ArrayList<CustomerDataManagementBase>) accArrayData, getInputNationIdForLogin(), customer.getBalance());
                savaAccountsData();
                System.out.println("Your new balance is: " + customer.getBalance() + "$\n");
                Thread.sleep(200);
                semiUI();
            } else {
                System.out.println("Please enter a number multiples of 50 or 100..");
                deposit();
            }
        } else {
            exit();
        }
    }

    static protected void balance() throws InterruptedException, IOException, ClassNotFoundException {
        System.out.println("Your balance is: " + customer.getBalance() + "$\n");
        Thread.sleep(200);
        semiUI();
    }

    static private void resetLimits() throws InterruptedException, IOException, ClassNotFoundException {
        semiUILimit = 0;
        mainUILimit = 0;
        depositeProcessLimit = 0;
        withdrawProcessLimit = 0;
        setThreeTimesChanceLogin(0);
        setThreeTimesChancePassword(0);
        setThreeTimesChanceRegisterNationID(0);
        balance = customer.getBalance();
        mainTurn--;
        LoginOrRegister();
    }

    static protected void exit() throws InterruptedException, IOException, ClassNotFoundException {
        System.out.println("Thanks for using our SSM.");

        if (!isFinalCustomerPassed) {
            OrganizingMachine.dequeue();
            Thread.sleep(100);
            resetLimits();
        } else {
            OrganizingMachine.resetQueueDataFile();
            System.out.println("In order to use our SSM Please pick an order.\nUsing the Organizing Machine.");
        }
    }

    public static int getIndexForReceiver() {
        return indexForReceiver;
    }

    public void setIndexForReceiver(int indexForReceiver) {
        SelfServiceMachine.indexForReceiver = indexForReceiver;
    }

    public static int getSearchHelper() {
        return searchHelper;
    }

    public static void setSearchHelper(int searchHelper) {
        SelfServiceMachine.searchHelper = searchHelper;
    }
}