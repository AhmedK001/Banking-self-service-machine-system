import com.fasterxml.jackson.core.type.TypeReference;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.SerializationFeature;

import java.io.File;
import java.io.IOException;
import java.util.*;

public class BinaryTree {
    private static TreeNode root;
    private static List<CustomerDataManagementBase> customerNodeDataList = new ArrayList<>();
    private static final ObjectMapper objectMapper = new ObjectMapper();
    private static final String TreeDataFile = "SSM_System/Data/TreeData.json";
    private static ArrayList<CustomerDataManagementBase> searchMethodArray = new ArrayList<>();
    private static ArrayList<CustomerDataManagementBase> searchMethodArrayForReceiver = new ArrayList<>();
    private static BinaryTree binaryTree = new BinaryTree();


    protected static void loadTreeData() throws IOException {
        try {
            List<CustomerDataManagementBase> loadedJsonDataArray = objectMapper.readValue(new File(TreeDataFile), new TypeReference<ArrayList<CustomerDataManagementBase>>() {
            });

//            System.out.println(loadedJsonDataArray);
            for (int i = 0; i < loadedJsonDataArray.size(); i++) {
                insertOnTheTree(loadedJsonDataArray.get(i));
            }
        } catch (Exception e) {
            e.getStackTrace();
        }
    }

    protected static void insertOnTheTree(CustomerDataManagementBase data) {
        if (getRoot() == null) {
            binaryTree.setRoot(new TreeNode(data));
            customerNodeDataList.add(getRoot().getData());
        } else {
            insertInOrder(data, getRoot());
        }
    }

    private static void insertInOrder(CustomerDataManagementBase data, TreeNode node) {
        int compareResult = Integer.compare(data.getNationalID(), node.getData().getNationalID());
        if (compareResult < 0) {
            if (node.getLeft() == null) {
                node.setLeft(new TreeNode(data));
                customerNodeDataList.add(node.getLeft().getData());
            } else {
                insertInOrder(data, node.getLeft());
            }
        } else if (compareResult > 0) {
            if (node.getRight() == null) {
                node.setRight(new TreeNode(data));
                customerNodeDataList.add(node.getRight().getData());
            } else {
                insertInOrder(data, node.getRight());
            }
        }
    }

    protected static void storeTreeData() {
        objectMapper.enable(SerializationFeature.INDENT_OUTPUT);
        try {
            objectMapper.writeValue(new File(TreeDataFile), getCustomerNodeDataList());
        } catch (IOException e) {
            System.err.println("Error storing tree data to file: ");
            e.printStackTrace();
        }
    }

    protected static void updateNewChanges() {
        try {
            for (int i = 0; i < getCustomerNodeDataList().size(); i++) {
                if (getCustomerNodeDataList().get(i).getNationalID() == getSearchMethodArray().get(0).getNationalID()) {
                    //Update the new balance
                    getCustomerNodeDataList().get(i).setBalance(getSearchMethodArray().get(0).getBalance());
                    //Update the password if any changes
                    System.out.println(getCustomerNodeDataList().get(i).getBalance());

                    if (!getCustomerNodeDataList().get(i).getPassword().equals(getSearchMethodArray().get(0).getPassword()))
                        getCustomerNodeDataList().get(i).setPassword(getSearchMethodArray().get(0).getPassword());
                }
                break;
            }
        } catch (Exception e) {
            e.getMessage();
        }
        storeTreeData(); // Update the changes to the tree
    }

    protected static void updateNewChangesForReceiver() {
        try {
            for (int i = 0; i < getCustomerNodeDataList().size(); i++) {
                if (getCustomerNodeDataList().get(i).getNationalID() == getSearchMethodArrayForReceiver().get(0).getNationalID()) {
                    //Update the new balance
                    getCustomerNodeDataList().get(i).setBalance(getSearchMethodArrayForReceiver().get(0).getBalance());
                    //Display the receiver new balance
                    System.out.println(getCustomerNodeDataList().get(i).getBalance());
                }
                break;
            }
        } catch (Exception e) {
            e.getMessage();
        }
        storeTreeData(); // Update the changes to the tree
    }

    protected static void searchOnTree(int nationalID) {
        searchMethodArray.clear(); // Clear the array before performing search
        if (getRoot() == null) {
            System.out.println("BinaryTree is empty.");
            return;
        }

        TreeNode node = getRoot();
        int compareResult;
        while (node != null) {
            compareResult = Integer.compare(nationalID, node.getData().getNationalID());
            if (compareResult == 0) {
                searchMethodArray.clear();
                searchMethodArray.add(node.getData());
//                System.out.println("Found: " + node.getData());
                return;
            }
            searchMethodArray.add(node.getData());
            if (compareResult < 0) {
                node = node.getLeft();
            }
            if (compareResult > 0) {
                node = node.getRight();
            }
        }
        searchMethodArray.clear();
        //System.out.println("Not found.");
    }

    protected static void searchOnTreeForReceiver(int nationalID) throws IOException, InterruptedException, ClassNotFoundException {
        getSearchMethodArrayForReceiver().clear(); // clear the array before performing search
        if (getRoot() == null) {
            System.out.println("BinaryTree is empty.");
            return;
        }

        TreeNode node = getRoot();
        int compareResult;
        while (node != null) {
            compareResult = Integer.compare(nationalID, node.getData().getNationalID());
            if (compareResult == 0) {
                getSearchMethodArrayForReceiver().clear();
                getSearchMethodArrayForReceiver().add(node.getData());
//                System.out.println("Found: " + node.getData());
                // when find receiver account continue
                SelfServiceMachine.checkExistsForReceiverAccount();
                return;
            }
            getSearchMethodArrayForReceiver().add(node.getData());
            if (compareResult < 0) {
                node = node.getLeft();
            }
            if (compareResult > 0) {
                node = node.getRight();
            }
        }
        getSearchMethodArrayForReceiver().clear();
        System.out.println("Not found.");
    }

    // method to display the binary tree if needed
    protected static void displayTree() {
        if (root == null) {
            System.out.println("BinaryTree is empty.");
            return;
        }
        Queue<TreeNode> queue = new LinkedList<>();
        queue.add(root);
        while (!queue.isEmpty()) {
            TreeNode node = queue.poll();
            System.out.println("=================");
            System.out.println("Node National ID: " + node.data.getNationalID());
            System.out.println("        Password: " + node.data.getPassword());
            System.out.println("         Balance: " + node.data.getBalance());
            System.out.println("      First Name: " + node.data.getFirstName());
            System.out.println("       Last Name: " + node.data.getLastName());
            System.out.println("            Root: " + node.data.getNationalID());
            System.out.print("       Left Node: ");
            if (node.left != null) {
                System.out.println(node.left.data.getNationalID());
                queue.add(node.left);
            } else {
                System.out.println("null");
            }
            System.out.print("      Right Node: ");
            if (node.right != null) {
                System.out.println(node.right.data.getNationalID());
                queue.add(node.right);
            } else {
                System.out.println("null");
            }
            System.out.println();
        }
    }

    protected void displayLowestValueInTree() {
        TreeNode node = root;
        if (root == null) {
            System.out.println("BinaryTree is empty.");
            return;
        }
        while (node.left != null) {
            node = node.left;
            node.lowestValueInTree = node;
                /*
                in some systems if the primary key for the node arranged in a sequential pattern
                We could use that method in order to reduce the time complexity
                it could be less than O(log(n)) in the some cases

                However, we could reverse routating pattern in order to reach what ever value we want
                if we noticed that it is near of the highest value in the tree or lowest value in the tree
                instead of starting from the root.
                 */
        }
        System.out.println("lowest ID value in the binary tree: " + node.lowestValueInTree.data.getNationalID());

    }

    protected void displayHighestValueInTree() {
        if (root == null) {
            System.out.println("BinaryTree is empty.");
            return;
        }
        TreeNode node = root;
        while (node.right != null) {
            node = node.right;
            node.highestValueInTree = node;
                /*
                in some systems if the primary key for the node arranged in a sequential pattern
                We could use that method in order to reduce the time complexity
                it could be less than O(log(n)) in the some cases

                However, we could reverse routating pattern in order to reach what ever value we want
                if we noticed that it is near of the highest value in the tree or lowest value in the tree
                instead of starting from the root.
                 */
        }
        System.out.println("highest ID value in the binary tree: " + node.highestValueInTree.data.getNationalID());
    }

    public static List<CustomerDataManagementBase> getCustomerNodeDataList() {
        return customerNodeDataList;
    }

    public static ArrayList<CustomerDataManagementBase> getSearchMethodArrayForReceiver() {
        return searchMethodArrayForReceiver;
    }

    public static void setSearchMethodArrayForReceiver(ArrayList<CustomerDataManagementBase> searchMethodArrayForReceiver) {
        BinaryTree.searchMethodArrayForReceiver = searchMethodArrayForReceiver;
    }

    public static List<CustomerDataManagementBase> getSearchMethodArray() {
        return searchMethodArray;
    }

    public static TreeNode getRoot() {
        return root;
    }

    public void setRoot(TreeNode root) {
        this.root = root;
    }
}
