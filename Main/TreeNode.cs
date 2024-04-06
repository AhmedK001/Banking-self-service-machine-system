namespace SSM_IN_C_Sharp_;

using System;

public class TreeNode : UserDataManager
{
    public UserDataManager Data { get; set; }
    public TreeNode Left { get; set; }
    public TreeNode Right { get; set; }
    public TreeNode LowestValueInTree { get; set; }
    public TreeNode HighestValueInTree { get; set; }

    public TreeNode(UserDataManager data)
    {
        Data = data;
        Left = null;
        Right = null;
        LowestValueInTree = null;
        HighestValueInTree = null;
    }
}