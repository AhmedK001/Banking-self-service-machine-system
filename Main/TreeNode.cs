using BankingSelfServiceMachine.Managers;

namespace BankingSelfServiceMachine.Structures;

using System;

public class TreeNode : UserManager
{
    public UserManager Data { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }
    public TreeNode LowestValueInTree { get; set; }
    public TreeNode HighestValueInTree { get; set; }

    public TreeNode(UserManager data)
    {
        Data = data;
        Left = null;
        Right = null;
        LowestValueInTree = null;
        HighestValueInTree = null;
    }
}