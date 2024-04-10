namespace SSM_IN_C_Sharp_;

public class UserDataManager
{
    // Process helpers
    protected static int InputNationIdForLogin { get; set; }
    private static string InputtedPasswordForLoggingIn { get; set; }
    private static int InputtedNationalIdToRegister { get; set; }
    private static string InputtedPasswordToRegister { get; set; }
    private static string InputtedFirstNameToRegister { get; set; }
    private static string InputtedSecondNameToRegister { get; set; }
    protected static int InputtedIdToTrans { get; set; }
    private static int NationalIDforChangingPassword { get; set; }
    public static string PasswordForChangingPassword { get; set; }
    private static string NewPasswordFirstTime { get; set; }
    private static string NewPasswordSecondTime { get; set; }

    // Constructor data types
    public int NationalID { get; set; }
    public string Password { get; set; }
    public double Balance { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    // Configurations
    private static UserDataManager customer = new();
    private static readonly string BOLD = "\u001B[1m";
    private static readonly string ANSI_RESET = "\u001B[0m";
    private static readonly string ANSI_RED = "\u001B[31m";
    private static readonly string ANSI_BRIGHT_GREEN = "\u001B[92m";
    private static readonly string ANSI_BRIGHT_WHITE = "\u001B[97m";

    // Limitations
    private static bool StoredFromFileAccountsData { get; set; } = false;
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

    private UserDataManager(string firstName,
        string lastName,
        int NationalID,
        string password)
    {
        this.NationalID = NationalID;
        Password = password;
        Balance = 0.0;
        FirstName = firstName;
        LastName = lastName;
    }

    public UserDataManager()
    {
    }

    // --- START OF UPDATING PASSWORD PROCESS ---
    protected static void ChangePassword()
    {
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "==============================" + ANSI_RESET);
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "Here you can change your password" + ANSI_RESET);
        InputIdForChangingPassword();
    }

    private static void InputIdForChangingPassword()
    {
        if (LimitIDForChangingPassword < 3)
        {
            LimitIDForChangingPassword = LimitIDForChangingPassword + 1;
            Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your Account national ID..\n\n" + ANSI_RESET);
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

            if (NationalIDforChangingPassword ==
                UserBinaryTree.SearchMethodArray.First()
                    .NationalID)
            {
                InputOldPasswordForChangingPassword(); // continue
            }
            else
            {
                LimitIDForChangingPassword++;
                Console.WriteLine(ANSI_RED + BOLD + "National ID Number for the Account is wrong." + ANSI_RESET);
                InputIdForChangingPassword();
            }
        }
        else
        {
            Console.WriteLine("Chances are out!!");
            SelfServiceMachine.Exit();
            return;
        }
    }

    private static void InputOldPasswordForChangingPassword()
    {
        if (LimitOldPasswordToChangingPassword < 3)
        {
            LimitOldPasswordToChangingPassword++;

            Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your old password..\n\n" + ANSI_RESET);
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

            if (oldPassword.Equals(UserBinaryTree.SearchMethodArray.First()
                    .Password))
            {
                InputNewPasswordForChangingIt(); // continue
            }
            else
            {
                Console.WriteLine("Invalid password!!");
                InputOldPasswordForChangingPassword();
            }
        }
        else
        {
            Console.WriteLine("Chances are out!!");
            SelfServiceMachine.Exit();
        }
    }

    private static void InputNewPasswordForChangingIt()
    {
        if (LimitNewPasswordForChangingIt < 5)
        {
            LimitNewPasswordForChangingIt++;

            Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your new password..\n\n" + ANSI_RESET);
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
                Console.WriteLine(ANSI_RED +
                                  BOLD +
                                  "Invalid input: Passwords must consist of more than 7 characters." +
                                  ANSI_RESET);
                InputNewPasswordForChangingIt(); // Repeat
            }
            else
            {
                if (!NewPasswordFirstTime.Equals(UserBinaryTree.SearchMethodArray.First()
                        .Password))
                {
                    InputNewPasswordSecondTime(); // Continue
                }
                else
                {
                    Console.WriteLine(ANSI_RED + BOLD + "Cannot use the same old password." + ANSI_RESET);
                    InputNewPasswordForChangingIt();
                }
            }
        }
        else
        {
            Console.WriteLine("Five chances out!!");
            SelfServiceMachine.Exit();
        }
    }

    private static void InputNewPasswordSecondTime()
    {
        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your new password again to confirm..\n\n" + ANSI_RESET);

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
            Console.WriteLine(ANSI_RED + BOLD + "Does not match!!" + ANSI_RESET);
            InputNewPasswordForChangingIt();
        }
    }

    private static void ChangeOldPasswordFinalStep()
    {
        // Update password
        UserBinaryTree.SearchMethodArray.First()
            .Password = NewPasswordSecondTime;
        // Update the new changes
        UserBinaryTree.UpdateNewChanges();
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "\n\nChanged your password successfully!" + ANSI_RESET);
        SelfServiceMachine.SemiUi();
    }
    // --- END OF UPDATING PASSWORD PROCESS ---
    //
    // --- START OF LOADING DATA PROCESSES ---

    protected static void LoadAccountsData()
    {
        if (!StoredFromFileAccountsData)
        {
            StoredFromFileAccountsData = true;
            HandleDataFile(UserBinaryTree.TreeDataFile); // make sure of the data file

            UserBinaryTree.LoadTreeData();
            //Console.WriteLine("-----");
            UserBinaryTree.DisplayTree();
        }
    }

    public static void HandleDataFile(string filePath)
    {
        var file = new FileInfo(filePath);
        if (!file.Exists)
        {
            Directory.CreateDirectory(file.DirectoryName);
            var fileCreated = false;
            try
            {
                using (file.Create())
                {
                    fileCreated = true;
                }
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }

            if (!fileCreated)
                throw new IOException("Failed to create a new file: " + filePath);
        }
    }

    // --- END OF LOADING DATA PROCESSES ---
    //
    // --- START OF CHOCEEING BETWEEN LOGIN OR REGISTER PROCESS ---
    protected static void LoginOrRegister()
    {
        GetInputForLoginOrRegister();
    }

    private static void GetInputForLoginOrRegister()
    {
        LimitInputForLoginOrRegister++;
        if (LimitInputForLoginOrRegister > 3)
            SelfServiceMachine.Exit();

        Console.WriteLine(BOLD + ANSI_BRIGHT_WHITE + "Welcome to our SSM System.");
        Console.WriteLine(ANSI_BRIGHT_GREEN + "Press 1 for Log in to your account.");
        Console.WriteLine("Press 2 Register for a new account.\n" + ANSI_RESET);

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
                Login();
                break;
            case 2:
                Register();
                break;
            default:
                LoginOrRegister();
                break;
        }
    }

    // --- END OF CHOCEEING BETWEEN LOGIN OR REGISTER PROCESS ---
    //
    // --- START OF LOGGING IN PROCESS --
    private static void Login()
    {
        LimitLogin++;
        if (LimitLogin >= 3)
        {
            Console.WriteLine("Chances out!!");
            SelfServiceMachine.Exit();
            return;
        }

        try
        {
            LoadAccountsData();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.StackTrace);
        }

        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "Here you can Login." + ANSI_RESET);
        GetIDInputForLoggingIn(); // continue
    }

    private static void GetIDInputForLoggingIn()
    {
        LimitLoginProcess++;
        if (LimitLoginProcess >= 5)
        {
            Console.WriteLine("Chances out!!");
            SelfServiceMachine.Exit();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your National ID..\n\n" + ANSI_RESET);
        try
        {
            InputNationIdForLogin = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Console.WriteLine("Invalid input!!");
            GetIDInputForLoggingIn();
            return;
        }

        GetPasswordInputForLoggingIn(); // continue
    }

    private static void GetPasswordInputForLoggingIn()
    {
        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your Password..\n\n" + ANSI_RESET);
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
        CheckAccountValidity(InputNationIdForLogin,
            InputtedPasswordForLoggingIn);
    }

    private static void CheckAccountValidity(int inputNationIdForLogin,
        string inputPasswordForLogin)
    {
        UserBinaryTree.SearchOnTree(inputNationIdForLogin);
        // if national id does not exist
        if (UserBinaryTree.SearchMethodArray.Count == 0)
        {
            Console.WriteLine(ANSI_RED + BOLD + "National ID or password is incorrect!\n\n" + ANSI_RESET);
            LimitLoginProcess++; // Chances --;
            GetIDInputForLoggingIn();
            return;
        }

        // if national id and password doesn't match
        if (UserBinaryTree.SearchMethodArray[0].NationalID != inputNationIdForLogin ||
            !UserBinaryTree.SearchMethodArray[0]
                .Password.Equals(inputPasswordForLogin))
        {
            Console.WriteLine(ANSI_RED + BOLD + "National ID or password is incorrect!\n\n" + ANSI_RESET);
            LimitLoginProcess++; // Chances --;
            GetIDInputForLoggingIn();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "\n\nSuccessfully logged in!" + ANSI_RESET);
        SelfServiceMachine.MainUi(); // if passed continue
    }

    // --- END OF LOGGING IN PROCESS ---
    //
    // --- START OF REGISTRATION PROCESS ---
    private static void Register()
    {
        try
        {
            LoadAccountsData();
        }
        catch (Exception e)
        {
            Console.WriteLine("Exception in loading the data: " + e.Message);
        }

        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "Here you can quickly REGISTER in our system." + ANSI_RESET);

        RegisterFirstName(); // Start
    }

    private static void RegisterFirstName()
    {
        LimitRegisterFirstName++;
        if (LimitRegisterFirstName > 5)
        {
            Console.WriteLine("Chances out!!");
            SelfServiceMachine.Exit();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your first name..\n\n" + ANSI_RESET);
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

        if (InputtedFirstNameToRegister.Any(char.IsDigit))
        {
            Console.WriteLine(ANSI_RED + BOLD + "Invalid input!! Please enter letters only!" + ANSI_RESET);
            RegisterFirstName();
            return;
        }

        RegisterSecondName(); // if passed continue
    }

    private static void RegisterSecondName()
    {
        LimitRegisterSecondName++; // Chances --
        if (LimitRegisterSecondName > 5)
        {
            Console.WriteLine("Chances out!!");
            SelfServiceMachine.Exit();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter you second name..\n\n" + ANSI_RESET);
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

        if (InputtedSecondNameToRegister.Any(char.IsDigit))
        {
            Console.WriteLine(ANSI_RED + BOLD + "Invalid input!! Please enter letters only!" + ANSI_RESET);
            RegisterSecondName();
            return;
        }

        RegisterNationID(); // if passed continue
    }

    private static void RegisterNationID()
    {
        LimitRegisterNationID++;
        if (LimitRegisterNationID >= 3)
        {
            Console.WriteLine("Three chances out!!");
            SelfServiceMachine.Exit();
            return;
        }

        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Enter your National ID (exactly 8 digits)..\n\n" + ANSI_RESET);
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

        if (InputtedNationalIdToRegister.ToString()
                .Length !=
            8)
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
            Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "You have an account already! \n" + ANSI_RESET);
            GetInputForLoginOrRegister();
            return;
        }
    }

    private static void RegisterPassword()
    {
        if (LimitPassword < 3)
        {
            LimitPassword++;
            Console.WriteLine(ANSI_BRIGHT_GREEN +
                              BOLD +
                              "Enter Your Password..(Equal to or more than 8 chars.)\n\n" +
                              ANSI_RESET);
            try
            {
                InputtedPasswordToRegister = Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Invalid input!!");
                Console.WriteLine(e.StackTrace);
                RegisterPassword();
                return;
            }

            if (InputtedPasswordToRegister.Length < 8)
            {
                Console.WriteLine("Invalid input!!");
                RegisterPassword();
                return;
            }
        }
        else
        {
            Console.WriteLine("Three chances out!!");
            SelfServiceMachine.Exit();
        }

        CompleteRegistration(); // if passed continue
    }

    private static void CompleteRegistration()
    {
        //if unique ID continue register password
        var userDataManager1 = new UserDataManager(InputtedFirstNameToRegister,
            InputtedSecondNameToRegister,
            InputtedNationalIdToRegister,
            InputtedPasswordToRegister);

        UserBinaryTree.InsertOnTheTree(userDataManager1);
        UserBinaryTree.StoreTreeData();
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "=========================" + ANSI_RESET);
        Console.WriteLine(ANSI_BRIGHT_GREEN + BOLD + "Registered Successfully!!" + ANSI_RESET);
        Console.WriteLine(ANSI_BRIGHT_WHITE + BOLD + "=========================\n\n" + ANSI_RESET);
        Login();
    }
    // --- END OF REGISTRATION PROCESS ---
    //

    public override string ToString()
    {
        return "\n FirstName: " +
               FirstName +
               "\n  LastName: " +
               LastName +
               "\nNationalID: " +
               NationalID +
               "\n   Balance: " +
               Balance +
               "$\n  Password: " +
               Password +
               "\n--------\n";
    }
}