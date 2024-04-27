namespace Main;

public class ServiceMachine : User
{
    private static double _balance;

    // Limitations
    public static int SemiUiLimit = 0;
    public static int MainUiLimit = 0;
    public static int DepositeProcessLimit = 0;
    public static int WithdrawProcessLimit = 0;
    public static int TransferProcessLimit = 0;
    public static int IdToTransferLimit = 0;

    private static int _amountToTransferChances = 0;

    // Process helpers
    private static double _amountToTrans;
    private static double _amountToWithdraw;

    private static double _amountToDeposit;


    private static BankStatement _bankStatement = new BankStatement();

    public ServiceMachine()
    {
    }

    public static void Main(string[] args)
    {
        DataHandler dataHandler = new DataHandler(_bankStatement);
        DataHandler.LoadAccountsData(); // For loading the customers accounts data from json
        dataHandler.LoadStatementData();
        StartPoint(); // from here we start
    }

    protected static void StartPoint()
    {
        Console.WriteLine(FontStyle.White("*|=====|*       *|=====|*"));
        Console.WriteLine(FontStyle.Green("Welcome press 0 to start."));
        Console.WriteLine(FontStyle.White("*|=====|*=======*|=====|*"));


        ChooseOfStartPoint(InputForStartPoint());
    }

    private static int InputForStartPoint()
    {
        int startPointInput;
        try
        {
            startPointInput = int.Parse(Console.ReadLine() ?? string.Empty);
        }
        catch (Exception e)
        {
            Console.Clear();
            StartPoint();
            return -1;
        }

        return startPointInput;
    }

    private static void ChooseOfStartPoint(int startPointInput)
    {
        switch (startPointInput)
        {
            case 0:
                Console.Clear();
                LoginOrRegister();
                break;
            default:
                Console.Clear();
                StartPoint();
                break;
        }
    }

    private static void LoginOrRegister()
    {
        AccountManager.LimitInputForLoginOrRegister =
            AttemptsHandler.IncreaseAttempts(AccountManager.LimitInputForLoginOrRegister);
        if (AttemptsHandler.IsExceededAttempts(AccountManager.LimitInputForLoginOrRegister, 3))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.White($"\n*==| {WelcomeSystemMessage()} |==*"));
        Console.WriteLine(FontStyle.Green("\n" + LoginOrRegisterMessage()));
        Console.Write(FontStyle.White("\nYour Choice: "));
        ChooseForLoginOrRegister(GetInputForLoginOrRegister());
    }

    private static string WelcomeSystemMessage()
    {
        return "Welcome to our SSM System.";
    }

    private static string LoginOrRegisterMessage()
    {
        return "1. Log in to your account." + "\n2. Register for a new account.";
    }

    public static int GetInputForLoginOrRegister()
    {
        int choice;
        try
        {
            choice = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.Clear();
            LoginOrRegister();
            return -1;
        }

        return choice;
    }

    private static void ChooseForLoginOrRegister(int choice)
    {
        switch (choice)
        {
            case 1:
                AccountManager.LimitInputForLoginOrRegister =
                    AttemptsHandler.ResetAttempts(AccountManager.LimitInputForLoginOrRegister);
                UserAuth.Login();
                break;
            case 2:
                AccountManager.LimitInputForLoginOrRegister =
                    AttemptsHandler.ResetAttempts(AccountManager.LimitInputForLoginOrRegister);
                UserAuth.Register();
                break;
            default:
                Console.Clear();
                LoginOrRegister();
                break;
        }
    }

    public static void MainUi()
    {
        MainUiLimit = AttemptsHandler.IncreaseAttempts(MainUiLimit);
        if (AttemptsHandler.IsExceededAttempts(MainUiLimit, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }


        Console.WriteLine(FontStyle.White(MainUiWelcomeMessage()));
        Console.WriteLine(FontStyle.White("Welcome Mr." + TreeManager.SearchMethodArray[0].FirstName + " ."));
        Console.WriteLine(FontStyle.Green(MainUiChoicesMessage()));
        Console.Write(FontStyle.White("Your Choice: "));

        ChooseOfMainUi(InputForMainUi()); // continue
    }

    private static string MainUiWelcomeMessage()
    {
        return "\n===*| Services Section |*===\n";
    }

    private static string MainUiChoicesMessage()
    {
        return "1. Withdraw\n2. Deposit\n3. Balance" + "\n4. Transfer\n5. Statements" +
               "\n9. Update Password\n0. Log out\n";
    }

    private static int InputForMainUi()
    {
        int inputForMainUi;
        try
        {
            inputForMainUi = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            MainUi();
            return -1;
        }

        return inputForMainUi;
    }

    private static void ChooseOfMainUi(int inputForMainUi)
    {
        switch (inputForMainUi)
        {
            case 1:
                MainUiLimit = AttemptsHandler.ResetAttempts(MainUiLimit);
                Withdraw();
                break;
            case 2:
                MainUiLimit = AttemptsHandler.ResetAttempts(MainUiLimit);
                Deposit();
                break;
            case 3:
                MainUiLimit = AttemptsHandler.ResetAttempts(MainUiLimit);
                Balance();
                break;
            case 4:
                MainUiLimit = AttemptsHandler.ResetAttempts(MainUiLimit);
                TransferMoney();
                break;
            case 5:
                MainUiLimit = AttemptsHandler.ResetAttempts(MainUiLimit);
                Statements();
                break;
            case 9:
                MainUiLimit = AttemptsHandler.ResetAttempts(MainUiLimit);
                AccountManager.ChangePassword();
                break;
            case 0:
                Exit();
                break;
            default:
                Console.WriteLine(FontStyle.Red("Invalid option. Please try again."));
                MainUi();
                break;
        }
    }

    public static void SemiUi()
    {
        SemiUiLimit = AttemptsHandler.IncreaseAttempts(SemiUiLimit);
        if (AttemptsHandler.IsExceededAttempts(SemiUiLimit, 6))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.Green("\n1. Return Main Menu\n0. Log out"));
        Console.Write(FontStyle.White("\nYour Choice: "));

        ChooseOfSemiUi(InputForSemiUi());
    }

    private static int InputForSemiUi()
    {
        int inputForSemiUi;
        try
        {
            inputForSemiUi = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            MainUi();
            return -1;
        }

        return inputForSemiUi;
    }

    private static void ChooseOfSemiUi(int inputForSemiUi)
    {
        switch (inputForSemiUi)
        {
            case 1:
                SemiUiLimit = AttemptsHandler.ResetAttempts(SemiUiLimit);
                MainUi();
                break;
            case 0:
                Exit();
                break;
            default:
                Console.WriteLine(FontStyle.Red("Invalid option. Please try again."));
                SemiUi();
                break;
        }
    }


    // --- Start OF TRANSFER PROCESS ---
    protected static void TransferMoney()
    {
        TransferProcessLimit = AttemptsHandler.IncreaseAttempts(TransferProcessLimit);
        if (AttemptsHandler.IsExceededAttempts(TransferProcessLimit, 6))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        int receiverId = GetIdToTransfer();
        TreeManager.SearchOnTreeForReceiver(receiverId);

        if (!IsReceiverAccountExists(receiverId))
        {
            SemiUi();
            return;
        }

        double amountToTransfer = InputAmountToTransfer();
        if (!IsValidTransferAmount(amountToTransfer))
        {
            SemiUi();
            return;
        }

        CompleteTransferProcess(amountToTransfer, receiverId); // if passed all conditions continue
    }

    private static int GetIdToTransfer()
    {
        IdToTransferLimit = AttemptsHandler.IncreaseAttempts(IdToTransferLimit);
        if (AttemptsHandler.IsExceededAttempts(IdToTransferLimit, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return -1;
        }

        Console.Write(FontStyle.Green("Enter the ID number of the person you want to transfer to: "));

        int receiverId;
        try
        {
            receiverId = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            GetIdToTransfer();
            return -1;
        }

        return receiverId;
    }

    public static bool IsReceiverAccountExists(int receiverId)
    {
        if (receiverId == TreeManager.SearchMethodArray[0].NationalId)
        {
            Console.WriteLine(FontStyle.Red("You cannot transfer to yourself!"));
            return false;
        }

        if (receiverId != TreeManager.SearchMethodArrayForReceiver[0].NationalId)
        {
            Console.WriteLine(FontStyle.Red("No account exists under this ID number"));
            return false;
        }

        if (receiverId == TreeManager.SearchMethodArrayForReceiver[0].NationalId)
        {
            return true;
        }

        return false;
    }

    private static double InputAmountToTransfer()
    {
        _amountToTransferChances = AttemptsHandler.IncreaseAttempts(_amountToTransferChances);
        if (AttemptsHandler.IsExceededAttempts(_amountToTransferChances, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return -1;
        }

        Console.Write(FontStyle.Green("Enter amount to transfer to the selected account: "));
        try
        {
            _amountToTrans = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            InputAmountToTransfer();
            return -1;
        }

        return _amountToTrans;
        //ContinueTransferProcess(); // if passed all conditions continue
    }

    private static bool IsValidTransferAmount(double amount)
    {
        if (!InputsFilter.IsItBiggerThan49(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.LessThan50Message()));
            InputAmountToTransfer();
            return false;
        }

        if (!InputsFilter.IsItLessThan10001(amount))
        {
            InputsFilter.BiggerThan10000Message();
            InputAmountToTransfer();
            return false;
        }

        return true;
    }

    private static double GetSenderBalance()
    {
        return TreeManager.SearchMethodArray[0].Balance;
    }

    private static double GetReceiverBalance()
    {
        return TreeManager.SearchMethodArrayForReceiver[0].Balance;
    }

    private static bool IsSenderBalanceEnought(double amountToTransfer)
    {
        if (amountToTransfer > GetSenderBalance())
        {
            Console.WriteLine(FontStyle.Red(BalanceNotEnoughtMessage()));
            Console.WriteLine(FontStyle.White(DisplayBalance()));
            SemiUi();
            return false;
        }

        return true;
    }

    private static void UpdateSenderBalance(double senderBalanceAfterDeduct)
    {
        TreeManager.SearchMethodArray[0].Balance = senderBalanceAfterDeduct;
        TreeManager.UpdateNewChanges();
    }

    private static void UpdateReceiverBalance(double receiverBalanceAfterReceived)
    {
        TreeManager.SearchMethodArrayForReceiver[0].Balance = receiverBalanceAfterReceived;
        TreeManager.UpdateNewChangesForReceiver();
    }


    private static void CompleteTransferProcess(double amountToTransfer, int receiverId)
    {
        // Update the main user balance
        double receiverBalanceAfterReceived = GetSenderBalance() - amountToTransfer;
        UpdateSenderBalance(receiverBalanceAfterReceived);

        // Update the receiver balance
        double balanceAfterAdded = GetReceiverBalance() + amountToTransfer;
        UpdateReceiverBalance(balanceAfterAdded);

        // store operation data
        _bankStatement.AddTransaction(UserAuth.UserAccountId, amountToTransfer, receiverId);


        Console.Clear();
        Console.Write(FontStyle.Red("==> "));
        Console.Write(FontStyle.Green("Transferred successfully."));
        Console.Write(FontStyle.Red(" <==\n"));
        _amountToTransferChances = AttemptsHandler.ResetAttempts(_amountToTransferChances);
        SemiUi();
    } // --- END OF TRANSFER PROCESS ---

    private static void Withdraw()
    {
        WithdrawProcessLimit = AttemptsHandler.IncreaseAttempts(WithdrawProcessLimit);
        if (AttemptsHandler.IsExceededAttempts(WithdrawProcessLimit, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        double amount = GetAmountToWithdraw();
        if (!IsValidWithdraw(amount))
        {
            Withdraw();
        }

        CompleteWithdraw(amount); // Continue process
    }

    private static double GetAmountToWithdraw()
    {
        Console.Write(FontStyle.Green("Enter amount to withdraw: "));
        Double amount;
        try
        {
            amount = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInputMessage()));
            Withdraw();
            return -1;
        }

        return amount;
    }

    private static bool IsValidWithdraw(double amount)
    {
        if (!InputsFilter.IsItBiggerThan49(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.LessThan50Message()));
            Withdraw();
            return false;
        }

        if (!InputsFilter.IsItLessThan5001(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.BiggerThan5000Message()));
            Withdraw();
            return false;
        }

        if (!InputsFilter.IsItMultipleOf50Or100(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.NotMultipleOf50Or100Message()));
            Withdraw();
            return false;
        }

        if (!(amount <= TreeManager.SearchMethodArray[0].Balance))
        {
            Console.WriteLine(FontStyle.Red(BalanceNotEnoughtMessage()));
            Console.WriteLine(FontStyle.White(DisplayBalance()));
            MainUi();
        }

        return true;
    }

    protected static void CompleteWithdraw(double amount)
    {
        // Current balance - withdrawn amount
        _balance = AccountBalance() - amount;
        UpdateBalance(_balance);
        Console.WriteLine(FontStyle.White(DisplayNewBalance()));

        // store operation data
        _bankStatement.AddWithdraw(UserAuth.UserAccountId, amount);

        // Save the new account updates
        TreeManager.UpdateNewChanges();
        WithdrawProcessLimit = AttemptsHandler.ResetAttempts(WithdrawProcessLimit);
        SemiUi();
    }

    private static double AccountBalance()
    {
        return (TreeManager.SearchMethodArray[0].Balance);
    }

    private static void UpdateBalance(double newBalance)
    {
        TreeManager.SearchMethodArray[0].Balance = newBalance;
    }

    private static string DisplayBalance()
    {
        return "Your balance is: " + AccountBalance() + "$";
    }

    private static string DisplayNewBalance()
    {
        return "Your new balance is: " + AccountBalance() + "$";
    }

    private static string BalanceNotEnoughtMessage()
    {
        return "Your balance is not enough!";
    }

    protected static void Deposit()
    {
        InputForDeposit();
        DepositeProcessLimit = AttemptsHandler.ResetAttempts(DepositeProcessLimit);
        _balance = _amountToDeposit + TreeManager.SearchMethodArray[0].Balance;
        // Update the new balance
        UpdateBalance(_balance);
        TreeManager.UpdateNewChanges();

        Console.WriteLine(FontStyle.White(DisplayNewBalance()));

        _bankStatement.AddDeposit(UserAuth.UserAccountId, _amountToDeposit);
        System.Threading.Thread.Sleep(200);
        SemiUi();
    }

    private static void InputForDeposit()
    {
        DepositeProcessLimit = AttemptsHandler.IncreaseAttempts(DepositeProcessLimit);
        if (AttemptsHandler.IsExceededAttempts(DepositeProcessLimit, 3))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.Write(FontStyle.Green("Enter amount to deposit: "));

        try
        {
            _amountToDeposit = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            InputForDeposit();
            return;
        }

        if (_amountToDeposit > 150000)
        {
            Console.WriteLine(FontStyle.Red("You cannot deposit more than 150000$ per time"));
            InputForDeposit();
            return;
        }

        if (!((_amountToDeposit % 50 == 0 || _amountToDeposit % 100 == 0) && _amountToDeposit != 0))
        {
            Console.Write(FontStyle.Red("Please enter a number multiples of 50 or 100: "));
            InputForDeposit();
        } // If passed until here Continue process
    }

    private static void Balance()
    {
        Console.WriteLine(FontStyle.White("Your balance is: " + TreeManager.SearchMethodArray[0].Balance + "$"));
        System.Threading.Thread.Sleep(200);
        SemiUi();
    }

    private static void Statements()
    {
        Console.WriteLine(FontStyle.White("====* Statements section *====\n"));
        Console.WriteLine(FontStyle.Green("1. Withdraw Statements."));
        Console.WriteLine(FontStyle.Green("2. Deposit Statements."));
        Console.WriteLine(FontStyle.Green("3. Transactions Statements."));
        Console.WriteLine(FontStyle.Green("4. All Statements Operations"));
        Console.Write(FontStyle.White("\nYour Choice: "));


        InputStatementType(); // continue
    }

    private static void InputStatementType()
    {
        int type;
        try
        {
            type = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Statements();
            return;
        }

        StatementFilterType(type); // continue
    }

    private static void StatementFilterType(int type)
    {
        switch (type)
        {
            case 1:

                _bankStatement.FilterStatementsByWithdraw(UserAuth.UserAccountId);
                break;
            case 2:

                _bankStatement.FilterStatementsByDeposit(UserAuth.UserAccountId);
                break;
            case 3:

                _bankStatement.FilterStatementsByTransaction(UserAuth.UserAccountId);
                break;
            case 4:

                _bankStatement.FilterStatementsById(UserAuth.UserAccountId);
                break;
            default:
                Console.Clear();
                Statements();
                break;
        }
    }


    private static void SlowClearConsole(int speed)
    {
        string clearLine = new string(' ', Console.WindowWidth);

        for (int i = 0; i < Console.WindowHeight; i++)
        {
            Console.WriteLine(clearLine);
            Thread.Sleep(speed);
        }

        Console.Clear();
    }

    public static void Exit()
    {
        Console.Clear();
        Thread.Sleep(100);
        UserAuth.ResetOldData();
        AttemptsHandler.ResetLimits();
        SlowClearConsole(20);
        Console.WriteLine(FontStyle.White("Thanks for using our SSM."));
        StartPoint();
    }
}