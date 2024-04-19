using System.Text.Json;
using BankingSelfServiceMachine.Operations;
using BankingSelfServiceMachine.Structures;

namespace BankingSelfServiceMachine.Data;

public class DataManager
{
    public readonly string StatementsDataFile = Path.Combine(SolutionDirectory, "Data", "Statements.json");
    private static bool StoredFromFileAccountsData { get; set; }
    private BankStatement _bankStatement;


    public DataManager(BankStatement bankStatement)
    {
        _bankStatement = bankStatement;
    }

    public static readonly string SolutionDirectory =
        Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;


    public void StoreStatementData()
    {
        HandleDataFile(StatementsDataFile); // make sure of the data file

        JsonSerializerOptions options = new() { WriteIndented = true };
        var serializedData = JsonSerializer.Serialize(_bankStatement.Statements, options);
        File.WriteAllText(StatementsDataFile, serializedData);
    }

    public void LoadStatementData()
    {
        HandleDataFile(StatementsDataFile); // make sure of the data file

        var loadedStatementsData = File.ReadAllText(StatementsDataFile);
        List<StatementOperation> deserializedStatementsData =
            JsonSerializer.Deserialize<List<StatementOperation>>(loadedStatementsData);
        _bankStatement.Statements = deserializedStatementsData;
    }

    public static void LoadAccountsData()
    {
        if (!StoredFromFileAccountsData)
        {
            StoredFromFileAccountsData = true;
            DataManager.HandleDataFile(UserBinaryTree.TreeDataFile); // make sure of the data file

            UserBinaryTree.LoadTreeData();
            //Console.WriteLine("-----");
            UserBinaryTree.DisplayTree();
        }
    }


    public static void HandleDataFile(string filePath)
    {
        var file = new FileInfo(filePath);
        if (!file.Exists)
        {
            Directory.CreateDirectory(file.DirectoryName);
            var fileCreated = false;
            try
            {
                using (file.Create())
                {
                    fileCreated = true;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            if (!fileCreated) throw new IOException("Failed to create a new file: " + filePath);
        }
    }
}