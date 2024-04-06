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


    public SelfServiceMachine()
    {
    }

    public static void Main(string[] args)
    {
        LoadAccountsData(); // For loading the customers accounts data from json
        startPoint(); // from here we start
    }

    protected static void startPoint()
    {
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "========================");
        Console.WriteLine(ANSI_BRIGHT_GREEN + "Welcome press 0 to start.\n\n" + ANSI_RESET);
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

    public static void MainUI()
    {
        if (mainUILimit < 6)
        {
            mainUILimit++;
            Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "==================================" + ANSI_RESET);
            Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "We are providing these services only for you:\n\n");
            Console.WriteLine(ANSI_BRIGHT_GREEN + "Press 1 for withdraw\nPress 2 for deposit");
            Console.WriteLine("Press 3 for balance\nPress 4 to transfer\nPress 9 to update password");
            Console.WriteLine("\nPress 0 to log out\n" + ANSI_RESET);
            int yoInput;
            try
            {
                yoInput = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
                MainUI();
                return;
            }

            switch (yoInput)
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
                case 9:
                    ChangePassword();
                    break;
                case 0:
                    Exit();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    MainUI();
                    break;
            }
        }
        else
        {
            Exit();
        }
    }

    public static void SemiUI()
    {
        if (semiUILimit < 6)
        {
            semiUILimit++;
            Console.WriteLine(ANSI_BRIGHT_GREEN +
                              BOLD +
                              "Press 1 return to the main menu\nPress 0 to log out\n" +
                              ANSI_RESET);
            int yoInput;
            try
            {
                yoInput = Convert.ToInt32(Console.ReadLine());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                Console.ReadLine();
                MainUI();
                return;
            }

            switch (yoInput)
            {
                case 1:
                    MainUI();
                    break;
                case 0:
                    Exit();
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    SemiUI();
                    break;
            }
        }
        else
        {
            Exit();
        }
    }

    //
    // --- Start OF TRANSFER PROCESS ---
    static protected void TransferMoney()
    {
        if (transferProcessLimit < 6)
        {
            InputIDToTransfer();
        }
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

        UserAvlTree.SearchOnTreeForReceiver(InputtedIdToTrans);
        // Method above mentions checkExistsForReceiverAccount() when finish
    }

    public static void CheckExistsForReceiverAccount()
    {
        if ((InputtedIdToTrans == UserAvlTree.SearchMethodArrayForReceiver[0].NationalID) &&
            (UserAvlTree.SearchMethodArray[0].NationalID != UserAvlTree.SearchMethodArrayForReceiver[0].NationalID))
        {
            InputAmountToTransfer(); // Continue if found receiver account
            return;
        }
        else if (InputtedIdToTrans == UserAvlTree.SearchMethodArray[0].NationalID)
        {
            Console.WriteLine(ANSI_RED + BOLD + "Cannot transfer to yourself" + ANSI_RESET);
            SemiUI();
            return;
        }

        Console.WriteLine(ANSI_RED + BOLD + "No account exists under this ID number" + ANSI_RESET);
        SemiUI();
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
        double mainCustomerBalance = UserAvlTree.SearchMethodArray[0].Balance;
        if (amountToTrans > mainCustomerBalance)
        {
            Console.WriteLine(ANSI_RED + BOLD + "Your balance is not enough!" + ANSI_RESET);
            Console.WriteLine(ANSI_BRIGHT_WHITE +
                              BOLD +
                              "Your balance is: " +
                              mainCustomerBalance +
                              "$\n" +
                              ANSI_RESET);
            SemiUI();
            return;
        }

        double receiverCustomerBalance = UserAvlTree.SearchMethodArrayForReceiver[0].Balance;
        // Update the main user balance
        double balanceAfterDeduct = mainCustomerBalance - amountToTrans;
        UserAvlTree.SearchMethodArray[0].Balance = (balanceAfterDeduct);
        UserAvlTree.UpdateNewChanges();
        // Update the receiver balance
        double balanceAfterAdded = receiverCustomerBalance + amountToTrans;
        UserAvlTree.SearchMethodArrayForReceiver[0].Balance = (balanceAfterAdded);
        UserAvlTree.UpdateNewChangesForReceiver();

        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "====================" + ANSI_RESET);
        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "\n\nTransferred successfully." + ANSI_RED);

        SemiUI();
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

        if (amountToWithdraw <= UserAvlTree.SearchMethodArray[0].Balance)
        {
            Withdraw(); // Continue process
        }
        else
        {
            Console.WriteLine(ANSI_RED + BOLD + "Your balance is not enough!" + ANSI_RESET);
            Console.WriteLine(ANSI_BRIGHT_WHITE +
                              BOLD +
                              "Your balance is: " +
                              UserAvlTree.SearchMethodArray[0].Balance +
                              "$\n" +
                              ANSI_RESET);
            MainUI();
        }
    }

    static protected void Withdraw()
    {
        // Current balance - withdrawn amount
        balance = UserAvlTree.SearchMethodArray[0].Balance;
        balance = UserAvlTree.SearchMethodArray[0].Balance - amountToWithdraw;
        UserAvlTree.SearchMethodArray[0].Balance = (balance);
        Console.WriteLine(ANSI_BRIGHT_WHITE +
                          BOLD +
                          "Your new balance is: " +
                          UserAvlTree.SearchMethodArray[0].Balance +
                          "$" +
                          ANSI_RESET);
        // Save the new account updates
        UserAvlTree.UpdateNewChanges();
        SemiUI();
    }

    static protected void Deposit()
    {
        InputForDeposit();
        balance = amountToDeposit + UserAvlTree.SearchMethodArray[0].Balance;
        // Update the new balance
        UserAvlTree.SearchMethodArray[0].Balance = (balance);
        UserAvlTree.UpdateNewChanges();

        Console.WriteLine(ANSI_BRIGHT_WHITE +
                          BOLD +
                          "Your new balance is: " +
                          UserAvlTree.SearchMethodArray[0].Balance +
                          "$\n" +
                          ANSI_RESET);
        System.Threading.Thread.Sleep(200);
        SemiUI();
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
                          UserAvlTree.SearchMethodArray[0].Balance +
                          "$\n" +
                          ANSI_RESET);
        System.Threading.Thread.Sleep(200);
        SemiUI();
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