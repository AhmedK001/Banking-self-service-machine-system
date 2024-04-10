using System.Text.Json;

namespace SSM_IN_C_Sharp_;

public class DataManager
{
    
    public readonly string StatementsDataFile = Path.Combine(SolutionDirectory, "Data", "Statements.json");
    private BankStatement _bankStatement;
    
    
    public DataManager(BankStatement bankStatement)
    {
        _bankStatement = bankStatement;
    }

    public static readonly string SolutionDirectory =
        Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;


    public void StoreStatementData()
    {
        UserDataManager.HandleDataFile(StatementsDataFile); // make sure of the data file
        
        JsonSerializerOptions options = new() { WriteIndented = true };
        var serializedData = JsonSerializer.Serialize(_bankStatement.Statements, options);
        File.WriteAllText(StatementsDataFile, serializedData);
    }

    public void LoadStatementData()
    {
        UserDataManager.HandleDataFile(StatementsDataFile); // make sure of the data file
        
        var loadedStatementsData = File.ReadAllText(StatementsDataFile);
        List<StatementOperation> deserializedStatementsData = JsonSerializer.Deserialize<List<StatementOperation>>(loadedStatementsData);
        _bankStatement.Statements = deserializedStatementsData;
    }
}