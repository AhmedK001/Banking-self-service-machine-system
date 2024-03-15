import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;
import java.io.IOException;
import java.io.PrintWriter;
import java.util.Scanner;


public class OrganizingMachine {
    static String queueDataFile = "Data/queueData.txt";
	static int continueAfterRear = 0;
    static Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) throws IOException {
    	storeFromFile();
        try {
        	mainQueueUI();
        } catch (InterruptedException e) {
            System.err.println("Error during operation: " + e.getMessage());
        }
    }

    static void mainQueueUI() throws InterruptedException, IOException {
        System.out.println("\nPress 1 to enter the queue of the SSM\nPress 2 for more details\n");
        int choice = scanner.nextInt();
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
        if (SelfServiceMachine.front == null) {
            SelfServiceMachine.front = SelfServiceMachine.rear = newNode;
        } else {
            SelfServiceMachine.rear.next = newNode;
            newNode.prev = SelfServiceMachine.rear;
            SelfServiceMachine.rear = newNode;
        }
        System.out.println("Your turn number is: " + value);
        System.out.println("Customers before you: "+SelfServiceMachine.getRearMinusFront());
        storeToFile();
        try {
            mainQueueUI();
        } catch (InterruptedException e) {
            e.printStackTrace();
        }
    }
    private static void addToQueue(int value) {
        Node newNode = new Node(value);
        if (SelfServiceMachine.front == null) {
            SelfServiceMachine.front = SelfServiceMachine.rear = newNode;
        } else {
            SelfServiceMachine.rear.next = newNode;
            newNode.prev = SelfServiceMachine.rear;
            SelfServiceMachine.rear = newNode;
        }
    }
    static void dequeue() {
        if (SelfServiceMachine.front == null) {
            SelfServiceMachine.front = SelfServiceMachine.rear = null;
        } else {
            SelfServiceMachine.front = SelfServiceMachine.front.next;
            SelfServiceMachine.front.prev = null;
            try {
				storeToFile();
			} catch (IOException e) {
				System.out.println("Saving data $Dequeue Method:: Failed!");
				e.printStackTrace();
			}
        }
    }
    public static void displayFullQueue() {
        if (SelfServiceMachine.front == null) {
            System.out.println("Queue is empty");
            return;
        }
        Node current = SelfServiceMachine.front;
        while (current != null) {
            System.out.print(current.data + " ");
            current = current.next;
        }
        System.out.println();
    }
    public static void displayFinalQueueNum() {
        if (SelfServiceMachine.front == null) {
            System.out.println("Queue is empty");
            return;
        }
        
        Node current = SelfServiceMachine.front;
        while (current.next != null) {
            current = current.next;
        }
        System.out.println(current.data + " ");
    }


    public static void storeToFile() throws IOException {
        try (BufferedWriter bufferedWriter = new BufferedWriter(new FileWriter(queueDataFile))) {
            Node current = SelfServiceMachine.front;
            while (current != null) {
                bufferedWriter.write(Integer.toString(current.data));
                bufferedWriter.newLine();
                current = current.next;
            }
        } catch (IOException e) {
            System.err.println("Error writing to file: " + e.getMessage());
        }
    }
    public static void storeFromFile() throws IOException {
        File file = new File(queueDataFile);
        if (!file.exists()) {
            boolean fileCreated = file.createNewFile();
            if (!fileCreated) {
                throw new IOException("Failed to create new file: " + queueDataFile);
            }
        }
        Scanner scan = new Scanner(file);
        while (scan.hasNextLine()) {
            try {
                int dataFromFile = scan.nextInt();
                addToQueue(dataFromFile);
                continueAfterRear = dataFromFile;
            } catch (java.util.NoSuchElementException e) {
            	
            }
        }
        scan.close();
    }
    public static void resetFile() throws IOException { 
        FileWriter fileWriter = new FileWriter(queueDataFile);
        PrintWriter printWriter = new PrintWriter(fileWriter);
        printWriter.print("");
        printWriter.close();
    }
    static void machDetails() throws InterruptedException, IOException {
        System.out.println("\n\nThis is the Organizing Machine\nIn order to use our SSM Please pick an order.");
        mainQueueUI();
    }
}
