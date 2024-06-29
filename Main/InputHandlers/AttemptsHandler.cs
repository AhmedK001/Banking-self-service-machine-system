namespace Main;

public class AttemptsHandler
{
    public static readonly int ANSI_CHANCES = 5; // chances for each limit in the whole system
    public static int GeneralLimit = ANSI_CHANCES;
    public static int InputsLimit = ANSI_CHANCES;


    public static int IncreaseAttempts(int limit)
    {
        return --limit;
    }

    public static int ResetAttempts(int limit)
    {
        limit = ANSI_CHANCES;
        return ANSI_CHANCES;
    }

    public static bool IsExceededAttempts(int limit)
    {
        return limit <= 0;
    }

    public static void HandleExceededAttempts()
    {
        HandleExceededAttemptsMassages();
        ServiceMachine.Exit();
    }

    public static void HandleExceededAttemptsMassages()
    {
        Writer.WriteLine("Your chances are out!","red");
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
        UserAuth.LimitInputId = IncreaseAttempts(UserAuth.LimitInputId);
        if (IsExceededAttempts(UserAuth.LimitInputId))
        {
            HandleExceededAttempts();
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

    public static bool LetIsIdForCurrentAccount()
    {
        PasswordModifyer.LimitIdForChangingPassword = IncreaseAttempts(PasswordModifyer.LimitIdForChangingPassword);
        if (IsExceededAttempts(PasswordModifyer.LimitIdForChangingPassword))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetInputOldPassword()
    {
        PasswordModifyer.LimitOldPassword = IncreaseAttempts(PasswordModifyer.LimitOldPassword);
        if (IsExceededAttempts(PasswordModifyer.LimitOldPassword))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetInputNewPassword()
    {
        PasswordModifyer.LimitNewPassword = IncreaseAttempts(PasswordModifyer.LimitNewPassword);
        if (IsExceededAttempts(PasswordModifyer.LimitNewPassword))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }

    public static bool LetLoginOrRegister()
    {
        PasswordModifyer.LimitInputForLoginOrRegister = IncreaseAttempts(PasswordModifyer.LimitInputForLoginOrRegister);
        if (IsExceededAttempts(PasswordModifyer.LimitInputForLoginOrRegister))
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
    
    public static bool GeneralLet()
    {
        GeneralLimit = IncreaseAttempts(GeneralLimit);
        if (IsExceededAttempts(GeneralLimit))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static void ResetGeneralLet()
    {
        GeneralLimit = ANSI_CHANCES;
    }
    
    public static bool LetInput()
    {
        InputsLimit = IncreaseAttempts(InputsLimit);
        if (IsExceededAttempts(InputsLimit))
        {
            HandleExceededAttempts();
            return false;
        }

        return true;
    }
    
    public static void ResetLetInput()
    {
        InputsLimit = ANSI_CHANCES;
    }

    public static void ResetLimitations()
    {
        // reset class limitations
        ResetLetInput();
        ResetGeneralLet();
    }

    public static void ResetSystemLimitations()
    {
        // reset each file limitations
        ResetLimitations();
        ServiceMachine.ResetLimitations();
        UserAuth.ResetLimitations();
        PasswordModifyer.ResetLimitations();
    }
}