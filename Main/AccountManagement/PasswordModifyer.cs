namespace Main;

public class PasswordModifyer : User
{
    public static int LimitNewPassword { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitInputForLoginOrRegister { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitIdForChangingPassword { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitOldPassword { get; set; } = AttemptsHandler.ANSI_CHANCES;


    public static void ChangePassword()
    {
        List<User>? user = HandleChangePasswordInputs();

        if (user is null)
        {
            ServiceMachine.SemiUi();
            return;
        }

        UpdatePassword(user);
    }

    public static List<User>? HandleChangePasswordInputs()
    {
        Console.WriteLine(FontStyle.White("==|* Changing Password *|=="));

        int? nationalId = HandleNationalIdInput();
        if (nationalId is null) return null;

        string? userOldPassword = HandleOldPasswordInput();
        if (userOldPassword is null) return null;

        string? newPassword = HandleNewPasswordInput(userOldPassword, "first");
        if (newPassword is null) return null;

        string? reNewPassword = HandleNewPasswordInput(userOldPassword, "second");
        if (reNewPassword is null) return null;

        // Check if the new password first and second chance matches
        if (!Validator.AreStringsMatches(newPassword, reNewPassword)) return null;

        // If passed all then return national id & new password
        return InputsConverter.ToNonNullable(nationalId, newPassword);
    }

    private static int? HandleNationalIdInput()
    {
        if (!AttemptsHandler.LetInput()) return null;

        int? userId = InputsHandler.GetInt("Enter your National Id: ", AttemptsHandler.InputsLimit);

        if (userId is null)
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(AttemptsHandler.InputsLimit)));
            return HandleNationalIdInput();
        }

        if (!Validator.IsNationalId(userId))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.InvalidInput(AttemptsHandler.InputsLimit)));
            return HandleNationalIdInput();
        }

        if (!Validator.IsIdForCurrentAccount(userId))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.IdNotForCurrentUser(AttemptsHandler.InputsLimit)));
            return HandleNationalIdInput();
        }

        AttemptsHandler.ResetLetInput();
        return userId;
    }

    private static string? HandleOldPasswordInput()
    {
        if (!AttemptsHandler.LetInput()) return null;

        string? userOldPassword = InputsHandler.GetString("Enter your current password: ", AttemptsHandler.InputsLimit);
        if (!Validator.IsCurrentPassword(userOldPassword))
        {
            Console.WriteLine(FontStyle.Red(ValidatorMessenger.IncorrectPassword(AttemptsHandler.InputsLimit)));
            return HandleOldPasswordInput();
        }

        AttemptsHandler.ResetLetInput();
        return userOldPassword;
    }

    private static string? HandleNewPasswordInput(string oldPassword, string type)
    {
        if (!AttemptsHandler.LetInput()) return null;
        string? userNewPassword = null;
        if (type.Equals("first"))
        {
            userNewPassword = InputsHandler.GetString("Enter your new password: ", AttemptsHandler.InputsLimit);
        }

        if (type.Equals("second"))
        {
            userNewPassword = InputsHandler.GetString("Re enter your new password: ", AttemptsHandler.InputsLimit);
        }

        if (!Validator.IsPassword(userNewPassword))
        {
            Console.WriteLine(ValidatorMessenger.InvalidInput(AttemptsHandler.InputsLimit));
            return HandleNewPasswordInput(oldPassword, "first");
        }

        if (userNewPassword.Equals(oldPassword))
        {
            Console.WriteLine(FontStyle.Red("New password should be different! " +
                                            ValidatorMessenger.DisplayChances(AttemptsHandler.InputsLimit)));
            return HandleNewPasswordInput(oldPassword, "first");
        }

        AttemptsHandler.ResetLetInput();
        return userNewPassword;
    }

    private static void UpdatePassword(List<User> user)
    {
        if (!user.Any()) return;

        if (!(Validator.IsNationalId(user[0].NationalId) || Validator.IsPassword(user[0].Password))) return;

        // Update password
        TreeManager.SearchMethodArray.First().Password = user[0].Password;
        // Update the new changes
        TreeManager.UpdateNewChanges();

        Console.WriteLine(FontStyle.White("\n\n** Changed your password successfully! **"));
        ServiceMachine.SemiUi();
    }

    public static void ResetLimitations()
    {
        LimitIdForChangingPassword = AttemptsHandler.ANSI_CHANCES;
        LimitOldPassword = AttemptsHandler.ANSI_CHANCES;
        LimitNewPassword = AttemptsHandler.ANSI_CHANCES;
        LimitInputForLoginOrRegister = AttemptsHandler.ANSI_CHANCES;
    }
}