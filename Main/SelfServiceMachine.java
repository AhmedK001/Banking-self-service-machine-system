import java.io.IOException;
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
    static double amountToTrans;
    static double amountToWithdraw;
    static double amountToDeposit;
    //
    //
    static Scanner scanner = new Scanner(System.in);
    static final String BOLD = "\u001B[1m";
    static final String ANSI_RED = "\u001B[31m";
    static final String ANSI_RESET = "\u001B[0m";

    public SelfServiceMachine() {

    }

    public static void main(String[] args) throws InterruptedException, IOException, ClassNotFoundException {
        OrganizingMachine.loadQueueDataFromFile(); //For loading the old queue data if any
        loadAccountsData(); //For loading the customers accounts data from json
        LoginOrRegister();
    }

    static void mainUI() throws InterruptedException, IOException, ClassNotFoundException {
        if (mainUILimit < 6) {
            mainUILimit++;
            System.out.println("============================================");
            System.out.println(ANSI_RED + "We are providing these services only for you:\n\n");
            System.out.println("Press 1 for withdraw\nPress 2 for deposit");
            System.out.println("Press 3 for balance\nPress 4 to transfer\nPress 9 to update password");
            System.out.println("\nPress 0 to log out\n" + ANSI_RESET + BOLD);
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
            System.out.println("Press 1 return to the main menu\nPress 0 to log out\n");
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

        System.out.println("Enter the ID number of the person you want to transfer to\n");
        try {
            setInputtedIdToTrans(scanner.nextInt());
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            System.out.println("Invalid input!!");
            inputIDToTransfer();
            return;
        }
        BinaryTree.searchOnTreeForReceiver(getInputtedIdToTrans());
        // Method above mentions checkExistsForReceiverAccount() when finish
    }

    static void checkExistsForReceiverAccount() throws IOException, InterruptedException, ClassNotFoundException {

        if ((getInputtedIdToTrans() == BinaryTree.getSearchMethodArrayForReceiver().get(0).getNationalID()) && (BinaryTree.getSearchMethodArray().get(0).getNationalID() != BinaryTree.getSearchMethodArrayForReceiver().get(0).getNationalID())) {
            inputAmountToTransfer(); // Continue if found receiver account
            return;
        } else if (getInputtedIdToTrans() == BinaryTree.getSearchMethodArray().get(0).getNationalID()) {
            System.out.println("Cannot transfer to yourself");
            semiUI();
            return;
        }
        System.out.println("No account exists under this ID number");
        semiUI();
    }

    static private void inputAmountToTransfer() throws IOException, InterruptedException, ClassNotFoundException {
        amountToTransferChances++;
        if (amountToTransferChances > 5) {
            System.out.println("Five chances out!!");
            return;
        }
        System.out.println("Enter amount to transfer to the selected account..\n");
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

        double mainCustomerBalance = BinaryTree.getSearchMethodArray().get(0).getBalance();
        if (amountToTrans > mainCustomerBalance) {
            System.out.println("Your balance is not enough!");
            System.out.println("Your balance is: " + mainCustomerBalance + "$\n");
            semiUI();
            return;
        }
        double receiverCustomerBalance = BinaryTree.getSearchMethodArrayForReceiver().get(0).getBalance();
        // Update the main user balance
        double balanceAfterDeduct = mainCustomerBalance - amountToTrans;
        BinaryTree.getSearchMethodArray().get(0).setBalance(balanceAfterDeduct);
        BinaryTree.updateNewChanges();
        // Update the receiver balance
        double balanceAfterAdded = receiverCustomerBalance + amountToTrans;
        BinaryTree.getSearchMethodArrayForReceiver().get(0).setBalance(balanceAfterAdded);
        BinaryTree.updateNewChangesForReceiver();

        System.out.println("Transferred successfully.");

        semiUI();
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
        System.out.println("Enter amount to withdraw..\n");
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
        if (amountToWithdraw <= BinaryTree.getSearchMethodArray().get(0).getBalance()) {
            withdraw(); // Continue process
        } else {
            System.out.println("Your balance is not enough!");
            System.out.println("Your balance is: " + BinaryTree.getSearchMethodArray().get(0).getBalance() + "$\n");
            mainUI();
        }
    }

    static protected void withdraw() throws InterruptedException, IOException, ClassNotFoundException {
        // Current balance - withdrawn amount
        balance = BinaryTree.getSearchMethodArray().get(0).getBalance();
        balance = BinaryTree.getSearchMethodArray().get(0).getBalance() - amountToWithdraw;
        BinaryTree.getSearchMethodArray().get(0).setBalance(balance);
        System.out.println("Your new balance is: " + BinaryTree.getSearchMethodArray().get(0).getBalance() + "$");
        // Save the new account updates
        BinaryTree.updateNewChanges();
        semiUI();
    }

    static protected void deposit() throws InterruptedException, IOException, ClassNotFoundException {
        inputForDeposit();
        balance = amountToDeposit + BinaryTree.getSearchMethodArray().get(0).getBalance();
        // Update the new balance
        BinaryTree.getSearchMethodArray().get(0).setBalance(balance);
        BinaryTree.updateNewChanges();

        System.out.println("Your new balance is: " + BinaryTree.getSearchMethodArray().get(0).getBalance() + "$\n");
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
        System.out.println("Enter amount to deposit..\n");
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
        } // If passed until here Continue process
    }

    static protected void balance() throws InterruptedException, IOException, ClassNotFoundException {
        System.out.println("Your balance is: " + BinaryTree.getSearchMethodArray().get(0).getBalance() + "$\n");
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
        setLimitRegisterFirstName(0);
        setLimitRegisterSecondName(0);
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
            System.out.println("In order to use our SSM Please pick an order.\nFrom the Organizing Machine, First");
        }
    }

}