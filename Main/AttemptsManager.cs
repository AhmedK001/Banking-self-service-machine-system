using BankingSelfServiceMachine.UI;

namespace BankingSelfServiceMachine.Managers;

public class AttemptsManager
{
    // Need to return value to value of user manager
    public static int IncreaseAttempts(int limit)
    {
        limit++;
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
        SelfServiceMachine.Exit();
    }

    public static void HandleExceededAttemptsMassages()
    {
        Console.WriteLine(FontStyle.ANSI_RED + FontStyle.BOLD + "Your chances are out!" + FontStyle.ANSI_RESET);
    }
}