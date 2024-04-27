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

    public void FilterStatementsById(int nationalId)
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

        DisplayStatements(SearchedStatements);
    }

    public void FilterStatementsByWithdraw(int nationalId)
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

        DisplayStatements(SearchedStatements);
    }

    public void FilterStatementsByDeposit(int nationalId)
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

        DisplayStatements(SearchedStatements);
    }

    public void FilterStatementsByTransaction(int nationalId)
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

        DisplayStatements(SearchedStatements);
    }

    public void FilterStatementsByDate(int nationalId, DateTime time)
    {
    }
}