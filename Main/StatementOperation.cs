namespace SSM_IN_C_Sharp_;

public class StatementOperation : BankStatement
{
    public int NationalID { get; set; }
    public string Type { set; get; }
    public double Amount { set; get; }
    //public string Description { set; get; }

    public StatementOperation()
    {
    }

    public StatementOperation(int nationalId, DateTime date, string type, double amount)
    {
        NationalID = nationalId;
        Type = type;
        Amount = amount;
    }

    public StatementOperation(int nationalId, DateTime date, string type, double amount, int reciverId)
    {
        NationalID = nationalId;
        Type = type;
        Amount = amount;
        ReciverID = reciverId;
    }

    public override string ToString()
    {
        if (ReciverID == null)
        {
            return
                $"National ID: {NationalID}\nDate: {Date}\nType: {Type}\nAmount: {Amount}";
        }
        else 
        {
            return
                $"National ID: {NationalID}\nReciver ID: {ReciverID}\nDate: {Date}\nType: {Type}\nAmount: {Amount}";
        }
    }
}