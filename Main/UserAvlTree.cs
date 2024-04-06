using System.Text.Json;

namespace SSM_IN_C_Sharp_;

public class UserAvlTree
{
    public static TreeNode Root { get; set; }
    public static List<UserDataManager> CustomerNodeDataList { get; } = new();

    public static readonly string SolutionDirectory =
        Directory.GetParent(Directory.GetCurrentDirectory())
            .Parent.Parent.FullName;

    public static readonly string TreeDataFile = Path.Combine(SolutionDirectory,
        "Data",
        "TreeData.json");

    public static List<UserDataManager> SearchMethodArray { get; } = new();
    public static List<UserDataManager> SearchMethodArrayForReceiver { get; } = new();
    public static UserAvlTree userAvlTree { get; } = new();

    public static void LoadTreeData()
    {
        try
        {
            var contens = File.ReadAllText(TreeDataFile);
            //Console.WriteLine(contens);
            var deserializedUsers = JsonSerializer.Deserialize<List<UserDataManager>>(contens);

            for (var i = 0;
                 i < deserializedUsers.Count;
                 i++)
                //Console.WriteLine(deserializedUsers[i]);
                InsertOnTheTree(deserializedUsers[i]); // insert the data to tree
        }
        catch (Exception e)
        {
            Console.WriteLine("Error loading tree data: " + e.Message);
        }
    }

    public static void InsertOnTheTree(UserDataManager data)
    {
        if (Root == null)
        {
            Root = new TreeNode(data);
            CustomerNodeDataList.Add(Root.Data);
        }
        else
        {
            InsertInOrder(data,
                Root);
        }
    }

    public static void InsertInOrder(UserDataManager data,
        TreeNode node)
    {
        var compareResult = data.NationalID.CompareTo(node.Data.NationalID);
        if (compareResult < 0)
        {
            if (node.Left == null)
            {
                node.Left = new TreeNode(data);
                CustomerNodeDataList.Add(node.Left.Data);
            }
            else
            {
                InsertInOrder(data,
                    node.Left);
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
                InsertInOrder(data,
                    node.Right);
            }
        }
    }

    public static void StoreTreeData()
    {
        JsonSerializerOptions options = new()
        {
            WriteIndented = true
        };

        try
        {
            var contents = JsonSerializer.Serialize(CustomerNodeDataList,
                options);
            File.WriteAllText(TreeDataFile,
                contents);
        }
        catch (Exception e)
        {
            Console.WriteLine("Error storing tree data to file: " + e.Message);
        }
    }

    public static void UpdateNewChanges()
    {
        try
        {
            foreach (var data in CustomerNodeDataList)
            {
                if (data.NationalID == SearchMethodArray[0].NationalID)
                {
                    // Update the new balance
                    data.Balance = SearchMethodArray[0].Balance;
                    // Update the password if any changes
                    Console.WriteLine(data.Balance);

                    if (data.Password != SearchMethodArray[0].Password)
                        data.Password = SearchMethodArray[0].Password;
                }

                break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error updating new changes: " + e.Message);
        }

        StoreTreeData(); // Update the changes to the tree
    }

    public static void UpdateNewChangesForReceiver()
    {
        try
        {
            foreach (var data in CustomerNodeDataList)
            {
                if (data.NationalID == SearchMethodArrayForReceiver[0].NationalID)
                {
                    // Update the new balance
                    data.Balance = SearchMethodArrayForReceiver[0].Balance;
                    // Display the receiver new balance
                    Console.WriteLine(data.Balance);
                }

                break;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine("Error updating new changes for receiver: " + e.Message);
        }

        StoreTreeData(); // Update the changes to the tree
    }

    public static void SearchOnTree(int nationalID)
    {
        SearchMethodArray.Clear(); // Clear the array before performing search
        if (Root == null)
            // Console.WriteLine("UserAvlTree is empty.");
            return;

        var node = Root;
        while (node != null)
        {
            var compareResult = nationalID.CompareTo(node.Data.NationalID);
            if (compareResult == 0)
            {
                SearchMethodArray.Clear();
                SearchMethodArray.Add(node.Data);
                // Console.WriteLine("Found: " + node.Data);
                return;
            }

            SearchMethodArray.Add(node.Data);
            if (compareResult < 0)
                node = node.Left;
            if (compareResult > 0)
                node = node.Right;
        }

        SearchMethodArray.Clear();
        // Console.WriteLine("Not found.");
    }


    public static void SearchOnTreeForReceiver(int nationalID)
    {
        SearchMethodArrayForReceiver.Clear(); // Clear the array before performing search
        if (Root == null)
        {
            Console.WriteLine("UserAvlTree is empty.");
            return;
        }

        var node = Root;
        while (node != null)
        {
            var compareResult = nationalID.CompareTo(node.Data.NationalID);
            if (compareResult == 0)
            {
                SearchMethodArrayForReceiver.Clear();
                SearchMethodArrayForReceiver.Add(node.Data);
                // Console.WriteLine("Found: " + node.Data);
                // When find receiver account, continue
                SelfServiceMachine.CheckExistsForReceiverAccount();
                return;
            }

            SearchMethodArrayForReceiver.Add(node.Data);
            if (compareResult < 0)
                node = node.Left;
            if (compareResult > 0)
                node = node.Right;
        }

        SearchMethodArrayForReceiver.Clear();
        Console.WriteLine(SelfServiceMachine.ANSI_RED +
                          SelfServiceMachine.BOLD +
                          "\n\nNot found." +
                          SelfServiceMachine.ANSI_RESET);
        SelfServiceMachine.SemiUI();
    }

    // Method to display the binary tree if needed
    public static void DisplayTree()
    {
        if (Root == null)
        {
            Console.WriteLine("UserAvlTree is empty.");
            return;
        }

        var queue = new Queue<TreeNode>();
        queue.Enqueue(Root);

        while (queue.Count > 0)
        {
            var node = queue.Dequeue();
            Console.WriteLine("=================");
            Console.WriteLine("Node National ID: " + node.Data.NationalID);
            Console.WriteLine("        Password: " + node.Data.Password);
            Console.WriteLine("         Balance: " + node.Data.Balance);
            Console.WriteLine("      First Name: " + node.Data.FirstName);
            Console.WriteLine("       Last Name: " + node.Data.LastName);
            Console.WriteLine("            Root: " + node.Data.NationalID);

            Console.Write("       Left Node: ");
            if (node.Left != null)
            {
                Console.WriteLine(node.Left.Data.NationalID);
                queue.Enqueue(node.Left);
            }
            else
            {
                Console.WriteLine("null");
            }

            Console.Write("      Right Node: ");
            if (node.Right != null)
            {
                Console.WriteLine(node.Right.Data.NationalID);
                queue.Enqueue(node.Right);
            }
            else
            {
                Console.WriteLine("null");
            }

            Console.WriteLine();
        }
    }

    public void DisplayLowestValueInTree()
    {
        var node = Root;
        if (Root == null)
        {
            Console.WriteLine("UserAvlTree is empty.");
            return;
        }

        while (node.Left != null)
        {
            node = node.Left;
            node.LowestValueInTree = node;
            /*
            in some systems if the primary key for the node arranged in a sequential pattern
            We could use that method in order to reduce the time complexity
            it could be less than O(log(n)) in the some cases

            However, we could reverse rotating pattern in order to reach whatever value we want
            if we noticed that it is near of the highest value in the tree or lowest value in the tree
            instead of starting from the root.
             */
        }

        Console.WriteLine("lowest ID value in the binary tree: " + node.LowestValueInTree.Data.NationalID);
    }

    public void DisplayHighestValueInTree()
    {
        if (Root == null)
        {
            Console.WriteLine("UserAvlTree is empty.");
            return;
        }

        var node = Root;
        while (node.Right != null)
        {
            node = node.Right;
            node.HighestValueInTree = node;
            /*
            in some systems if the primary key for the node arranged in a sequential pattern
            We could use that method in order to reduce the time complexity
            it could be less than O(log(n)) in some cases

            However, we could reverse rotating pattern in order to reach whatever value we want
            if we noticed that it is near of the highest value in the tree or lowest value in the tree
            instead of starting from the root.
             */
        }

        Console.WriteLine("highest ID value in the binary tree: " + node.HighestValueInTree.Data.NationalID);
    }
}