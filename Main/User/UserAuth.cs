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

    public static int LimitLogin { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitInputId { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitPassword { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitPasswordProcess { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitRegisterNationId { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitRegisterFirstName { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitRegisterSecondName { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static bool IsPassedOneTime { set; get; }

    public static void Login()
    {
        if (!AttemptsHandler.LetLogin()) return;

        DataHandler.LoadAccountsData();

        if (!IsPassedOneTime)
        {
            IsPassedOneTime = true;
            Console.WriteLine(FontStyle.White("\n====* Here you can Login. *===="));
        }

        if (HandleLoginInputs() is null)
        {
            Login();
            return;
        }

        IsPassedOneTime = false;
        DisplaySuccessLogin();
        ServiceMachine.MainUi();
    }

    private static List<User>? HandleLoginInputs()
    {
        int? userId = GetNationalId();
        if (userId == null) return null;

        string? userPassword = GetPassword("normal");
        if (userPassword == null) return null;

        bool? checkValidityResult = CheckAccountValidity(userId, userPassword);
        if (checkValidityResult is null or false) return null;

        return InputsConverter.ToNonNullable(userId, userPassword);
    }

    public static int? GetNationalId()
    {
        if (!AttemptsHandler.LetInputId()) return null;

        Console.Write(FontStyle.Green("Enter your National ID: "));
        try
        {
            UserAccountId = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(LimitInputId)));
            return GetNationalId();
        }

        LimitInputId = AttemptsHandler.ResetAttempts(LimitInputId);
        return UserAccountId;
    }

    public static string? GetPassword(string type)
    {
        if (!AttemptsHandler.LetGetPasswordToLogin()) return null;

        if (type == "normal") Console.Write(FontStyle.Green("Enter your Password: "));
        if (!type.Equals("normal")) Console.Write(FontStyle.Green($"Enter your {type} Password: "));
        
        try
        {
            InputtedPasswordForLoggingIn = Console.ReadLine();
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(LimitPasswordProcess)));
            return GetPassword(type);
        }

        LimitPasswordProcess = AttemptsHandler.ResetAttempts(LimitPasswordProcess);
        return InputtedPasswordForLoggingIn;
    }

    private static bool? CheckAccountValidity(int? inputNationIdForLogin, string? inputPasswordForLogin)
    {
        TreeManager.SearchOnTree(inputNationIdForLogin ?? -1);
        // if national id does not exist
        if (TreeManager.SearchMethodArray.Count == 0)
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.IncorrectInput(LimitInputId)));
            AttemptsHandler.IncreaseAttempts(LimitInputId); // Chances --;
            // GetIdInputForLoggingIn();
            return false;
        }

        // if national id and password doesn't match
        var password = TreeManager.SearchMethodArray[0].Password;
        if (password != null && (TreeManager.SearchMethodArray[0].NationalId != inputNationIdForLogin ||
                                 !password.Equals(inputPasswordForLogin)))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.IncorrectInput(LimitInputId)));
            AttemptsHandler.IncreaseAttempts(LimitInputId); // Chances --;
            // GetIdInputForLoggingIn();
            return false;
        }

        return true;
    }

    private static void DisplaySuccessLogin()
    {
        //Console.Clear();
        Console.Write(FontStyle.Red("\n==> "));
        Console.Write(FontStyle.Green("Successfully logged in!"));
        Console.WriteLine(FontStyle.Red(" <=="));
    } // --- END OF LOGGING IN PROCESS ---

    // --- START OF REGISTRATION PROCESS ---
    public static void Register()
    {
        DataHandler.LoadAccountsData();

        Console.WriteLine(FontStyle.White("====* Registration *===="));

        // if newUser instance != null completeRegistration
        User? newUser = HandelUserInfo();

        if (newUser is null) return;

        CompleteRegistration(newUser);
        DisplaySuccessRegister();
        Login();
    }

    private static User? HandelUserInfo()
    {
        // get firstName, if any goes wrong it returns null
        string? firstName = HandleFirstName();
        if (firstName is null) return null;

        // get secondName, if any goes wrong it returns null
        string? secondName = HandleSecondName();
        if (secondName is null) return null;

        // get nationalId, if any goes wrong it returns null
        int? nationalId = HandleNationalId();
        if (nationalId is null) return null;

        int nationalIdNonNullable = (int)nationalId;
        // get password, if any goes wrong it returns null
        string? password = HandlePassword();
        if (password is null) return null;

        // return newUser instance
        return new User(firstName, secondName, nationalIdNonNullable, password);
    }

    private static string? HandleFirstName()
    {
        if (!AttemptsHandler.LetHandleFirstName()) return null;

        // Fix old name repeats without return;
        string? firstName = GetName("first", LimitRegisterFirstName);
        if (!Validator.IsItName(firstName))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(LimitRegisterFirstName)));
            return HandleFirstName();
        }

        return firstName;
    }

    private static string? HandleSecondName()
    {
        if (!AttemptsHandler.LetHandleSecondName()) return null;

        string? secondName = GetName("second", LimitRegisterSecondName);

        if (!Validator.IsItName(secondName))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(LimitRegisterSecondName)));
            return HandleSecondName();
        }

        return secondName;
    }

    private static string? GetName(string nameType, int limit)
    {
        Console.Write(FontStyle.Green($"Enter your {nameType} name: "));

        string? name;
        try
        {
            name = Console.ReadLine();
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(limit)));
            return null;
        }

        return name;
    }

    private static int? HandleNationalId()
    {
        if (!AttemptsHandler.LetHandleNationalId()) return null;

        int? nationalId = GetNationalId(LimitRegisterNationId);

        if (nationalId is null)
        {
            return HandleNationalId();
        }

        int nationalIdNotNull = (int)nationalId;
        if (!Validator.IsNationalId(nationalIdNotNull))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(LimitRegisterNationId)));
            return HandleNationalId();
        }

        if (TreeManager.IsUsedId(nationalIdNotNull))
        {
            HandleIfUsedNationalId();
            return null;
        }

        return nationalIdNotNull;
    }

    private static int? GetNationalId(int limit)
    {
        Console.Write(FontStyle.Green("Enter your National ID (exactly 8 digits): "));

        try
        {
            InputtedNationalIdToRegister = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(limit)));
            return null;
        }

        return InputtedNationalIdToRegister;
    }

    private static void HandleIfUsedNationalId()
    {
        Console.Clear();
        Console.WriteLine(FontStyle.Red("\nEach user can has only one account.!"));
        Thread.Sleep(3000);

        Console.Clear();
        AttemptsHandler.ResetSystemLimitations();
        ServiceMachine.LoginOrRegister();
    }

    private static string? HandlePassword()
    {
        if (!AttemptsHandler.LetHandlePassword()) return null;

        string? password = GetPasswordToRegister();
        if (!Validator.IsPassword(password))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(LimitPassword)));
            HandlePassword();
            return null;
        }

        return password;
    }

    private static string? GetPasswordToRegister()
    {
        Console.Write(FontStyle.Green("Enter Your Password..(Equal to or more than 8 chars.): "));
        try
        {
            InputtedPasswordToRegister = Console.ReadLine();
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(LimitPassword)));
            return null;
        }

        return InputtedPasswordToRegister;
    }

    private static void CompleteRegistration(User? newUser)
    {
        // insert user's data to the structure then store it.
        if (newUser is null) return;

        TreeManager.InsertOnTheTree(newUser);

        TreeManager.StoreTreeData();
    }

    private static void DisplaySuccessRegister()
    {
        Console.WriteLine(FontStyle.SpaceLine());
        Console.WriteLine(FontStyle.Green("=* Registered Successfully!! *="));
        Console.WriteLine(FontStyle.SpaceLine() + "\n\n");
    }

    public static void ResetLimitations()
    {
        IsPassedOneTime = false;
        LimitLogin = AttemptsHandler.ANSI_CHANCES;
        LimitPassword = AttemptsHandler.ANSI_CHANCES;
        LimitRegisterNationId = AttemptsHandler.ANSI_CHANCES;
        LimitRegisterFirstName = AttemptsHandler.ANSI_CHANCES;
        LimitRegisterSecondName = AttemptsHandler.ANSI_CHANCES;
        LimitInputId = AttemptsHandler.ANSI_CHANCES;
        LimitPasswordProcess = AttemptsHandler.ANSI_CHANCES;
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