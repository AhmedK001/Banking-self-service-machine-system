import java.io.*;
import java.util.NoSuchElementException;
import java.util.Scanner;


public class OrganizingMachine extends SelfServiceMachine {
    // Node identifiers
    static Node front;
    static Node rear;

    static String queueDataFile = "Data/OrganizingMachineData.txt";
    static int continueAfterRear = 0;
    static int rearMinusFront;
    static boolean storedFromFileQueueData = false;
    static Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) throws IOException {
        loadQueueDataFromFile();
        try {
            mainQueueUI();
        } catch (InterruptedException e) {
            System.err.println("Error during operation: " + e.getMessage());
        }
    }

    static void mainQueueUI() throws InterruptedException, IOException {
        System.out.println("\nPress 1 to enter the queue of the SSM\nPress 2 for more details\n");
        int choice = -1;
        try {
            choice = scanner.nextInt();
        } catch (Exception e) {
            e.getStackTrace();
            scanner.nextLine();
            mainQueueUI();
        }
        switch (choice) {
            case 1:
                continueAfterRear++;
                enqueue(continueAfterRear);
                break;
            case 2:
                machDetails();
                break;
            default:
                System.out.println("Invalid option. Please try again.");
                mainQueueUI();
                break;
        }
    }

    public static void enqueue(int value) throws IOException {
        Node newNode = new Node(value);
        if (front == null) {
            front = rear = newNode;
        } else {
            rear.next = newNode;
            newNode.prev = rear;
            rear = newNode;
        }
        System.out.println("Your turn number is: " + value);
        System.out.println("Customers before you: " + getRearMinusFront());
        saveQueueDataToFile();
        try {
            mainQueueUI();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }

    private static void addToQueue(int value) {
        Node newNode = new Node(value);
        if (front == null) {
            front = rear = newNode;
        } else {
            rear.next = newNode;
            newNode.prev = rear;
            rear = newNode;
        }
    }

    static void dequeue() {
        if (front == null) {
            front = rear = null;
        } else {
            front = front.next;
            front.prev = null;
            try {
                saveQueueDataToFile();
            } catch (IOException e) {
                System.out.println("Saving data $Dequeue Method:: Failed!");
                e.printStackTrace();
            }
        }
    }

    public static void saveQueueDataToFile() throws IOException {
        try (BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(queueDataFile))) {
            Node current = front;
            while (current != null) {
                bufferedWriter.write(Integer.toString(current.data));
                bufferedWriter.newLine();
                current = current.next;
            }
        } catch (IOException e) {
            System.err.println("Error storing data to file: " + e.getMessage());
        }
    }

    public static void loadQueueDataFromFile() throws IOException {
        if (!storedFromFileQueueData) {
            storedFromFileQueueData = true;
            File file = new File(queueDataFile);
            if (!file.exists()) {
                file.getParentFile().mkdirs();
                boolean fileCreated = false;
                try {
                    fileCreated = file.createNewFile();
                } catch (IOException e) {
                    e.printStackTrace();
                }
                if (!fileCreated) {
                    throw new IOException("Failed to create a new file: " + queueDataFile);
                }
            }
            Scanner scan = null;
            try {
                scan = new Scanner(file);
                while (scan.hasNextLine()) {
                    try {
                        int dataFromFile = scan.nextInt();
                        addToQueue(dataFromFile);
                        continueAfterRear = dataFromFile;
                    } catch (NoSuchElementException e) {
                    }
                }
            } catch (FileNotFoundException e) {
                e.printStackTrace();
            } finally {
                if (scan != null) {
                    scan.close();
                }
            }
        }
    }

    public static void resetQueueDataFile() throws IOException {
        FileWriter fileWriter = new FileWriter(queueDataFile);
        PrintWriter printWriter = new PrintWriter(fileWriter);
        printWriter.print("");
        printWriter.close();
    }

    static void machDetails() throws InterruptedException, IOException {
        System.out.println("\n\nThis is the Organizing Machine\nIn order to use our SSM Please pick an order.");
        mainQueueUI();
    }

    public static int getRearMinusFront() {
        return rearMinusFront = rear.data - front.data;
    }
}