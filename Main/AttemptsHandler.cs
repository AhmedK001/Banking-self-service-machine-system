namespace Main;

public class AttemptsHandler
{
    public static int IncreaseAttempts(int limit)
    {
        limit++;
        return limit;
    }

    public static int ResetAttempts(int limit)
    {
        limit = 0;
        return limit;
    }

    public static bool IsExceededAttempts(int limit, int numberOfAttempts)
    {
        if (limit > numberOfAttempts)
        {
            return true;
        }

        return false;
    }

    public static void HandleExceededAttempts()
    {
        HandleExceededAttemptsMassages();
        ServiceMachine.Exit();
    }

    public static void HandleExceededAttemptsMassages()
    {
        Console.WriteLine(FontStyle.Red("Your chances are out!"));
    }

    public static void ResetLimits()
    {
        ServiceMachine.SemiUiLimit = 0;
        ServiceMachine.MainUiLimit = 0;
        ServiceMachine.DepositeProcessLimit = 0;
        ServiceMachine.WithdrawProcessLimit = 0;
        ServiceMachine.TransferProcessLimit = 0;
        ServiceMachine.IdToTransferLimit = 0;

        UserAuth.LimitLogin = 0;
        UserAuth.LimitPassword = 0;
        UserAuth.LimitRegisterNationId = 0;
        UserAuth.LimitRegisterFirstName = 0;
        UserAuth.LimitRegisterSecondName = 0;
        UserAuth.LimitLoginProcess = 0;

        AccountManager.LimitIdForChangingPassword = 0;
        AccountManager.LimitOldPasswordToChangingPassword = 0;
        AccountManager.LimitNewPasswordForChangingIt = 0;
        AccountManager.LimitInputForLoginOrRegister = 0;
    }
}