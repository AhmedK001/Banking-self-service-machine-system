import java.io.IOException;
import java.util.Scanner;

public class SelfServiceMachine {
	
	
    static int balance = 500;
    //Limitations
    static int semiUILimit = 0;
    static int mainUILimit = 0;
    static int depositeProcessLimit = 0;
    static int withdrawProcessLimit = 0;
    //
    //Operating attributes
    static int mainTurn = 0;
    static int rearMinusFront;
    static boolean isFinalCustomerPassed = false;
    //
    //Node identifiers
    static Node front;
	static Node rear;
	//
	//
    static Scanner scanner = new Scanner(System.in);

    public static void main(String[] args) throws InterruptedException, IOException {
        OrganizingMachine.storeFromFile();
        processStartPoint();
    }
    
    static void processStartPoint() throws InterruptedException, IOException {
    	
    	if(!isFinalCustomerPassed) {
        if (rear != null && front != null) {

            if (getRearMinusFront() >= 0) {
            	if(getRearMinusFront() == 0) {
            		isFinalCustomerPassed = true;        
            	}
                System.out.println("-----------------\nNext customer, number: " + front.data + " ");
                System.out.println("Remaining customers: " + getRearMinusFront() + "\n");
                mainUI();
            	}
            } else {
        	System.out.println("In order to use the SSM Please pick an order.");
        }
    	}else {
    		System.out.println("In order to use the SSM Please pick an order.");
    	}
    }
    static void mainUI() throws InterruptedException, IOException {
        if (mainUILimit < 5) {
            mainUILimit++;
            System.out.println("press 1 for withdraw\npress 2 for deposit");
            System.out.println("press 3 for balance\npress 4 to exit");
            int yoInput = scanner.nextInt();

            switch (yoInput) {
                case 1:
                    withdraw();
                    break;
                case 2:
                    deposit();
                    break;
                case 3:
                    balance();
                    break;
                case 4:
                    exit();
                    break;
                default:
                	System.out.println("Invalid option. Please try again.");
                    mainUI();
                    break;
            }
        } else {
            exit();
        }
    }
    static void semiUI() throws InterruptedException, IOException {
        if (semiUILimit < 3) {
            semiUILimit++;
            System.out.println("Press 1 return to the main menu\nPress 2 to exit");
            int yoInput = scanner.nextInt();

            switch (yoInput) {
                case 1:
                    mainUI();
                    break;
                case 2:
                    exit();
                    break;
                default:
                	System.out.println("Invalid option. Please try again.");
                    semiUI();
            }
        } else {
            exit();
        }
    }

    static void withdraw() throws InterruptedException, IOException {
        System.out.println("Enter the amount to withdraw..");
        int withdrawInput = scanner.nextInt();

        if (withdrawProcessLimit < 3) {
            withdrawProcessLimit++;
            if (withdrawInput <= balance) {
                if ((withdrawInput % 50 == 0 || withdrawInput % 100 == 0) && withdrawInput != 0) {
                    balance = balance - withdrawInput;
                    System.out.println("Your new balance is: " + balance + "$\n");
                    semiUI();
                } else {
                    System.out.println("Please enter a number multiples of 50 or 100..");
                    withdraw();
                }
            } else {
                System.out.println("Your balance is not enough!");
                System.out.println("Your balance is: " + balance + "$\n");
                mainUI();
            }
        } else {
            exit();
        }
    }

    static void deposit() throws InterruptedException, IOException {
        System.out.println("Please enter the amount to deposit..");
        int depositInput = scanner.nextInt();

        depositeProcessLimit++;
        if (depositeProcessLimit < 5) {
            if ((depositInput % 50 == 0 || depositInput % 100 == 0) && depositInput != 0) {
                balance = depositInput + balance;
                System.out.println("Your new balance is: " + balance + "$\n");
                Thread.sleep(200);
                semiUI();
            } else {
                System.out.println("Please enter a number multiples of 50 or 100..");
                deposit();
            }
        } else {
            exit();
        }
    }

    static void balance() throws InterruptedException, IOException {
        System.out.println("Your balance is: " + balance + "$\n");
        Thread.sleep(500);
        semiUI();
    }
    
    
    static void resetLimits() throws InterruptedException, IOException {
    	semiUILimit = 0;
        mainUILimit = 0;
        depositeProcessLimit = 0;
        withdrawProcessLimit = 0;
        balance = 500;
        mainTurn--;
        processStartPoint();
    }
    static void exit() throws InterruptedException, IOException {
        System.out.println("Thanks for using our SSM.");
        
        if(!isFinalCustomerPassed) {
        OrganizingMachine.dequeue();
        Thread.sleep(100);
        resetLimits();
        }else {
        	OrganizingMachine.resetFile();
        	System.out.println("In order to use the SSM Please pick an order.");
        }
    }
    
    public static int getRearMinusFront() {
		return rearMinusFront = rear.data - front.data;
	}
}
