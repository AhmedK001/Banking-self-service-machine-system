namespace Main;

public class ServiceMachine : User
{
    private static double _balance;

    // Limitations
    public static int LimitSemiUi = 6;
    public static int LimitMainUi = 6;
    public static int LimitDepositeProcess = 6;
    public static int LimitWithdrawProcess = 6;
    public static int LimitTransferProcess = 6;
    public static int LimitStatementProcess = 6;
    public static int LimitIdToTransfer = 6;

    public static int LimitValueToTrans = 6;

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

    public static void LoginOrRegister()
    {
        if (!AttemptsHandler.LetLoginOrRegister()) return;

        Console.WriteLine(FontStyle.White($"\n*==| {WelcomeSystemMessage()} |==*"));
        Console.WriteLine(FontStyle.Green("\n" + LoginOrRegisterMessage()));
        Console.Write(FontStyle.White("\nYour Choice: "));
        ChooseForLoginOrRegister(GetInputForLoginOrRegister());
    }

    private static string WelcomeSystemMessage()
    {
        return "Welcome to our SSM System.";
    }

    public static string LoginOrRegisterMessage()
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
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(AccountManager.LimitInputForLoginOrRegister)));
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
                Console.WriteLine(
                    FontStyle.Red(InputsFilter.InvalidOption(AccountManager.LimitInputForLoginOrRegister)));
                LoginOrRegister();
                break;
        }
    }

    public static void MainUi()
    {
        if (!AttemptsHandler.LetMainUi()) return;

        Console.WriteLine(FontStyle.White(MainUiWelcomeMessage()));
        Console.WriteLine(FontStyle.White("Welcome Mr." + TreeManager.SearchMethodArray[0].FirstName + " .\n"));
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
            Console.Clear();
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(LimitMainUi)));
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
                Console.Clear();
                LimitMainUi = AttemptsHandler.ResetAttempts(LimitMainUi);
                Withdraw();
                break;
            case 2:
                Console.Clear();
                LimitMainUi = AttemptsHandler.ResetAttempts(LimitMainUi);
                Deposit();
                break;
            case 3:
                Console.Clear();
                LimitMainUi = AttemptsHandler.ResetAttempts(LimitMainUi);
                Balance();
                break;
            case 4:
                Console.Clear();
                LimitMainUi = AttemptsHandler.ResetAttempts(LimitMainUi);
                TransferMoney();
                break;
            case 5:
                Console.Clear();
                LimitMainUi = AttemptsHandler.ResetAttempts(LimitMainUi);
                Statements();
                break;
            case 9:
                Console.Clear();
                LimitMainUi = AttemptsHandler.ResetAttempts(LimitMainUi);
                AccountManager.ChangePassword();
                break;
            case 0:
                Console.Clear();
                Exit();
                break;
            default:
                Console.Clear();
                Console.WriteLine(FontStyle.Red(InputsFilter.InvalidOption(LimitMainUi)));
                MainUi();
                break;
        }
    }

    public static void SemiUi()
    {
        if(!AttemptsHandler.LetSemiUi()) return;

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
            Console.Clear();
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
                Console.Clear();
                LimitSemiUi = AttemptsHandler.ResetAttempts(LimitSemiUi);
                MainUi();
                break;
            case 0:
                Console.Clear();
                Exit();
                break;
            default:
                Console.Clear();
                Console.WriteLine(FontStyle.Red(InputsFilter.InvalidOption(LimitSemiUi)));
                SemiUi();
                break;
        }
    }


    // --- Start OF TRANSFER PROCESS ---
    protected static void TransferMoney()
    {
        if(!AttemptsHandler.LetTransferMoney()) return;

        int receiverId = GetIdToTransfer();

        if (!InputsFilter.IsItNationalId(receiverId))
        {
            Console.WriteLine(FontStyle.Red("Not Found."));
            SemiUi();
            return;
        }

        TreeManager.SearchOnTreeForReceiver(receiverId); // search 

        if (!IsReceiverAccountExists(receiverId))
        {
            SemiUi();
            return;
        }

        double amountToTransfer = GetAmountToTransfer();
        if (!IsValidTransferAmount(amountToTransfer))
        {
            SemiUi();
            return;
        }

        CompleteTransferProcess(amountToTransfer, receiverId); // if passed all conditions continue
    }

    private static int GetIdToTransfer()
    {
        if (!AttemptsHandler.LetGetIdToTransfer()) return -1;

        Console.Write(FontStyle.Green("Enter the ID number of the person you want to transfer to: "));

        int receiverId;
        try
        {
            receiverId = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            GetIdToTransfer();
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(LimitIdToTransfer)));
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

    private static double GetAmountToTransfer()
    {
        if (!AttemptsHandler.LetGetAmountToTransfer()) return -1;

        Console.Write(FontStyle.Green("Enter amount to transfer to the selected account: "));
        try
        {
            _amountToTrans = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(LimitValueToTrans)));
            GetAmountToTransfer();
            return -1;
        }

        return _amountToTrans;
        //ContinueTransferProcess(); // if passed all conditions continue
    }

    private static bool IsValidTransferAmount(double amount)
    {
        if (!InputsFilter.IsBiggerThan49(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.LessThan50()));
            GetAmountToTransfer();
            return false;
        }

        if (!InputsFilter.IsLessThan10001(amount))
        {
            InputsFilter.BiggerThan10000();
            GetAmountToTransfer();
            return false;
        }

        if (!IsSenderBalanceEnought(amount))
        {
            Console.WriteLine(FontStyle.Red(BalanceNotEnoughtMessage()));
            GetAmountToTransfer();
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
        LimitValueToTrans = AttemptsHandler.ResetAttempts(LimitValueToTrans);
        SemiUi();
    } // --- END OF TRANSFER PROCESS ---

    private static void Withdraw()
    {
        if (!AttemptsHandler.LetWithdraw()) return;

        double amount = GetAmountToWithdraw();
        if (!IsValidWithdraw(amount))
        {
            Withdraw();
            return;
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
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(LimitWithdrawProcess)));
            Withdraw();
            return -1;
        }

        return amount;
    }

    private static bool IsValidWithdraw(double amount)
    {
        if (!InputsFilter.IsBiggerThan49(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.LessThan50()));
            Withdraw();
            return false;
        }

        if (!InputsFilter.IsLessThan5001(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.BiggerThan5000()));
            Withdraw();
            return false;
        }

        if (!InputsFilter.IsMultipleOf50Or100(amount))
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.NotMultipleOf50Or100()));
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
        UpdateUserBalance(_balance = AccountBalance() - amount);
        Console.WriteLine(FontStyle.White(DisplayNewBalance()));

        // store operation data
        _bankStatement.AddWithdraw(UserAuth.UserAccountId, amount);

        LimitWithdrawProcess = AttemptsHandler.ResetAttempts(LimitWithdrawProcess);
        SemiUi();
    }

    private static double AccountBalance()
    {
        return TreeManager.SearchMethodArray[0].Balance;
    }

    private static void UpdateUserBalance(double newBalance)
    {
        TreeManager.SearchMethodArray[0].Balance = newBalance;
        TreeManager.UpdateNewChanges();
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
        LimitDepositeProcess = AttemptsHandler.IncreaseAttempts(LimitDepositeProcess);
        if (AttemptsHandler.IsExceededAttempts(LimitDepositeProcess))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        if (!IsValidDeposit(InputForDeposit()))
        {
            Deposit();
            return;
        }

        LimitDepositeProcess = AttemptsHandler.ResetAttempts(LimitDepositeProcess);
        _balance = _amountToDeposit + AccountBalance();

        // Update balance
        UpdateUserBalance(_balance);
        Console.WriteLine(FontStyle.White(DisplayNewBalance()));

        // Add to statements
        _bankStatement.AddDeposit(UserAuth.UserAccountId, _amountToDeposit);
        System.Threading.Thread.Sleep(200);
        SemiUi();
    }

    private static double InputForDeposit()
    {
        Console.Write(FontStyle.Green("Enter amount to deposit: "));

        try
        {
            _amountToDeposit = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(LimitDepositeProcess)));
            Deposit();
            return -1;
        }

        return _amountToDeposit;
    }

    private static bool IsValidDeposit(double amount)
    {
        if (amount > 150000)
        {
            Console.WriteLine(FontStyle.Red("You cannot deposit more than 150000$ per time"));
            return false;
        }

        if (!InputsFilter.IsMultipleOf50Or100(amount))
        {
            Console.WriteLine(FontStyle.Red("Please enter a number multiples of 50 or 100: "));
            return false;
        }

        return true;
    }

    private static void Balance()
    {
        Console.WriteLine(FontStyle.White(DisplayBalance()));
        SemiUi();
    }

    private static void Statements()
    {
        if(!AttemptsHandler.LetStatements()) return;

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
            Console.Clear();
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(LimitStatementProcess)));
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
                Console.Clear();
                AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterByWithdraw(UserAuth.UserAccountId);
                break;
            case 2:
                Console.Clear();
                AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterByDeposit(UserAuth.UserAccountId);
                break;
            case 3:
                Console.Clear();
                AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterByTransaction(UserAuth.UserAccountId);
                break;
            case 4:
                Console.Clear();
                AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterById(UserAuth.UserAccountId);
                break;
            default:
                Console.Clear();
                Console.WriteLine(FontStyle.Red(InputsFilter.InvalidOption(LimitStatementProcess)));
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
        AttemptsHandler.ResetLimitations();

        SlowClearConsole(20);
        Console.WriteLine(FontStyle.White("Thanks for using our SSM."));
        StartPoint();
    }
}