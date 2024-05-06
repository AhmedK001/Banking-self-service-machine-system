namespace Main;

public class AttemptsHandler
{
    public static int IncreaseAttempts(int limit)
    {
        limit--;
        return limit;
    }

    public static int ResetAttempts(int limit)
    {
        limit = 6;
        return limit;
    }

    public static bool IsExceededAttempts(int limit)
    {
        if (limit <= 0)
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

    public static bool LetLogin()
    {
        UserAuth.LimitLogin = IncreaseAttempts(UserAuth.LimitLogin);
        if (IsExceededAttempts(UserAuth.LimitLogin))
        {
            HandleExceededAttempts();
            return false;
        }

        if (IsExceededAttempts(UserAuth.LimitInputId) || IsExceededAttempts(UserAuth.LimitPasswordProcess))
        {
            ServiceMachine.Exit();
            return false;
        }

        return true;
    }

    public static bool LetInputId()
    {
        UserAuth.LimitInputId = AttemptsHandler.IncreaseAttempts(UserAuth.LimitInputId);
        if (AttemptsHandler.IsExceededAttempts(UserAuth.LimitInputId))
        {
            AttemptsHandler.HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetGetPasswordToLogin()
    {
        UserAuth.LimitPasswordProcess = IncreaseAttempts(UserAuth.LimitPasswordProcess);
        if (IsExceededAttempts(UserAuth.LimitPasswordProcess))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetHandleFirstName()
    {
        UserAuth.LimitRegisterFirstName = IncreaseAttempts(UserAuth.LimitRegisterFirstName);
        if (IsExceededAttempts(UserAuth.LimitRegisterFirstName))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetHandleSecondName()
    {
        UserAuth.LimitRegisterSecondName = IncreaseAttempts(UserAuth.LimitRegisterSecondName);
        if (IsExceededAttempts(UserAuth.LimitRegisterSecondName))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetHandleNationalId()
    {
        UserAuth.LimitRegisterNationId = IncreaseAttempts(UserAuth.LimitRegisterNationId);
        if (IsExceededAttempts(UserAuth.LimitRegisterNationId))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetHandlePassword()
    {
        UserAuth.LimitPassword = IncreaseAttempts(UserAuth.LimitPassword);
        if (IsExceededAttempts(UserAuth.LimitPassword))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetGetIdForChangingPassword()
    {
        AccountManager.LimitIdForChangingPassword = IncreaseAttempts(AccountManager.LimitIdForChangingPassword);
        if (IsExceededAttempts(AccountManager.LimitIdForChangingPassword))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetInputOldPassword()
    {
        AccountManager.LimitOldPassword = IncreaseAttempts(AccountManager.LimitOldPassword);
        if (IsExceededAttempts(AccountManager.LimitOldPassword))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetInputNewPassword()
    {
        AccountManager.LimitNewPassword = IncreaseAttempts(AccountManager.LimitNewPassword);
        if (IsExceededAttempts(AccountManager.LimitNewPassword))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetLoginOrRegister()
    {
        AccountManager.LimitInputForLoginOrRegister = IncreaseAttempts(AccountManager.LimitInputForLoginOrRegister);
        if (IsExceededAttempts(AccountManager.LimitInputForLoginOrRegister))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static bool LetMainUi()
    {
        ServiceMachine.LimitMainUi = IncreaseAttempts(ServiceMachine.LimitMainUi);
        if (IsExceededAttempts(ServiceMachine.LimitMainUi))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static bool LetSemiUi()
    {
        ServiceMachine.LimitSemiUi = IncreaseAttempts(ServiceMachine.LimitSemiUi);
        if (IsExceededAttempts(ServiceMachine.LimitSemiUi))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static bool LetTransferMoney()
    {
        ServiceMachine.LimitTransferProcess = IncreaseAttempts(ServiceMachine.LimitTransferProcess);
        if (IsExceededAttempts(ServiceMachine.LimitTransferProcess))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static bool LetGetIdToTransfer()
    {
        ServiceMachine.LimitIdToTransfer = IncreaseAttempts(ServiceMachine.LimitIdToTransfer);
        if (IsExceededAttempts(ServiceMachine.LimitIdToTransfer))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static bool LetGetAmountToTransfer()
    {
        ServiceMachine.LimitValueToTrans = IncreaseAttempts(ServiceMachine.LimitValueToTrans);
        if (IsExceededAttempts(ServiceMachine.LimitValueToTrans))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static bool LetWithdraw()
    {
        ServiceMachine.LimitWithdrawProcess = IncreaseAttempts(ServiceMachine.LimitWithdrawProcess);
        if (IsExceededAttempts(ServiceMachine.LimitWithdrawProcess))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static bool LetStatements()
    {
        ServiceMachine.LimitStatementProcess = IncreaseAttempts(ServiceMachine.LimitStatementProcess);
        if (IsExceededAttempts(ServiceMachine.LimitStatementProcess))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static void ResetLimitations()
    {
        ServiceMachine.LimitSemiUi = 6;
        ServiceMachine.LimitMainUi = 6;
        ServiceMachine.LimitDepositeProcess = 6;
        ServiceMachine.LimitWithdrawProcess = 6;
        ServiceMachine.LimitTransferProcess = 6;
        ServiceMachine.LimitIdToTransfer = 6;
        ServiceMachine.LimitStatementProcess = 6;
        ServiceMachine.LimitValueToTrans = 6;
        
        UserAuth.IsPassedOneTime = false;
        UserAuth.LimitLogin = 6;
        UserAuth.LimitPassword = 6;
        UserAuth.LimitRegisterNationId = 6;
        UserAuth.LimitRegisterFirstName = 6;
        UserAuth.LimitRegisterSecondName = 6;
        UserAuth.LimitInputId = 6;
        UserAuth.LimitPasswordProcess = 6;

        AccountManager.LimitIdForChangingPassword = 6;
        AccountManager.LimitOldPassword = 6;
        AccountManager.LimitNewPassword = 6;
        AccountManager.LimitInputForLoginOrRegister = 6;
    }
}