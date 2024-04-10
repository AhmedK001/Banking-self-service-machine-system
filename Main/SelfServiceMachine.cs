using System.Threading.Channels;

namespace SSM_IN_C_Sharp_;

using System;
using System.IO;

public class SelfServiceMachine : UserDataManager
{
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
    static double amountToTrans;
    static double amountToWithdraw;

    static double amountToDeposit;

    //
    //
    public static readonly string BOLD = "\u001B[1m";
    public static readonly string ANSI_RESET = "\u001B[0m";
    public static readonly string ANSI_RED = "\u001B[31m";
    public static readonly string ANSI_BRIGHT_GREEN = "\u001B[92m";
    public static readonly string ANSI_BRIGHT_WHITE = "\u001B[97m";

    static BankStatement _bankStatement = new BankStatement();

    public SelfServiceMachine()
    {
    }

    public static void Main(string[] args)
    {
        DataManager dataManager = new DataManager(_bankStatement);
        LoadAccountsData(); // For loading the customers accounts data from json
        dataManager.LoadStatementData();
        startPoint(); // from here we start
    }

    protected static void startPoint()
    {
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "========================");
        Console.WriteLine(ANSI_BRIGHT_GREEN + "Welcome press 0 to start.\n\n" + ANSI_RESET);
        
        InputForStartPoint();
    }

    private static void InputForStartPoint()
    {
        int startPointInput;
        try
        {
            startPointInput = int.Parse(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input.");
            startPoint();
            return;
        }

        ChooseOfStartPoint(startPointInput); // continue
    }

    private static void ChooseOfStartPoint(int startPointInput)
    {
        switch (startPointInput)
        {
            case 0:
                LoginOrRegister();
                break;
            default:
                startPoint();
                break;
        }
    }

    public static void MainUi()
    {
        if (mainUILimit >= 6)
        {
            Exit();
        }

        mainUILimit++;
            Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "==================================" + ANSI_RESET);
            Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "We are providing these services only for you:\n\n");
            Console.WriteLine(ANSI_BRIGHT_GREEN + "Press 1 for withdraw\nPress 2 for deposit");
            Console.WriteLine(
                "Press 3 for balance\nPress 4 to transfer\nPress 5 for account statements\nPress 9 to update password");
            Console.WriteLine("\nPress 0 to log out\n" + ANSI_RESET);
            
            InputForMainUi(); // continue
    }

    private static void InputForMainUi()
    {
        int inputForMainUI;
        try
        {
            inputForMainUI = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            MainUi();
            return;
        }
        
        ChooseOfMainUi(inputForMainUI); // continue
    }

    private static void ChooseOfMainUi(int inputForMainUi)
    {
        switch (inputForMainUi)
        {
            case 1:
                InputAmountToWithdraw();
                break;
            case 2:
                Deposit();
                break;
            case 3:
                Balance();
                break;
            case 4:
                TransferMoney();
                break;
            case 5:
                Statements();
                break;
            case 9:
                ChangePassword();
                break;
            case 0:
                Exit();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                MainUi();
                break;
        }
    }

    public static void SemiUi()
    {
        if (semiUILimit >= 6)
        {
            Exit();
        }

        semiUILimit++;
            Console.WriteLine(ANSI_BRIGHT_GREEN +
                              BOLD +
                              "Press 1 return to the main menu\nPress 0 to log out\n" +
                              ANSI_RESET);
            
            InputForSemiUi();
    }

    private static void InputForSemiUi()
    {
        int inputForSemiUi;
        try
        {
            inputForSemiUi = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Console.ReadLine();
            MainUi();
            return;
        }

        ChooseOfSemiUi(inputForSemiUi);
    }

    private static void ChooseOfSemiUi(int inputForSemiUi)
    {
        switch (inputForSemiUi)
        {
            case 1:
                MainUi();
                break;
            case 0:
                Exit();
                break;
            default:
                Console.WriteLine("Invalid option. Please try again.");
                SemiUi();
                break;
        }
    }

    //
    // --- Start OF TRANSFER PROCESS ---
    static protected void TransferMoney()
    {
        if (transferProcessLimit >= 6)
        {
            Exit();
        }

        InputIDToTransfer(); // continue
    }

    static private void InputIDToTransfer()
    {
        transferProcessLimit++;
        if (transferProcessLimit > 5)
        {
            Console.WriteLine("Five chances out!!");
            Exit();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN +
                          BOLD +
                          "Enter the ID number of the person you want to transfer to\n\n" +
                          ANSI_RESET);
        try
        {
            InputtedIdToTrans = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("Invalid input!!");
            InputIDToTransfer();
            return;
        }

        UserBinaryTree.SearchOnTreeForReceiver(InputtedIdToTrans);
        // Method above mentions checkExistsForReceiverAccount() when finish
    }

    public static void CheckExistsForReceiverAccount()
    {
        if ((InputtedIdToTrans == UserBinaryTree.SearchMethodArrayForReceiver[0].NationalID) &&
            (UserBinaryTree.SearchMethodArray[0].NationalID != UserBinaryTree.SearchMethodArrayForReceiver[0].NationalID))
        {
            InputAmountToTransfer(); // Continue if found receiver account
            return;
        }
        else if (InputtedIdToTrans == UserBinaryTree.SearchMethodArray[0].NationalID)
        {
            Console.WriteLine(ANSI_RED + BOLD + "Cannot transfer to yourself" + ANSI_RESET);
            SemiUi();
            return;
        }

        Console.WriteLine(ANSI_RED + BOLD + "No account exists under this ID number" + ANSI_RESET);
        SemiUi();
    }

    static private void InputAmountToTransfer()
    {
        amountToTransferChances++;
        if (amountToTransferChances > 5)
        {
            Console.WriteLine("Five chances out!!");
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN +
                          BOLD +
                          "Enter amount to transfer to the selected account..\n\n" +
                          ANSI_RESET);
        try
        {
            amountToTrans = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Console.ReadLine();
            InputAmountToTransfer();
            return;
        }

        if (amountToTrans <= 10)
        {
            Console.WriteLine(ANSI_RED + BOLD + "You Cannot transfer less than 10$" + ANSI_RESET);
            InputAmountToTransfer();
            return;
        }

        if (amountToTrans > 50000)
        {
            Console.WriteLine(ANSI_RED + BOLD + "You cannot transfer more than 50000$ per time" + ANSI_RESET);
            InputAmountToTransfer();
            return;
        }

        ContinueTransferProcess(); // if passed all conditions continue
    }

    static private void ContinueTransferProcess()
    {
        double mainCustomerBalance = UserBinaryTree.SearchMethodArray[0].Balance;
        if (amountToTrans > mainCustomerBalance)
        {
            Console.WriteLine(ANSI_RED + BOLD + "Your balance is not enough!" + ANSI_RESET);
            Console.WriteLine(ANSI_BRIGHT_WHITE +
                              BOLD +
                              "Your balance is: " +
                              mainCustomerBalance +
                              "$\n" +
                              ANSI_RESET);
            SemiUi();
            return;
        }

        double receiverCustomerBalance = UserBinaryTree.SearchMethodArrayForReceiver[0].Balance;
        // Update the main user balance
        double balanceAfterDeduct = mainCustomerBalance - amountToTrans;
        UserBinaryTree.SearchMethodArray[0].Balance = (balanceAfterDeduct);
        UserBinaryTree.UpdateNewChanges();
        // Update the receiver balance
        double balanceAfterAdded = receiverCustomerBalance + amountToTrans;
        UserBinaryTree.SearchMethodArrayForReceiver[0].Balance = (balanceAfterAdded);
        UserBinaryTree.UpdateNewChangesForReceiver();
        // store operation data
        _bankStatement.AddTransaction(InputNationIdForLogin,amountToTrans,InputtedIdToTrans);

        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "====================" + ANSI_RESET);
        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "\n\nTransferred successfully." + ANSI_RED);

        SemiUi();
    }

    // --- END OF TRANSFER PROCESS ---
    // ---------------------------------

    static private void InputAmountToWithdraw()
    {
        withdrawProcessLimit++;
        if (withdrawProcessLimit > 5)
        {
            Console.WriteLine("Five chances out!!");
            Exit();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter amount to withdraw..\n\n" + ANSI_RESET);
        try
        {
            amountToWithdraw = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Console.ReadLine();
            InputAmountToWithdraw();
            return;
        }

        if (amountToWithdraw > 5000)
        {
            Console.WriteLine(ANSI_RED + BOLD + "You cannot withdraw more than 5000$ per time" + ANSI_RESET);
            InputAmountToWithdraw();
            return;
        }

        if (!((amountToWithdraw % 50 == 0 || amountToWithdraw % 100 == 0) && amountToWithdraw != 0))
        {
            Console.WriteLine(ANSI_RED + BOLD + "Please enter a number multiples of 50 or 100..\n\n" + ANSI_RESET);
            InputAmountToWithdraw();
            return;
        }

        if (amountToWithdraw <= UserBinaryTree.SearchMethodArray[0].Balance)
        {
            Withdraw(); // Continue process
        }
        else
        {
            Console.WriteLine(ANSI_RED + BOLD + "Your balance is not enough!" + ANSI_RESET);
            Console.WriteLine(ANSI_BRIGHT_WHITE +
                              BOLD +
                              "Your balance is: " +
                              UserBinaryTree.SearchMethodArray[0].Balance +
                              "$\n" +
                              ANSI_RESET);
            MainUi();
        }
    }

    static protected void Withdraw()
    {
        // Current balance - withdrawn amount
        balance = UserBinaryTree.SearchMethodArray[0].Balance;
        balance = UserBinaryTree.SearchMethodArray[0].Balance - amountToWithdraw;
        UserBinaryTree.SearchMethodArray[0].Balance = (balance);
        Console.WriteLine(ANSI_BRIGHT_WHITE +
                          BOLD +
                          "Your new balance is: " +
                          UserBinaryTree.SearchMethodArray[0].Balance +
                          "$" +
                          ANSI_RESET);
        // store operation data
        _bankStatement.AddWithdraw(InputNationIdForLogin,amountToWithdraw);
        
        // Save the new account updates
        UserBinaryTree.UpdateNewChanges();
        SemiUi();
    }

    static protected void Deposit()
    {
        InputForDeposit();
        balance = amountToDeposit + UserBinaryTree.SearchMethodArray[0].Balance;
        // Update the new balance
        UserBinaryTree.SearchMethodArray[0].Balance = (balance);
        UserBinaryTree.UpdateNewChanges();

        Console.WriteLine(ANSI_BRIGHT_WHITE +
                          BOLD +
                          "Your new balance is: " +
                          UserBinaryTree.SearchMethodArray[0].Balance +
                          "$\n" +
                          ANSI_RESET);
        
        _bankStatement.AddDeposit(InputNationIdForLogin,amountToDeposit);
        System.Threading.Thread.Sleep(200);
        SemiUi();
    }

    static private void InputForDeposit()
    {
        depositeProcessLimit++;
        if (depositeProcessLimit > 5)
        {
            Console.WriteLine("Five chances out!!");
            Exit();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter amount to deposit..\n\n" + ANSI_RESET);
        try
        {
            amountToDeposit = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Console.ReadLine();
            InputForDeposit();
            return;
        }

        if (amountToDeposit > 150000)
        {
            Console.WriteLine(ANSI_RED + BOLD + "You cannot deposit more than 150000$ per time" + ANSI_RESET);
            InputForDeposit();
            return;
        }

        if (!((amountToDeposit % 50 == 0 || amountToDeposit % 100 == 0) && amountToDeposit != 0))
        {
            Console.WriteLine(ANSI_RED + BOLD + "Please enter a number multiples of 50 or 100..\n\n" + ANSI_RESET);
            InputForDeposit();
        } // If passed until here Continue process
    }

    static protected void Balance()
    {
        Console.WriteLine(ANSI_BRIGHT_WHITE +
                          BOLD +
                          "Your balance is: " +
                          UserBinaryTree.SearchMethodArray[0].Balance +
                          "$\n" +
                          ANSI_RESET);
        System.Threading.Thread.Sleep(200);
        SemiUi();
    }

    static protected void Statements()
    {
        Console.WriteLine(ANSI_BRIGHT_WHITE+BOLD+"================================");
        Console.WriteLine("       Statements section:\n");
        Console.WriteLine(ANSI_BRIGHT_GREEN+"Press 1 for Withdraw statement.");
        Console.WriteLine("Press 2 for Deposit statement.");
        Console.WriteLine("Press 3 for Transactions statement.");
        Console.WriteLine("Press 4 for All statement operations");
        Console.WriteLine(ANSI_BRIGHT_WHITE+"================================\n\n"+ANSI_RESET);

        
        InputStatementType(); // continue
    }

    static private void InputStatementType()
    {
        int type;
        try
        {
            type = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Statements();
            return;
        }
        
        StatementFilterType(type); // continue
    }

    static private void StatementFilterType(int type)
    {
        switch (type)
        {
            case 1:
                _bankStatement.FilterStatementsByWithdraw(InputNationIdForLogin);
                break;
            case 2:
                _bankStatement.FilterStatementsByDeposit(InputNationIdForLogin);
                break;
            case 3:
                _bankStatement.FilterStatementsByTransaction(InputNationIdForLogin);
                break;
            case 4:
                _bankStatement.FilterStatementsByID(InputNationIdForLogin);
                break;
            default:
                Statements();
                break;
        }
    }

    static private void ResetLimits()
    {
        semiUILimit = 0;
        mainUILimit = 0;
        depositeProcessLimit = 0;
        withdrawProcessLimit = 0;
        transferProcessLimit = 0;
        LimitLogin = 0;
        LimitPassword = 0;
        LimitRegisterNationID = 0;
        LimitIDForChangingPassword = 0;
        LimitOldPasswordToChangingPassword = 0;
        LimitLoginProcess = 0;
        LimitNewPasswordForChangingIt = 0;
        LimitRegisterFirstName = 0;
        LimitRegisterSecondName = 0;
        LimitInputForLoginOrRegister = 0;
    }

    static void SlowClearConsole()
    {
        string clearLine = new string(' ',
            Console.WindowWidth);

        for (int i = 0;
             i < Console.WindowHeight;
             i++)
        {
            Console.WriteLine(clearLine);
            Thread.Sleep(20);
        }

        Console.Clear();
    }

    static public void Exit()
    {
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "Thanks for using our SSM." + ANSI_RESET);
        Thread.Sleep(100);

        ResetLimits();
        SlowClearConsole();
        startPoint();
    }
}