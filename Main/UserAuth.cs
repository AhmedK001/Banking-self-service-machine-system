namespace Main;

public class UserAuth : User
{
    public static int UserAccountId { get; set; }
    //public static int InputtedIdToTrans { get; set; }
    public static int NationalIdForChangingPassword { get; set; }
    public static string? NewPasswordFirstTime { get; set; }
    public static string? NewPasswordSecondTime { get; set; }

    private static string? InputtedPasswordForLoggingIn { get; set; }
    private static int InputtedNationalIdToRegister { get; set; }
    private static string? InputtedPasswordToRegister { get; set; }
    private static string? InputtedFirstNameToRegister { get; set; }
    private static string? InputtedSecondNameToRegister { get; set; }

    public static int LimitLogin { get; set; }
    public static int LimitLoginProcess { get; set; }
    public static int LimitPassword { get; set; }
    public static int LimitRegisterNationId { get; set; }
    public static int LimitRegisterFirstName { get; set; }
    public static int LimitRegisterSecondName { get; set; }


    public static void Login()
    {
        LimitLogin = AttemptsHandler.IncreaseAttempts(LimitLogin);
        if (AttemptsHandler.IsExceededAttempts(LimitLogin, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        try
        {
            DataHandler.LoadAccountsData();
        }
        catch (Exception)
        {
            // ignored
        }

        Console.WriteLine(FontStyle.White("\n====* Here you can Login. *===="));
        GetIdInputForLoggingIn(); // continue
    }

    private static void GetIdInputForLoggingIn()
    {
        LimitLoginProcess = AttemptsHandler.IncreaseAttempts(LimitLoginProcess);
        if (AttemptsHandler.IsExceededAttempts(LimitLoginProcess, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }


        Console.Write(FontStyle.Green("Enter your National ID: "));
        try
        {
            UserAccountId = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            GetIdInputForLoggingIn();
            return;
        }

        GetPasswordInputForLoggingIn(); // continue
    }

    private static void GetPasswordInputForLoggingIn()
    {
        Console.Write(FontStyle.Green("Enter your Password: "));
        try
        {
            InputtedPasswordForLoggingIn = Console.ReadLine();
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid input!!");
            GetPasswordInputForLoggingIn();
            return;
        }

        // then
        CheckAccountValidity(UserAccountId, InputtedPasswordForLoggingIn);
    }

    private static void CheckAccountValidity(int inputNationIdForLogin, string? inputPasswordForLogin)
    {
        TreeManager.SearchOnTree(inputNationIdForLogin);
        // if national id does not exist
        if (TreeManager.SearchMethodArray.Count == 0)
        {
            Console.WriteLine(FontStyle.Red("National ID or password is incorrect!\n\n"));
            AttemptsHandler.IncreaseAttempts(LimitLoginProcess); // Chances --;
            GetIdInputForLoggingIn();
            return;
        }

        // if national id and password doesn't match
        var password = TreeManager.SearchMethodArray[0].Password;
        if (password != null && (TreeManager.SearchMethodArray[0].NationalId != inputNationIdForLogin ||
                                 !password.Equals(inputPasswordForLogin)))
        {
            Console.WriteLine(FontStyle.Red("National ID or password is incorrect!\n\n"));
            AttemptsHandler.IncreaseAttempts(LimitLoginProcess); // Chances --;
            GetIdInputForLoggingIn();
            return;
        }
        
        //Console.WriteLine(FontStyle.White("====* *====================="));
        Console.Clear();
        Console.Write(FontStyle.Red("\n==> "));
        Console.Write(FontStyle.Green("Successfully logged in!"));
        Console.WriteLine(FontStyle.Red(" <=="));
        ServiceMachine.MainUi(); // if passed continue
    } // --- END OF LOGGING IN PROCESS ---


    // --- START OF REGISTRATION PROCESS ---
    public static void Register()
    {
        try
        {
            DataHandler.LoadAccountsData();
        }
        catch (Exception)
        {
            Console.WriteLine("Exception in loading the data: ");
        }

        Console.WriteLine(FontStyle.White("====* Registration *===="));

        RegisterFirstName(); // Start
    }

    private static void RegisterFirstName()
    {
        LimitRegisterFirstName = AttemptsHandler.IncreaseAttempts(LimitRegisterFirstName);
        if (AttemptsHandler.IsExceededAttempts(LimitRegisterFirstName, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.Write(FontStyle.Green("Enter your first name: "));
        try
        {
            InputtedFirstNameToRegister = Console.ReadLine();
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid input!!");
            RegisterFirstName();
            return;
        }

        if (!InputsFilter.IsItName(InputtedFirstNameToRegister))
        {
            Console.WriteLine(FontStyle.Red("Invalid input!!"));
            RegisterFirstName();
            return;
        }

        RegisterSecondName(); // if passed continue
    }

    private static void RegisterSecondName()
    {
        LimitRegisterSecondName = AttemptsHandler.IncreaseAttempts(LimitRegisterSecondName);
        if (AttemptsHandler.IsExceededAttempts(LimitRegisterSecondName, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.Write(FontStyle.Green("Enter you second name: "));
        try
        {
            InputtedSecondNameToRegister = Console.ReadLine();
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid input!!");
            RegisterFirstName();
            return;
        }

        if (!InputsFilter.IsItName(InputtedSecondNameToRegister))
        {
            Console.WriteLine(FontStyle.Red("Invalid input!!"));
            RegisterSecondName();
            return;
        }

        RegisterNationId(); // if passed continue
    }

    private static void RegisterNationId()
    {
        LimitRegisterNationId = AttemptsHandler.IncreaseAttempts(LimitRegisterNationId);
        if (AttemptsHandler.IsExceededAttempts(LimitRegisterNationId, 3))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.Write(FontStyle.Green("Enter your National ID (exactly 8 digits): "));
        try
        {
            InputtedNationalIdToRegister = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid input!!");
            RegisterNationId();
            return;
        }

        if (!InputsFilter.IsItNationalId(InputtedNationalIdToRegister))
        {
            RegisterNationId();
            return;
        }

        CheckIfNationIdUnique(); // continue
    }

    private static void CheckIfNationIdUnique()
    {
        TreeManager.SearchOnTree(InputtedNationalIdToRegister);
        if (TreeManager.SearchMethodArray.Count == 0)
        {
            RegisterPassword(); // continue
            return;
        }

        if (InputtedNationalIdToRegister == TreeManager.SearchMethodArray[0].NationalId)
        {
            Console.WriteLine(FontStyle.Green("You have an account already! \n"));
            ServiceMachine.GetInputForLoginOrRegister();
        }
    }

    private static void RegisterPassword()
    {
        if (AttemptsHandler.IsExceededAttempts(LimitPassword, 5))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        LimitPassword = AttemptsHandler.IncreaseAttempts(LimitPassword);

        Console.Write(FontStyle.Green("Enter Your Password..(Equal to or more than 8 chars.): "));
        try
        {
            InputtedPasswordToRegister = Console.ReadLine();
        }
        catch (Exception)
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
        var newUser = new User(InputtedFirstNameToRegister, InputtedSecondNameToRegister, InputtedNationalIdToRegister,
            InputtedPasswordToRegister);

        TreeManager.InsertOnTheTree(newUser);
        TreeManager.StoreTreeData();
        Console.WriteLine(FontStyle.SpaceLine());
        Console.WriteLine(FontStyle.Green("=* Registered Successfully!! *="));
        Console.WriteLine(FontStyle.SpaceLine() + "\n\n");

        Login();
    }

    public static void ResetOldData()
    {
        UserAccountId = -1;
        NationalIdForChangingPassword = -1;
        InputtedNationalIdToRegister = -1;
        NewPasswordFirstTime = null;
        NewPasswordSecondTime = null;
        InputtedPasswordForLoggingIn = null;
        InputtedPasswordToRegister = null;
        InputtedFirstNameToRegister = null;
        InputtedSecondNameToRegister = null;
    }
}