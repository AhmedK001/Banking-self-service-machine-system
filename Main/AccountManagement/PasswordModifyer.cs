namespace Main;

public class PasswordModifyer : User
{
    public static int LimitNewPassword { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitInputForLoginOrRegister { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitIdForChangingPassword { get; set; } = AttemptsHandler.ANSI_CHANCES;
    public static int LimitOldPassword { get; set; } = AttemptsHandler.ANSI_CHANCES;


    public static void ChangePassword()
    {
        User? user = HandleChangePasswordInputs();

        if (user is null)
        {
            ServiceMachine.SemiUi();
            return;
        }

        UpdatePassword(user);
    }

    public static User? HandleChangePasswordInputs()
    {
        Writer.WriteLine("==|* Changing Password *|==","white");

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

        // If passed all, then return national id & new password as instance of User
        return new User(InputsConverter.ToNonNull(nationalId),newPassword);
    }

    private static int? HandleNationalIdInput()
    {
        if (!AttemptsHandler.LetInput()) return null;

        int? userId = InputsHandler.GetInt("Enter your National Id: ", AttemptsHandler.InputsLimit);

        if (userId is null)
        {
            Console.WriteLine(Font.Red(Messenger.InvalidInput(AttemptsHandler.InputsLimit)));
            return HandleNationalIdInput();
        }

        if (!Validator.IsNationalId(userId))
        {
            Console.WriteLine(Font.Red(Messenger.InvalidInput(AttemptsHandler.InputsLimit)));
            return HandleNationalIdInput();
        }

        if (!Validator.IsIdForCurrentAccount(userId))
        {
            Console.WriteLine(Font.Red(Messenger.IdNotForCurrentUser(AttemptsHandler.InputsLimit)));
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
            Console.WriteLine(Font.Red(Messenger.IncorrectPassword(AttemptsHandler.InputsLimit)));
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
            Console.WriteLine(Messenger.InvalidInput(AttemptsHandler.InputsLimit));
            return HandleNewPasswordInput(oldPassword, "first");
        }

        if (userNewPassword.Equals(oldPassword))
        {
            Writer.WriteLine("New password should be different! ","red",AttemptsHandler.InputsLimit);
            return HandleNewPasswordInput(oldPassword, "first");
        }

        AttemptsHandler.ResetLetInput();
        return userNewPassword;
    }

    private static void UpdatePassword(User? user)
    {
        if (user is null) return;

        if (!(Validator.IsNationalId(user.NationalId) || Validator.IsPassword(user.Password))) return;

        // Update password
        TreeManager.SearchMethodArray.First().Password = user.Password;
        // Update the new changes
        TreeManager.UpdateNewChanges();

        Writer.WriteLine("\n\n** Changed your password successfully! **","white");
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