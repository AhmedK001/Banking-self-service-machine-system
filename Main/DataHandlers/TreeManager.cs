using System.Text;
using System.Text.Json;

namespace Main;

public class TreeManager
{
    public static TreeNode? Root { get; set; }
    public static List<User> CustomerNodeDataList { get; } = new();

    public static readonly string ANSI_SOLUTION_DIRECTORY =
        Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

    public static readonly string ANSI_TREE_DATA_FILE = Path.Combine(ANSI_SOLUTION_DIRECTORY, "Data", "TreeData.json");

    public static List<User>? SearchMethodArray { get; } = new();
    public static List<User> SearchMethodArrayForReceiver { get; } = new();

    public static void LoadTreeData()
    {
        try
        {
            var contents = File.ReadAllText(ANSI_TREE_DATA_FILE);
            var deserializedUsers = JsonSerializer.Deserialize<List<User>>(contents);

            for (var i = 0; i < deserializedUsers.Count; i++)
                InsertOnTheTree(deserializedUsers[i]); // insert the data to tree
        }
        catch (Exception e)
        {
            Writer.WriteLine("Error loading tree data: " + e.Message, "red");
        }
    }

    public static void InsertOnTheTree(User data)
    {
        if (IsTreeEmpty())
        {
            Root = new TreeNode(data);
            CustomerNodeDataList.Add(Root.Data);
        }
        else
        {
            InsertInOrder(data, Root);
        }
    }

    public static void InsertInOrder(User data, TreeNode? node)
    {
        var compareResult = data.NationalId.CompareTo(node.Data.NationalId);
        if (compareResult < 0)
        {
            if (node.Left == null)
            {
                node.Left = new TreeNode(data);
                CustomerNodeDataList.Add(node.Left.Data);
            }
            else
            {
                InsertInOrder(data, node.Left);
            }
        }
        else if (compareResult > 0)
        {
            if (node.Right == null)
            {
                node.Right = new TreeNode(data);
                CustomerNodeDataList.Add(node.Right.Data);
            }
            else
            {
                InsertInOrder(data, node.Right);
            }
        }
    }

    public static void StoreTreeData()
    {
        JsonSerializerOptions options = new() { WriteIndented = true };

        try
        {
            var contents = JsonSerializer.Serialize(CustomerNodeDataList, options);
            File.WriteAllText(ANSI_TREE_DATA_FILE, contents);
        }
        catch (Exception e)
        {
            Writer.WriteLine("Error storing tree data to file: " + e.Message, "red");
        }
    }

    public static void UpdateNewChanges()
    {
        try
        {
            foreach (var data in CustomerNodeDataList)
            {
                if (data.NationalId == SearchMethodArray[0].NationalId)
                {
                    // Update the new balance
                    data.Balance = SearchMethodArray[0].Balance;
                    // Update the password if any changes
                    Writer.WriteLine(data.Balance.ToString(), "green");

                    if (data.Password != SearchMethodArray[0].Password)
                        data.Password = SearchMethodArray[0].Password;
                }

                break;
            }
        }
        catch (Exception e)
        {
            Writer.WriteLine("Error updating new changes: " + e.Message, "red");
        }

        StoreTreeData(); // Update the changes to the tree
    }

    public static void UpdateNewChangesForReceiver()
    {
        try
        {
            foreach (var data in CustomerNodeDataList)
            {
                if (data.NationalId == SearchMethodArrayForReceiver[0].NationalId)
                {
                    // Update the new balance
                    data.Balance = SearchMethodArrayForReceiver[0].Balance;
                    // Display the receiver new balance
                    Writer.WriteLine(data.Balance.ToString(), "green");
                }

                break;
            }
        }
        catch (Exception e)
        {
            Writer.WriteLine("Error updating new changes for receiver: " + e.Message, "red");
        }

        StoreTreeData(); // Update the changes to the tree
    }

    public static void SearchOnTree(int nationalId)
    {
        SearchMethodArray.Clear(); // Clear the array before performing search
        if (IsTreeEmpty()) return;

        var node = Root;
        while (node != null)
        {
            var compareResult = nationalId.CompareTo(node.Data.NationalId);
            if (compareResult == 0)
            {
                SearchMethodArray.Clear();
                SearchMethodArray.Add(node.Data);
                // Writer.WriteLine("Found: " + node.Data, "white");
                return;
            }

            SearchMethodArray.Add(node.Data);
            if (compareResult < 0)
                node = node.Left;
            if (compareResult > 0)
                node = node?.Right;
        }

        SearchMethodArray.Clear();
        // Writer.WriteLine("Not found.", "red");
    }

    public static bool IsUsedId(int nationalId)
    {
        if (IsTreeEmpty()) return false;

        var node = Root;
        while (node != null)
        {
            var result = CompareIds(node, nationalId);
            if (result == 0)
            {
                return true;
            }

            if (result < 0)
                node = node.Left;
            if (result > 0)
                node = node?.Right;
        }

        return false;
    }

    private static bool IsTreeEmpty()
    {
        return Root == null;
    }

    private static int CompareIds(TreeNode node, int id)
    {
        return id.CompareTo(node.Data.NationalId);
    }

    public static void SearchOnTreeForReceiver(int nationalId)
    {
        SearchMethodArrayForReceiver.Clear(); // Clear the array before performing search
        if (Root == null)
        {
            Writer.WriteLine("UserAvlTree is empty.", "red");
            return;
        }

        var node = Root;
        while (node != null)
        {
            var compareResult = nationalId.CompareTo(node.Data.NationalId);
            if (compareResult == 0)
            {
                SearchMethodArrayForReceiver.Clear();
                SearchMethodArrayForReceiver.Add(node.Data);
                // Writer.WriteLine("Found: " + node.Data, "white");
                // When find receiver account, continue
                //ServiceMachine.CheckExistsForReceiverAccount();
                return;
            }

            SearchMethodArrayForReceiver.Add(node.Data);
            if (compareResult < 0)
                node = node.Left;
            if (compareResult > 0)
                node = node?.Right;
        }

        SearchMethodArrayForReceiver.Clear();
        Writer.WriteLine("\n\nNot found.", "red");
        ServiceMachine.SemiUi();
    }

    // Method to display the binary tree if needed
    public static void DisplayTree()
    {
        if (IsTreeEmpty())
        {
            Writer.WriteLine("User Tree is empty.", "red");
            return;
        }

        var queue = new Queue<TreeNode?>();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            Writer.WriteLine(Font.SpaceLine(), "white");
            var output = new StringBuilder();
            output.AppendLine("Node National ID: " + node.Data.NationalId);
            output.AppendLine("        Password: " + node.Data.Password);
            output.AppendLine("         Balance: " + node.Data.Balance);
            output.AppendLine("      First Name: " + node.Data.FirstName);
            output.AppendLine("       Last Name: " + node.Data.LastName);
            output.AppendLine("            Root: " + node.Data.NationalId);

            Writer.WriteLine(output.ToString(), "white");

            Writer.Write("       Left Node: ", "white");
            if (node.Left != null)
            {
                Writer.WriteLine(node.Left.Data.NationalId.ToString(), "white");
                queue.Enqueue(node.Left);
            }
            else
            {
                Writer.WriteLine("null", "white");
            }

            Writer.Write("      Right Node: ", "white");
            if (node.Right != null)
            {
                Writer.WriteLine(node.Right.Data.NationalId.ToString(), "white");
                queue.Enqueue(node.Right);
            }
            else
            {
                Writer.WriteLine("null", "white");
            }

            Writer.WriteLine("", "white");
        }
    }
}
