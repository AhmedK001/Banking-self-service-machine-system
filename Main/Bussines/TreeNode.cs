namespace Main;

public class TreeNode
{
    public User Data { get; set; }
    public TreeNode? Left { get; set; }
    public TreeNode? Right { get; set; }
    public TreeNode? LowestValueInTree { get; set; }
    public TreeNode? HighestValueInTree { get; set; }

    public TreeNode(User data)
    {
        Data = data;
        Left = null;
        Right = null;
        LowestValueInTree = null;
        HighestValueInTree = null;
    }
}