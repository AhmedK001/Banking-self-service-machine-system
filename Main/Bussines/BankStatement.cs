using System.Text.Json.Serialization;

namespace Main;

public class BankStatement
{
    private int NationalId { set; get; }
    public int? ReciverId { set; get; }
    [JsonIgnore] public List<StatementOperation> Statements { set; get; }
    [JsonIgnore] public List<StatementOperation> SearchedStatements { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    public BankStatement()
    {
        Statements = new List<StatementOperation>();
        SearchedStatements = new List<StatementOperation>();
    }

    public void AddWithdraw(int nationalId, double amount)
    {
        Statements.Add(new StatementOperation(nationalId, Date, "Withdraw", amount));
        DataHandler dataHandler = new DataHandler(this);

        dataHandler.StoreStatementData();
    }

    public void AddDeposit(int nationalId, double amount)
    {
        Statements.Add(new StatementOperation(nationalId, Date, "Deposit", amount));
        DataHandler dataHandler = new DataHandler(this);
        dataHandler.StoreStatementData();
    }

    public void AddTransaction(int nationalId, double amount, int reciverId)
    {
        Statements.Add(new StatementOperation(nationalId, Date, "Transaction", amount, reciverId));
        DataHandler dataHandler = new DataHandler(this);
        dataHandler.StoreStatementData();
    }

    public void DisplayStatements(List<StatementOperation> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine(FontStyle.White($"\n{list[i].ToString()}\n"));
        }

        ServiceMachine.SemiUi();
    }

    public void FilterById(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalId == nationalId)
            {
                SearchedStatements.Add(Statements[i]);
            }
        }

        if (!SearchedStatements.Any())
        {
            Console.WriteLine(FontStyle.Red("No Operations found."));
            ServiceMachine.SemiUi();
            return;
        }

        Console.WriteLine(FontStyle.White(FilterByIdMessage()));
        DisplayStatements(SearchedStatements);
    }

    public void FilterByWithdraw(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalId == nationalId && Statements[i].Type.ToLower().Equals("withdraw"))
            {
                SearchedStatements.Add(Statements[i]);
            }
        }

        if (!SearchedStatements.Any())
        {
            Console.WriteLine(FontStyle.Red("No Operations found."));
            ServiceMachine.SemiUi();
            return;
        }

        Console.WriteLine(FontStyle.White(FilterByWithdrawMessage()));
        DisplayStatements(SearchedStatements);
    }

    public void FilterByDeposit(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalId == nationalId && Statements[i].Type.ToLower().Equals("deposit"))
            {
                SearchedStatements.Add(Statements[i]);
            }
        }

        if (!SearchedStatements.Any())
        {
            Console.WriteLine(FontStyle.Red("No Operations found."));
            ServiceMachine.SemiUi();
            return;
        }

        Console.WriteLine(FontStyle.White(FilterByDepositMessage()));
        DisplayStatements(SearchedStatements);
    }

    public void FilterByTransaction(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalId == nationalId && Statements[i].Type.ToLower().Equals("transaction"))
            {
                SearchedStatements.Add(Statements[i]);
            }
        }

        if (!SearchedStatements.Any())
        {
            Console.WriteLine(FontStyle.Red("No Operations found."));
            ServiceMachine.SemiUi();
            return;
        }

        Console.WriteLine(FontStyle.White(FilterByTransactionMessage()));
        DisplayStatements(SearchedStatements);
    }

    public void FilterStatementsByDate(int nationalId, DateTime time)
    {
    }

    public static string FilterByDepositMessage()
    {
        return "*==| Deposits Operations |==*";
    }

    public static string FilterByWithdrawMessage()
    {
        return "*==| Withdrawals Operations |==*";
    }

    public static string FilterByTransactionMessage()
    {
        return "*==| Transactions Operations |==*";
    }

    public static string FilterByIdMessage()
    {
        return "*==| All Your Account Operations |==*";
    }
}