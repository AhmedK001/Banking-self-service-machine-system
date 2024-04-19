using BankingSelfServiceMachine.Data;
using BankingSelfServiceMachine.Managers;
using BankingSelfServiceMachine.Structures;
using BankingSelfServiceMachine.UI;

namespace BankingSelfServiceMachine.Security;

public class UserAuth : UserManager
{
    public static void Login()
    {
        LimitLogin = AttemptsManager.IncreaseAttempts(LimitLogin);
        if (AttemptsManager.IsExceededAttempts(LimitLogin, 5))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        try
        {
            DataManager.LoadAccountsData();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_WHITE + FontStyle.BOLD + "Here you can Login." + FontStyle.ANSI_RESET);
        GetIDInputForLoggingIn(); // continue
    }

    private static void GetIDInputForLoggingIn()
    {
        LimitLoginProcess = AttemptsManager.IncreaseAttempts(LimitLoginProcess);
        if (AttemptsManager.IsExceededAttempts(LimitLoginProcess, 5))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Enter your National ID..\n\n" +
                          FontStyle.ANSI_RESET);
        try
        {
            InputNationIdForLogin = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            GetIDInputForLoggingIn();
            return;
        }

        GetPasswordInputForLoggingIn(); // continue
    }

    private static void GetPasswordInputForLoggingIn()
    {
        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Enter your Password..\n\n" +
                          FontStyle.ANSI_RESET);
        try
        {
            InputtedPasswordForLoggingIn = Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
            Console.WriteLine("Invalid input!!");
            GetPasswordInputForLoggingIn();
            return;
        }

        // then
        CheckAccountValidity(InputNationIdForLogin, InputtedPasswordForLoggingIn);
    }

    private static void CheckAccountValidity(int inputNationIdForLogin, string inputPasswordForLogin)
    {
        UserBinaryTree.SearchOnTree(inputNationIdForLogin);
        // if national id does not exist
        if (UserBinaryTree.SearchMethodArray.Count == 0)
        {
            Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "National ID or password is incorrect!\n\n" +
                              FontStyle.ANSI_RESET);
            AttemptsManager.IncreaseAttempts(LimitLoginProcess); // Chances --;
            GetIDInputForLoggingIn();
            return;
        }

        // if national id and password doesn't match
        if (UserBinaryTree.SearchMethodArray[0].NationalID != inputNationIdForLogin ||
            !UserBinaryTree.SearchMethodArray[0].Password.Equals(inputPasswordForLogin))
        {
            Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "National ID or password is incorrect!\n\n" +
                              FontStyle.ANSI_RESET);
            AttemptsManager.IncreaseAttempts(LimitLoginProcess); // Chances --;
            GetIDInputForLoggingIn();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "\n\nSuccessfully logged in!" +
                          FontStyle.ANSI_RESET);
        SelfServiceMachine.MainUi(); // if passed continue
    } // --- END OF LOGGING IN PROCESS ---


    // --- START OF REGISTRATION PROCESS ---
    public static void Register()
    {
        try
        {
            DataManager.LoadAccountsData();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception in loading the data: " + e.Message);
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_WHITE + FontStyle.BOLD +
                          "Here you can quickly REGISTER in our system." + FontStyle.ANSI_RESET);

        RegisterFirstName(); // Start
    }

    private static void RegisterFirstName()
    {
        LimitRegisterFirstName = AttemptsManager.IncreaseAttempts(LimitRegisterFirstName);
        if (AttemptsManager.IsExceededAttempts(LimitRegisterFirstName, 5))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Enter your first name..\n\n" +
                          FontStyle.ANSI_RESET);
        try
        {
            InputtedFirstNameToRegister = Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            Console.WriteLine(e.StackTrace);
            RegisterFirstName();
            return;
        }

        if (!InputsFilter.IsItName(InputtedFirstNameToRegister))
        {
            Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "Invalid input!!" + FontStyle.ANSI_RESET);
            RegisterFirstName();
            return;
        }

        RegisterSecondName(); // if passed continue
    }

    private static void RegisterSecondName()
    {
        LimitRegisterSecondName = AttemptsManager.IncreaseAttempts(LimitRegisterSecondName);
        if (AttemptsManager.IsExceededAttempts(LimitRegisterSecondName, 5))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Enter you second name..\n\n" +
                          FontStyle.ANSI_RESET);
        try
        {
            InputtedSecondNameToRegister = Console.ReadLine();
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            Console.WriteLine(e.StackTrace);
            RegisterFirstName();
            return;
        }

        if (!InputsFilter.IsItName(InputtedSecondNameToRegister))
        {
            Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "Invalid input!!" + FontStyle.ANSI_RESET);
            RegisterSecondName();
            return;
        }

        RegisterNationID(); // if passed continue
    }

    private static void RegisterNationID()
    {
        LimitRegisterNationID = AttemptsManager.IncreaseAttempts(LimitRegisterNationID);
        if (AttemptsManager.IsExceededAttempts(LimitRegisterNationID, 3))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD +
                          "Enter your National ID (exactly 8 digits)..\n\n" + FontStyle.ANSI_RESET);
        try
        {
            InputtedNationalIdToRegister = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            Console.WriteLine(e.StackTrace);
            RegisterNationID();
            return;
        }

        if (!InputsFilter.IsItNationalId(InputtedNationalIdToRegister))
        {
            RegisterNationID();
            return;
        }

        CheckIfNationIdUnique(); // continue
    }

    private static void CheckIfNationIdUnique()
    {
        UserBinaryTree.SearchOnTree(InputtedNationalIdToRegister);
        if (UserBinaryTree.SearchMethodArray.Count == 0)
        {
            RegisterPassword(); // continue
            return;
        }

        if (InputtedNationalIdToRegister == UserBinaryTree.SearchMethodArray[0].NationalID)
        {
            Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "You have an account already! \n" +
                              FontStyle.ANSI_RESET);
            GetInputForLoginOrRegister();
        }
    }

    private static void RegisterPassword()
    {
        if (AttemptsManager.IsExceededAttempts(LimitPassword, 5))
        {
            AttemptsManager.HandleExceededAttempts();
            return;
        }

        LimitPassword = AttemptsManager.IncreaseAttempts(LimitPassword);

        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD +
                          "Enter Your Password..(Equal to or more than 8 chars.)\n\n" + FontStyle.ANSI_RESET);
        try
        {
            InputtedPasswordToRegister = Console.ReadLine();
        }
        catch (Exception e)
        {
            RegisterPassword();
            return;
        }

        if (!InputsFilter.IsItPassword(InputtedPasswordToRegister))
        {
            Console.WriteLine("Invalid input!!");
            RegisterPassword();
            return;
        }

        CompleteRegistration(); // if passed continue
    }

    private static void CompleteRegistration()
    {
        //if unique ID continue register password
        var newUser = new UserManager(InputtedFirstNameToRegister, InputtedSecondNameToRegister,
            InputtedNationalIdToRegister, InputtedPasswordToRegister);

        UserBinaryTree.InsertOnTheTree(newUser);
        UserBinaryTree.StoreTreeData();
        Console.WriteLine(FontStyle.ANSI_BRIGHT_WHITE + FontStyle.BOLD + "=========================" +
                          FontStyle.ANSI_RESET);
        Console.WriteLine(FontStyle.ANSI_BRIGHT_GREEN + FontStyle.BOLD + "Registered Successfully!!" +
                          FontStyle.ANSI_RESET);
        Console.WriteLine(FontStyle.ANSI_BRIGHT_WHITE + FontStyle.BOLD + "=========================\n\n" +
                          FontStyle.ANSI_RESET);
        Login();
    }
}