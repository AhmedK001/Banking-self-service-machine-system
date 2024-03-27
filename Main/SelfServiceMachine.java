import java.io.IOException;
import java.util.ArrayList;
import java.util.Scanner;


class SelfServiceMachine extends CustomerDataManagementBase {

    static double balance;
    //
    // Limitations
    static int semiUILimit = 0;
    static int mainUILimit = 0;
    static int depositeProcessLimit = 0;
    static int withdrawProcessLimit = 0;
    static int transferProcessLimit = 0;
    static int amountToTransferChances = 0;
    //
    // Process helpers
    static int mainTurn = 0;
    static boolean isFinalCustomerPassed = false;
    static int searchHelper;
    static int indexForReceiver;
    static double amountToTrans;
    static double amountToWithdraw;
    static double amountToDeposit;
    static double mainCustomerBalance;
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
                    inputAmountToWithdraw();
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
        transferProcessLimit++;
        if (transferProcessLimit > 5) {
            System.out.println("Five chances out!!");
            exit();
            return;
        }

        System.out.println("Enter the ID number of the person you want to transfer to");
        try {
            setInputtedIdToTrans(scanner.nextInt());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input!!");
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
                inputAmountToTransfer();
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

    static private void inputAmountToTransfer() throws IOException, InterruptedException, ClassNotFoundException {
        amountToTransferChances++;
        if (amountToTransferChances > 5) {
            System.out.println("Five chances out!!");
            return;
        }
        System.out.println("Enter amount to transfer to the selected account..");
        try {
            amountToTrans = scanner.nextDouble();
        } catch (Exception e) {
            e.getStackTrace();
            e.getStackTrace();
            scanner.nextLine();
            inputAmountToTransfer();
            return;
        }
        if (amountToTrans <= 10) {
            System.out.println("You Cannot transfer less than 10$");
            inputAmountToTransfer();
            return;
        }
        if (amountToTrans > 50000) {
            System.out.println("You cannot transfer more than 50000$ per time");
            inputAmountToTransfer();
            return;
        }
        continueTransferProcess(); // if passed all conditions continue
    }

    static private void continueTransferProcess() throws IOException, InterruptedException, ClassNotFoundException {

        double mainCustomerBalance = customer.getBalance();
        if (amountToTrans > mainCustomerBalance) {
            System.out.println("Your balance is not enough!");
            System.out.println("Your balance is: " + mainCustomerBalance + "$\n");
            semiUI();
            return;
        }
        double toTransCustomerBalance = accArrayData.get(searchHelper).getBalance();
        double balanceAfterDeduct = mainCustomerBalance - amountToTrans;

        updateUserBalance((ArrayList<CustomerDataManagementBase>) accArrayData, getInputNationIdForLogin(), balanceAfterDeduct);
        customer.setBalance(balanceAfterDeduct);
        double balanceAfterAdded = toTransCustomerBalance + amountToTrans;

        searchSenderAccount(); // searchSenderAccount
        updateReceiverBalance((ArrayList<CustomerDataManagementBase>) accArrayData, SelfServiceMachine.getIndexForReceiver(), balanceAfterAdded);

        System.out.println("Transferred successfully.");

        semiUI();
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

    static private void inputAmountToWithdraw() throws IOException, InterruptedException, ClassNotFoundException {
        withdrawProcessLimit++;
        if (withdrawProcessLimit > 5) {
            System.out.println("Five chances out!!");
            exit();
            return;
        }
        System.out.println("Enter amount to withdraw..");
        try {
            amountToWithdraw = scanner.nextDouble();
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            inputAmountToWithdraw();
            return;
        }
        if (amountToWithdraw > 5000) {
            System.out.println("You cannot withdraw more than 5000$ per time");
            inputAmountToWithdraw();
            return;
        }
        if (!((amountToWithdraw % 50 == 0 || amountToWithdraw % 100 == 0) && amountToWithdraw != 0)) {
            System.out.println("Please enter a number multiples of 50 or 100..");
            inputAmountToWithdraw();
            return;
        }
        if (amountToWithdraw <= customer.getBalance()) {
            withdraw(); // Continue process
        } else {
            System.out.println("Your balance is not enough!");
            System.out.println("Your balance is: " + customer.getBalance() + "$\n");
            mainUI();
        }
    }

    static protected void withdraw() throws InterruptedException, IOException, ClassNotFoundException {
        balance = customer.getBalance();
        balance = customer.getBalance() - amountToWithdraw;
        customer.setBalance(balance);
        updateUserBalance((ArrayList<CustomerDataManagementBase>) accArrayData, getInputNationIdForLogin(), customer.getBalance());
        System.out.println("Your new balance is: " + customer.getBalance() + "$");
        savaAccountsData();
        semiUI();
    }

    static protected void deposit() throws InterruptedException, IOException, ClassNotFoundException {
        inputForDeposit();
        balance = amountToDeposit + customer.getBalance();
        customer.setBalance(balance);
        updateUserBalance((ArrayList<CustomerDataManagementBase>) accArrayData, getInputNationIdForLogin(), customer.getBalance());
        savaAccountsData();
        System.out.println("Your new balance is: " + customer.getBalance() + "$\n");
        Thread.sleep(200);
        semiUI();
    }

    static private void inputForDeposit() throws IOException, InterruptedException, ClassNotFoundException {
        depositeProcessLimit++;
        if (depositeProcessLimit > 5) {
            System.out.println("Five chances out!!");
            exit();
            return;
        }
        System.out.println("Enter amount to deposit..");
        try {
            amountToDeposit = scanner.nextDouble();
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            inputForDeposit();
            return;
        }
        if (amountToDeposit > 150000) {
            System.out.println("You cannot deposit more than 150000$ per time");
            inputForDeposit();
            return;
        }
        if (!((amountToDeposit % 50 == 0 || amountToDeposit % 100 == 0) && amountToDeposit != 0)) {
            System.out.println("Please enter a number multiples of 50 or 100..");
            inputForDeposit();
            return; // if needed
        } // If passed until here Continue process
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
        transferProcessLimit = 0;
        setLimitLogin(0);
        setLimitPassword(0);
        setLimitRegisterNationID(0);
        setLimitIDForChangingPassword(0);
        setLimitOldPasswordToChangingPassword(0);
        setLimitLoginProcess(0);
        setLimitNewPasswordForChangingIt(0);

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