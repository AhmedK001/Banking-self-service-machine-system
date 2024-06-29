using System.Text;

namespace Main;

public class ServiceMachine : User
{
    private static double _balance;

    // Limitations
    public static int LimitSemiUi = AttemptsHandler.ANSI_CHANCES;
    public static int LimitMainUi = AttemptsHandler.ANSI_CHANCES;
    public static int LimitDepositeProcess = AttemptsHandler.ANSI_CHANCES;
    public static int LimitWithdrawProcess = AttemptsHandler.ANSI_CHANCES;
    public static int LimitTransferProcess = AttemptsHandler.ANSI_CHANCES;
    public static int LimitStatementProcess = AttemptsHandler.ANSI_CHANCES;
    public static int LimitIdToTransfer = AttemptsHandler.ANSI_CHANCES;

    public static int LimitValueToTrans = AttemptsHandler.ANSI_CHANCES;

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
        Writer.WriteLine("*|=====|*       *|=====|*", "white");
        Writer.WriteLine("Welcome press 0 to start.", "green");
        Writer.WriteLine("*|=====|*=======*|=====|*", "white");

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

        Writer.WriteLine($"\n*==| {WelcomeSystemMessage()} |==*", "white");
        Writer.WriteLine("\n" + LoginOrRegisterMessage(), "green");
        Writer.WriteLine("\nYour Choice: ", "white");

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
            Writer.WriteLine(Messenger.InvalidInput(PasswordModifyer.LimitInputForLoginOrRegister),"red");
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
                PasswordModifyer.LimitInputForLoginOrRegister =
                    AttemptsHandler.ResetAttempts(PasswordModifyer.LimitInputForLoginOrRegister);
                UserAuth.Login();
                break;
            case 2:
                PasswordModifyer.LimitInputForLoginOrRegister =
                    AttemptsHandler.ResetAttempts(PasswordModifyer.LimitInputForLoginOrRegister);
                UserAuth.Register();
                break;
            default:
                Console.Clear();
                Writer.WriteLine(Messenger.InvalidOption(PasswordModifyer.LimitInputForLoginOrRegister),"red");
                LoginOrRegister();
                break;
        }
    }

    public static void MainUi()
    {
        if (!AttemptsHandler.LetMainUi()) return;

        Writer.WriteLine(MainUiWelcomeMessage(), "white");
        Writer.WriteLine("Welcome Mr." + TreeManager.SearchMethodArray[0].FirstName + " .\n", "green");
        Writer.WriteLine(MainUiChoicesMessage(), "white");
        Writer.WriteLine("Your Choice: ", "white");

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
            Writer.WriteLine(Messenger.InvalidInput(LimitMainUi),"red");
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
                PasswordModifyer.ChangePassword();
                break;
            case 0:
                Console.Clear();
                Exit();
                break;
            default:
                Console.Clear();
                Writer.WriteLine(Messenger.InvalidOption(LimitMainUi),"red");
                MainUi();
                break;
        }
    }

    public static void SemiUi()
    {
        if (!AttemptsHandler.LetSemiUi()) return;

        Writer.WriteLine("\n1. Return Main Menu\n0. Log out", "white");
        Writer.Write("\nYour Choice: ", "green");
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
                Writer.WriteLine(Messenger.InvalidOption(LimitSemiUi),"red");
                SemiUi();
                break;
        }
    }


    // --- Start OF TRANSFER PROCESS ---
    protected static void TransferMoney()
    {
        if (!AttemptsHandler.LetTransferMoney()) return;

        int receiverId = GetIdToTransfer();

        if (!Validator.IsNationalId(receiverId))
        {
            Writer.WriteLine("Not Found.", "red");
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
        Writer.Write("Enter the ID number of the person you want to transfer to: ", "green");

        int receiverId;
        try
        {
            receiverId = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            GetIdToTransfer();
            Writer.WriteLine(Messenger.InvalidInput(LimitIdToTransfer),"red");
            return -1;
        }

        return receiverId;
    }

    public static bool IsReceiverAccountExists(int receiverId)
    {
        if (receiverId == TreeManager.SearchMethodArray[0].NationalId)
        {
            Writer.WriteLine("You cannot transfer to yourself!", "red");
            return false;
        }

        if (receiverId != TreeManager.SearchMethodArrayForReceiver[0].NationalId)
        {
            Writer.WriteLine("No account exists under this ID number", "red");
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

        Writer.Write("Enter amount to transfer to the selected account: ", "green");
        try
        {
            _amountToTrans = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            Writer.WriteLine(Messenger.InvalidInput(LimitValueToTrans),"red");
            GetAmountToTransfer();
            return -1;
        }

        return _amountToTrans;
        //ContinueTransferProcess(); // if passed all conditions continue
    }

    private static bool IsValidTransferAmount(double amount)
    {
        if (!Validator.IsBiggerThan49(amount))
        {
            Writer.WriteLine(Messenger.LessThan50(),"red");
            GetAmountToTransfer();
            return false;
        }

        if (!Validator.IsLessThan10001(amount))
        {
            Messenger.BiggerThan10000();
            GetAmountToTransfer();
            return false;
        }

        if (!IsSenderBalanceEnought(amount))
        {
            Writer.WriteLine(BalanceNotEnoughtMessage(),"red");
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
            Writer.WriteLine(BalanceNotEnoughtMessage(),"red");
            Writer.WriteLine(DisplayBalance(),"white");
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


        //Console.Clear();
        Writer.Write("==> ", "red");
        Writer.Write("Transferred successfully.", "green");
        Writer.Write(" <==\n", "red");
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
        Writer.Write("Enter amount to withdraw: ", "green");
        Double amount;
        try
        {
            amount = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            Writer.WriteLine(Messenger.InvalidInput(LimitWithdrawProcess),"red");
            Withdraw();
            return -1;
        }

        return amount;
    }

    private static bool IsValidWithdraw(double amount)
    {
        if (!Validator.IsBiggerThan49(amount))
        {
            Writer.WriteLine(Messenger.LessThan50(),"red");
            Withdraw();
            return false;
        }

        if (!Validator.IsLessThan5001(amount))
        {
            Writer.WriteLine(Messenger.BiggerThan5000(),"red");
            Withdraw();
            return false;
        }

        if (!Validator.IsMultipleOf50Or100(amount))
        {
            Writer.WriteLine(Messenger.NotMultipleOf50Or100(),"red");
            Withdraw();
            return false;
        }

        if (!(amount <= TreeManager.SearchMethodArray[0].Balance))
        {
            Writer.WriteLine(BalanceNotEnoughtMessage(),"red");
            Writer.WriteLine(DisplayBalance(),"white");
            MainUi();
        }

        return true;
    }

    protected static void CompleteWithdraw(double amount)
    {
        UpdateUserBalance(_balance = AccountBalance() - amount);
        Writer.WriteLine(DisplayNewBalance(),"white");

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
        Writer.WriteLine(DisplayNewBalance(),"white");

        // Add to statements
        _bankStatement.AddDeposit(UserAuth.UserAccountId, _amountToDeposit);
        Thread.Sleep(200);
        SemiUi();
    }

    private static double InputForDeposit()
    {
        Writer.Write("Enter amount to deposit: ", "green");
        try
        {
            _amountToDeposit = Convert.ToDouble(Console.ReadLine());
        }
        catch (Exception)
        {
            Writer.WriteLine(Messenger.InvalidInput(LimitDepositeProcess),"red");
            Deposit();
            return -1;
        }

        return _amountToDeposit;
    }

    private static bool IsValidDeposit(double amount)
    {
        if (amount > 150000)
        {
            Writer.WriteLine("You cannot deposit more than 150000$ per time", "red");
            return false;
        }

        if (!Validator.IsMultipleOf50Or100(amount))
        {
            Writer.WriteLine("Please enter a number multiples of 50 or 100: ", "red");
            return false;
        }

        return true;
    }

    private static void Balance()
    {
        Writer.WriteLine(DisplayBalance(),"white");
        SemiUi();
    }

    private static void Statements()
    {
        if (!AttemptsHandler.LetStatements()) return;

        var output = new StringBuilder();
        output.AppendLine("1. Withdraw Statements.");
        output.AppendLine("2. Deposit Statements.");
        output.AppendLine("3. Transactions Statements.");
        output.AppendLine("4. All Statements Operations");

        Writer.WriteLine("====* Statements section *====\n","white");
        Writer.Write(output.ToString(), "green"); // write the appended messages
        Writer.Write("\nYour Choice: ","white");

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
            Writer.WriteLine(Messenger.InvalidInput(LimitStatementProcess),"red");
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
                LimitStatementProcess = AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterByWithdraw(UserAuth.UserAccountId);
                break;
            case 2:
                Console.Clear();
                LimitStatementProcess = AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterByDeposit(UserAuth.UserAccountId);
                break;
            case 3:
                Console.Clear();
                LimitStatementProcess = AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterByTransaction(UserAuth.UserAccountId);
                break;
            case 4:
                Console.Clear();
                LimitStatementProcess = AttemptsHandler.ResetAttempts(LimitStatementProcess);
                _bankStatement.FilterById(UserAuth.UserAccountId);
                break;
            default:
                Console.Clear();
                Writer.WriteLine(Messenger.InvalidOption(LimitStatementProcess),"red");
                Statements();
                break;
        }
    }

    public static void ResetLimitations()
    {
        LimitSemiUi = AttemptsHandler.ANSI_CHANCES;
        LimitMainUi = AttemptsHandler.ANSI_CHANCES;
        LimitDepositeProcess = AttemptsHandler.ANSI_CHANCES;
        LimitWithdrawProcess = AttemptsHandler.ANSI_CHANCES;
        LimitTransferProcess = AttemptsHandler.ANSI_CHANCES;
        LimitIdToTransfer = AttemptsHandler.ANSI_CHANCES;
        LimitStatementProcess = AttemptsHandler.ANSI_CHANCES;
        LimitValueToTrans = AttemptsHandler.ANSI_CHANCES;
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
        AttemptsHandler.ResetSystemLimitations();

        SlowClearConsole(20);
        Console.WriteLine(Font.White("Thanks for using our SSM."));
        StartPoint();
    }
}