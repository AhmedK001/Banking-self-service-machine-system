


public class TreeNode extends CustomerDataManagementBase {
    CustomerDataManagementBase data;
    TreeNode left;
    TreeNode right;
    TreeNode lowestValueInTree; // in order to reduce time complexity less that O(log(n))
    TreeNode highestValueInTree; //

    public TreeNode(CustomerDataManagementBase data) {
        this.data = data;
        this.left = null;
        this.right = null;
        this.lowestValueInTree = null;
        this.highestValueInTree = null;
    }

    public CustomerDataManagementBase getData() {
        return data;
    }

    public void setData(CustomerDataManagementBase data) {
        this.data = data;
    }

    public TreeNode getLeft() {
        return left;
    }

    public void setLeft(TreeNode left) {
        this.left = left;
    }

    public TreeNode getRight() {
        return right;
    }

    public void setRight(TreeNode right) {
        this.right = right;
    }

    public TreeNode getLowestValueInTree() {
        return lowestValueInTree;
    }

    public void setLowestValueInTree(TreeNode lowestValueInTree) {
        this.lowestValueInTree = lowestValueInTree;
    }

    public TreeNode getHighestValueInTree() {
        return highestValueInTree;
    }

    public void setHighestValueInTree(TreeNode highestValueInTree) {
        this.highestValueInTree = highestValueInTree;
    }
}