namespace Main;

public class StatementOperation : BankStatement
{
    public int NationalId { get; set; }
    public string Type { set; get; }
    public double Amount { set; get; }
    //public string Description { set; get; }

    public StatementOperation()
    {
    }

    public StatementOperation(int nationalId, DateTime date, string type, double amount)
    {
        NationalId = nationalId;
        Type = type;
        Amount = amount;
    }

    public StatementOperation(int nationalId, DateTime date, string type, double amount, int reciverId)
    {
        NationalId = nationalId;
        Type = type;
        Amount = amount;
        ReciverId = reciverId;
    }

    public override string ToString()
    {
        if (ReciverId == null)
        {
            return $"National ID: {NationalId}\nDate: {Date}\nType: {Type}\nAmount: {Amount}";
        }
        else
        {
            return $"National ID: {NationalId}\nReciver ID: {ReciverId}\nDate: {Date}\nType: {Type}\nAmount: {Amount}";
        }
    }
}