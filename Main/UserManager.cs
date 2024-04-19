using BankingSelfServiceMachine.Data;
using BankingSelfServiceMachine.Security;
using BankingSelfServiceMachine.Structures;
using BankingSelfServiceMachine.UI;

namespace BankingSelfServiceMachine.Managers;

public class UserManager
{
    // Configurations
    private static UserManager customer = new();

    // Process helpers
    protected static int InputNationIdForLogin { get; set; }
    protected static string InputtedPasswordForLoggingIn { get; set; }
    protected static int InputtedNationalIdToRegister { get; set; }
    protected static string InputtedPasswordToRegister { get; set; }
    protected static string InputtedFirstNameToRegister { get; set; }
    protected static string InputtedSecondNameToRegister { get; set; }
    protected static int InputtedIdToTrans { get; set; }
    protected static int NationalIDforChangingPassword { get; set; }
    public static string PasswordForChangingPassword { get; set; }
    protected static string NewPasswordFirstTime { get; set; }
    protected static string NewPasswordSecondTime { get; set; }

    // Constructor data types
    public int NationalID { get; set; }
    public string Password { get; set; }
    public double Balance { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Limitations
    protected static int LimitRegisterNationID { get; set; }
    protected static int LimitLogin { get; set; }
    protected static int LimitPassword { get; set; }
    protected static int LimitIDForChangingPassword { get; set; }
    protected static int LimitOldPasswordToChangingPassword { get; set; }
    protected static int LimitLoginProcess { get; set; }
    protected static int LimitNewPasswordForChangingIt { get; set; }
    protected static int LimitRegisterFirstName { get; set; }
    protected static int LimitRegisterSecondName { get; set; }
    protected static int LimitInputForLoginOrRegister { get; set; }


    public UserManager(string firstName, string lastName, int NationalID, string password)
    {
        this.NationalID = NationalID;
        Password = password;
        Balance = 0.0;
        FirstName = firstName;
        LastName = lastName;
    }

    public UserManager()
    {
    }

    protected static void LoginOrRegister()
    {
        GetInputForLoginOrRegister();
    }

    protected static void GetInputForLoginOrRegister()
    {
        LimitInputForLoginOrRegister = AttemptsManager.IncreaseAttempts(LimitInputForLoginOrRegister);
        if (AttemptsManager.IsExceededAttempts(LimitInputForLoginOrRegister, 3))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.BOLD + FontStyle.ANSI_BRIGHT_WHITE + "Welcome to our SSM System.");
        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + "Press 1 for Log in to your account.");
        Console.WriteLine("Press 2 Register for a new account.\n" + FontStyle.ANSI_RESET);

        int inputForLoginOrRegister;
        try
        {
            inputForLoginOrRegister = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            LoginOrRegister();
            return;
        }

        switch (inputForLoginOrRegister)
        {
            case 1:
                UserAuth.Login();
                break;
            case 2:
                UserAuth.Register();
                break;
            default:
                LoginOrRegister();
                break;
        }
    }

    protected static void ChangePassword()
    {
        Console.WriteLine(FontStyle.ANSI_BRIGHT_WHITE + FontStyle.BOLD + "==============================" +
                          FontStyle.ANSI_RESET);
        Console.WriteLine(FontStyle.ANSI_BRIGHT_WHITE + FontStyle.BOLD + "Here you can change your password" +
                          FontStyle.ANSI_RESET);
        InputIdForChangingPassword();
    }

    private static void InputIdForChangingPassword()
    {
        LimitIDForChangingPassword = AttemptsManager.IncreaseAttempts(LimitIDForChangingPassword);
        if (AttemptsManager.IsExceededAttempts(LimitIDForChangingPassword, 3))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Enter your Account national ID..\n\n" +
                          FontStyle.ANSI_RESET);
        try
        {
            NationalIDforChangingPassword = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            InputIdForChangingPassword();
            return;
        }

        if (NationalIDforChangingPassword == UserBinaryTree.SearchMethodArray.First().NationalID)
        {
            InputOldPasswordForChangingPassword(); // continue
        }
        else
        {
            LimitIDForChangingPassword++;
            Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "National ID Number for the Account is wrong." +
                              FontStyle.ANSI_RESET);
            InputIdForChangingPassword();
        }
    }

    private static void InputOldPasswordForChangingPassword()
    {
        LimitOldPasswordToChangingPassword = AttemptsManager.IncreaseAttempts(LimitOldPasswordToChangingPassword);
        if (AttemptsManager.IsExceededAttempts(LimitOldPasswordToChangingPassword, 3))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Enter your old password..\n\n" +
                          FontStyle.ANSI_RESET);
        string oldPassword;
        try
        {
            oldPassword = Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            InputOldPasswordForChangingPassword();
            return;
        }

        if (oldPassword.Equals(UserBinaryTree.SearchMethodArray.First().Password))
        {
            InputNewPasswordForChangingIt(); // continue
        }
        else
        {
            Console.WriteLine("Invalid password!!");
            InputOldPasswordForChangingPassword();
        }
    }

    private static void InputNewPasswordForChangingIt()
    {
        LimitNewPasswordForChangingIt = AttemptsManager.IncreaseAttempts(LimitNewPasswordForChangingIt);
        if (AttemptsManager.IsExceededAttempts(LimitNewPasswordForChangingIt, 3))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Enter your new password..\n\n" +
                          FontStyle.ANSI_RESET);
        try
        {
            NewPasswordFirstTime = Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            InputNewPasswordForChangingIt();
            return;
        }

        if (NewPasswordFirstTime.Length < 8)
        {
            Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD +
                              "Invalid input: Passwords must consist of more than 7 characters." +
                              FontStyle.ANSI_RESET);
            InputNewPasswordForChangingIt(); // Repeat
        }
        else
        {
            if (!NewPasswordFirstTime.Equals(UserBinaryTree.SearchMethodArray.First().Password))
            {
                InputNewPasswordSecondTime(); // Continue
            }
            else
            {
                Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "Cannot use the same old password." +
                                  FontStyle.ANSI_RESET);
                InputNewPasswordForChangingIt();
            }
        }
    }

    private static void InputNewPasswordSecondTime()
    {
        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD +
                          "Enter your new password again to confirm..\n\n" + FontStyle.ANSI_RESET);

        try
        {
            NewPasswordSecondTime = Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            InputNewPasswordSecondTime(); // continue
            return;
        }

        if (NewPasswordFirstTime.Equals(NewPasswordSecondTime))
        {
            ChangeOldPasswordFinalStep(); // Continue
        }
        else
        {
            // Repeat the process
            Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "Does not match!!" + FontStyle.ANSI_RESET);
            InputNewPasswordForChangingIt();
        }
    }

    private static void ChangeOldPasswordFinalStep()
    {
        // Update password
        UserBinaryTree.SearchMethodArray.First().Password = NewPasswordSecondTime;
        // Update the new changes
        UserBinaryTree.UpdateNewChanges();
        Console.WriteLine(FontStyle.ANSI_BRIGHT_WHITE + FontStyle.BOLD + "\n\nChanged your password successfully!" +
                          FontStyle.ANSI_RESET);
        SelfServiceMachine.SemiUi();
    }

    public override string ToString()
    {
        return "\n FirstName: " + FirstName + "\n  LastName: " + LastName + "\nNationalID: " + NationalID +
               "\n   Balance: " + Balance + "$\n  Password: " + Password + "\n--------\n";
    }
}