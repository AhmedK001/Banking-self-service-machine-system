using System.Text.RegularExpressions;

namespace Main;

public class Validator
{
    public static bool IsItName(string? name)
    {
        if (name is null) return false;
        
        string pattern = @"^[a-zA-Z]{2,20}$";

        Regex regexNameFilter = new Regex(pattern);

        return regexNameFilter.IsMatch(name);
    }

    public static bool IsNationalId(int? id)
    {
        if (id is null) return false;
        
        string pattern = "^\\d{8,8}";

        Regex regexNationalIdFilter = new Regex(pattern);

        return regexNationalIdFilter.IsMatch(id.ToString());
    }
    
    public static bool IsIdForCurrentAccount(int? id)
    {
        if (!AttemptsHandler.LetIsIdForCurrentAccount()) return false;

        if (id is null) return false;

        if (TreeManager.SearchMethodArray == null) return false;

        return id == TreeManager.SearchMethodArray.First().NationalId;
    }
    
    public static bool IsCurrentPassword(string? oldPassword)
    {
        if (!AttemptsHandler.LetInputOldPassword()) return false;

        if (oldPassword is null || TreeManager.SearchMethodArray is null) return false;

        return oldPassword.Equals(TreeManager.SearchMethodArray.First().Password);
    }

    public static bool IsPassword(string? password)
    {
        if (password is null) return false;
        
        string pattern = "^[A-Za-z\\d]{8,20}$";

        Regex regexPasswordFilter = new Regex(pattern);

        return regexPasswordFilter.IsMatch(password);
    }
    
    public static bool AreStringsMatches(string? str1, string? str2)
    {
        if (str1 is null || str2 is null) return false;

        return Equals(str2, str1);
    }

    public static bool IsMultipleOf50Or100(double amount)
    {
        if (!((amount % 50 == 0 || amount % 100 == 0) && amount != 0 && amount > 0))
        {
            return false;
        }

        return true;
    }

    public static bool IsLessThan5001(double amount)
    {
        if (amount < 5001)
        {
            return true;
        }

        return false;
    }

    public static bool IsLessThan10001(double amount)
    {
        if (amount < 10001)
        {
            return true;
        }

        return false;
    }

    public static bool IsBiggerThan49(double amount)
    {
        if (amount > 49)
        {
            return true;
        }

        return false;
    }
}