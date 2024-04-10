using System.Text.Json.Serialization;

namespace SSM_IN_C_Sharp_;

public class BankStatement
{
    private int NationalID { set; get; }
    public int? ReciverID { set; get; }
    [JsonIgnore]
    public List<StatementOperation> Statements { set; get; }
    [JsonIgnore]
    public List<StatementOperation> SearchedStatements { get; set; }
    public DateTime Date { get; set; } = DateTime.Now;

    
    public static readonly string BOLD = "\u001B[1m";
    public static readonly string ANSI_RESET = "\u001B[0m";
    public static readonly string ANSI_RED = "\u001B[31m";
    public static readonly string ANSI_BRIGHT_GREEN = "\u001B[92m";
    public static readonly string ANSI_BRIGHT_WHITE = "\u001B[97m";

    public BankStatement()
    {
         Statements = new List<StatementOperation>();
         SearchedStatements = new List<StatementOperation>();
    }

    public void AddWithdraw(int nationalId, double amount)
    {
        Statements.Add(new StatementOperation(nationalId, Date, "Withdraw", amount));
        DataManager _dataManager = new DataManager(this);

        _dataManager.StoreStatementData();
    }

    public void AddDeposit(int nationalId, double amount)
    {
        Statements.Add(new StatementOperation(nationalId, Date, "Deposit", amount));
        DataManager dataManager = new DataManager(this);
        dataManager.StoreStatementData();
    }

    public void AddTransaction(int nationalId, double amount, int reciverID)
    {
        Statements.Add(new StatementOperation(nationalId, Date, "Transaction", amount, reciverID));
        DataManager _dataManager = new DataManager(this);
        _dataManager.StoreStatementData();
    }

    public void DisplayStatements(List<StatementOperation> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Console.WriteLine($"{ANSI_BRIGHT_WHITE+BOLD}\n{list[i].ToString()}\n{ANSI_RESET}");
        }
        SelfServiceMachine.SemiUi();
    }

    public void FilterStatementsByID(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalID == nationalId)
            {
                SearchedStatements.Add(Statements[i]);
            }
        }

        if (!SearchedStatements.Any())
        {
            Console.WriteLine(ANSI_RED+BOLD+"No Operations found."+ANSI_RESET);
            SelfServiceMachine.SemiUi();
            return;
        }
        
        DisplayStatements(SearchedStatements);
    }
    
    public void FilterStatementsByWithdraw(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalID == nationalId && Statements[i].Type.ToLower().Equals("withdraw"))
            {
                SearchedStatements.Add(Statements[i]);
            }
        }
        
        if (!SearchedStatements.Any())
        {
            Console.WriteLine("No Operations found.");
            SelfServiceMachine.SemiUi();
            return;
        }
        
        DisplayStatements(SearchedStatements);
    }

    public void FilterStatementsByDeposit(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalID == nationalId && Statements[i].Type.ToLower().Equals("deposit"))
            {
                SearchedStatements.Add(Statements[i]);
            }
        }
        
        if (!SearchedStatements.Any())
        {
            Console.WriteLine("No Operations found.");
            SelfServiceMachine.SemiUi();
            return;
        }
        
        DisplayStatements(SearchedStatements);

    }
    public void FilterStatementsByTransaction(int nationalId)
    {
        SearchedStatements.Clear();
        for (int i = 0; i < Statements.Count; i++)
        {
            if (Statements[i].NationalID == nationalId && Statements[i].Type.ToLower().Equals("transaction"))
            {
                SearchedStatements.Add(Statements[i]);
            }
        }
        
        if (!SearchedStatements.Any())
        {
            Console.WriteLine("No Operations found.");
            SelfServiceMachine.SemiUi();
            return;
        }
        
        DisplayStatements(SearchedStatements);
    }
    
    public void FilterStatementsByDate(int nationalId,DateTime time)
    {
    }
    
}
