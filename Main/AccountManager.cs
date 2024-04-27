namespace Main;

public class AccountManager : User
{
    public static int LimitNewPasswordForChangingIt { get; set; }
    public static int LimitInputForLoginOrRegister { get; set; }
    public static int LimitIdForChangingPassword { get; set; }
    public static int LimitOldPasswordToChangingPassword { get; set; }
    
    public static void ChangePassword()
    {
        Console.WriteLine(FontStyle.White("=* Here you can change your password *="));
        InputIdForChangingPassword();
    }

    private static void InputIdForChangingPassword()
    {
        LimitIdForChangingPassword = AttemptsHandler.IncreaseAttempts(LimitIdForChangingPassword);
        if (AttemptsHandler.IsExceededAttempts(LimitIdForChangingPassword, 3))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.Write(FontStyle.Green("Enter your Account national ID: "));
        try
        {
            UserAuth.NationalIdForChangingPassword = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine("Invalid input!!");
            InputIdForChangingPassword();
            return;
        }

        if (UserAuth.NationalIdForChangingPassword == TreeManager.SearchMethodArray.First().NationalId)
        {
            InputOldPasswordForChangingPassword(); // continue
        }
        else
        {
            Console.WriteLine(FontStyle.Red("National IDs do not match!"));
            InputIdForChangingPassword();
        }
    }

    private static void InputOldPasswordForChangingPassword()
    {
        LimitOldPasswordToChangingPassword = AttemptsHandler.IncreaseAttempts(LimitOldPasswordToChangingPassword);
        if (AttemptsHandler.IsExceededAttempts(LimitOldPasswordToChangingPassword, 3))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.Write(FontStyle.Green("Enter your old password: "));
        string? oldPassword;
        try
        {
            oldPassword = Console.ReadLine();
        }
        catch (Exception)
        {
            InputOldPasswordForChangingPassword();
            return;
        }

        if (oldPassword != null && oldPassword.Equals(TreeManager.SearchMethodArray.First().Password))
        {
            InputNewPasswordForChangingIt(); // continue
        }
        else
        {
            InputOldPasswordForChangingPassword();
        }
    }

    private static void InputNewPasswordForChangingIt()
    {
        LimitNewPasswordForChangingIt = AttemptsHandler.IncreaseAttempts(LimitNewPasswordForChangingIt);
        if (AttemptsHandler.IsExceededAttempts(LimitNewPasswordForChangingIt, 3))
        {
            AttemptsHandler.HandleExceededAttempts();
            return;
        }

        Console.Write(FontStyle.Green("Enter your new password: "));
        try
        {
            UserAuth.NewPasswordFirstTime = Console.ReadLine();
        }
        catch (Exception)
        {
            InputNewPasswordForChangingIt();
            return;
        }

        if (UserAuth.NewPasswordFirstTime != null && UserAuth.NewPasswordFirstTime.Length < 8)
        {
            Console.WriteLine(FontStyle.Red("Passwords must consist of more than 7 characters."));
            InputNewPasswordForChangingIt(); // Repeat
        }
        else
        {
            if (UserAuth.NewPasswordFirstTime != null &&
                !UserAuth.NewPasswordFirstTime.Equals(TreeManager.SearchMethodArray.First().Password))
            {
                InputNewPasswordSecondTime(); // Continue
            }
            else
            {
                Console.WriteLine(FontStyle.Red("Cannot use the same old password."));
                InputNewPasswordForChangingIt();
            }
        }
    }

    private static void InputNewPasswordSecondTime()
    {
        Console.Write(FontStyle.Green("Enter your new password again to confirm: "));

        try
        {
            UserAuth.NewPasswordSecondTime = Console.ReadLine();
        }
        catch (Exception)
        {
            InputNewPasswordSecondTime(); // continue
            return;
        }

        if (UserAuth.NewPasswordFirstTime != null && UserAuth.NewPasswordFirstTime.Equals(UserAuth.NewPasswordSecondTime))
        {
            ChangeOldPasswordFinalStep(); // Continue
        }
        else
        {
            // Repeat the process
            Console.WriteLine(FontStyle.Red("Does not match!"));
            InputNewPasswordForChangingIt();
        }
    }

    private static void ChangeOldPasswordFinalStep()
    {
        // Update password
        TreeManager.SearchMethodArray.First().Password = UserAuth.NewPasswordSecondTime;
        // Update the new changes
        TreeManager.UpdateNewChanges();

        Console.WriteLine(FontStyle.White("\n\n** Changed your password successfully! **"));
        ServiceMachine.SemiUi();
    }
}