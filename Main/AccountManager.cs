namespace Main;

public class AccountManager : User
{
    public static int LimitNewPassword { get; set; } = 5;
    public static int LimitInputForLoginOrRegister { get; set; } = 5;
    public static int LimitIdForChangingPassword { get; set; } = 5;
    public static int LimitOldPassword { get; set; } = 5;

    public static void ChangePassword()
    {
        Console.WriteLine(FontStyle.White("=* Here you can change your password *="));
        GetIdForChangingPassword();
    }

    private static void GetIdForChangingPassword()
    {
        if (!AttemptsHandler.LetGetIdForChangingPassword()) return;

        Console.Write(FontStyle.Green("Enter your Account national ID: "));
        try
        {
            UserAuth.NationalIdForChangingPassword = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception)
        {
            Console.WriteLine(FontStyle.Red(InputsFilter.InvalidInput(LimitIdForChangingPassword)));
            GetIdForChangingPassword();
            return;
        }

        if (UserAuth.NationalIdForChangingPassword == TreeManager.SearchMethodArray.First().NationalId)
        {
            InputOldPassword(); // continue
        }
        else
        {
            Console.WriteLine(FontStyle.Red("National IDs do not match!"));
            GetIdForChangingPassword();
        }
    }

    private static void InputOldPassword()
    {
        if (!AttemptsHandler.LetInputOldPassword()) return;

        Console.Write(FontStyle.Green("Enter your old password: "));
        string? oldPassword;
        try
        {
            oldPassword = Console.ReadLine();
        }
        catch (Exception)
        {
            InputOldPassword();
            return;
        }

        if (oldPassword != null && oldPassword.Equals(TreeManager.SearchMethodArray.First().Password))
        {
            InputNewPassword(); // continue
        }
        else
        {
            InputOldPassword();
        }
    }

    private static void InputNewPassword()
    {
        if (!AttemptsHandler.LetInputNewPassword()) return;

        Console.Write(FontStyle.Green("Enter your new password: "));
        try
        {
            UserAuth.NewPasswordFirstTime = Console.ReadLine();
        }
        catch (Exception)
        {
            InputNewPassword();
            return;
        }

        if (UserAuth.NewPasswordFirstTime != null && UserAuth.NewPasswordFirstTime.Length < 8)
        {
            Console.WriteLine(FontStyle.Red("Passwords must consist of more than 7 characters."));
            InputNewPassword(); // Repeat
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
                InputNewPassword();
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

        if (UserAuth.NewPasswordFirstTime != null &&
            UserAuth.NewPasswordFirstTime.Equals(UserAuth.NewPasswordSecondTime))
        {
            ChangeOldPasswordFinalStep(); // Continue
        }
        else
        {
            // Repeat the process
            Console.WriteLine(FontStyle.Red("Does not match!"));
            InputNewPassword();
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