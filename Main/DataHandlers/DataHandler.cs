using System.Text.Json;

namespace Main;

public class DataHandler
{
    public readonly string StatementsDataFile = Path.Combine(ANSI_SOLUTION_DIRECTORY, "Data", "Statements.json");
    private static bool StoredFromFileAccountsData { get; set; }
    private BankStatement _bankStatement;


    public DataHandler(BankStatement bankStatement)
    {
        _bankStatement = bankStatement;
    }

    public static readonly string ANSI_SOLUTION_DIRECTORY =
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
        List<StatementOperation>? deserializedStatementsData =
            JsonSerializer.Deserialize<List<StatementOperation>>(loadedStatementsData);
        _bankStatement.Statements = deserializedStatementsData;
    }

    public static void LoadAccountsData()
    {
        if (!StoredFromFileAccountsData)
        {
            StoredFromFileAccountsData = true;
            HandleDataFile(TreeManager.ANSI_TREE_DATA_FILE); // make sure of the data file

            TreeManager.LoadTreeData();
            //Console.WriteLine("-----");
            TreeManager.DisplayTree();
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
            catch (IOException)
            {
            }

            if (!fileCreated) throw new IOException("Failed to create a new file: " + filePath);
        }
    }
}