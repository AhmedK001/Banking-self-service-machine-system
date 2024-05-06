using System.Text.RegularExpressions;

namespace Main;

public class InputsFilter
{
    public static bool IsItName(string? name)
    {
        if (name is null) return false;
        
        string pattern = @"^[a-zA-Z]{2,20}$";

        Regex regexNameFilter = new Regex(pattern);

        return regexNameFilter.IsMatch(name);
    }

    public static bool IsItNationalId(int? id)
    {
        if (id is null) return false;
        
        string pattern = "^\\d{8,8}";

        Regex regexNationalIdFilter = new Regex(pattern);

        return regexNationalIdFilter.IsMatch(id.ToString());
    }

    public static bool IsItPassword(string? password)
    {
        if (password is null) return false;
        
        string pattern = "^[A-Za-z\\d]{8,20}$";

        Regex regexPasswordFilter = new Regex(pattern);

        return regexPasswordFilter.IsMatch(password);
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

    public static string LessThan50()
    {
        return "Amount should be equal to or bigger than 50$!";
    }

    public static string NotMultipleOf50Or100()
    {
        return "Please enter a value multiple of 50 or 100.";
    }

    public static string BiggerThan10000()
    {
        return "Amount should be less than 10000$";
    }

    public static string BiggerThan5000()
    {
        return "Amount should be equal to or less than 5000$!";
    }

    public static string InvalidOption(int chances)
    {
        if (chances - 1 > 1)
        {
            return $"Invalid option. {chances - 1} Chances Left.";
        }

        return $"Invalid option. {chances - 1} Chance Left.";
    }

    public static string InvalidInput(int chances)
    {
        if (chances - 1 > 1)
        {
            return $"Invalid Input. {chances - 1} Chances Left.";
        }

        return $"Invalid Input. {chances - 1} Chance Left.";
    }

    public static string IncorrectInput(int chances)
    {
        if (chances - 1 > 1)
        {
            return $"National ID or Password is incorrect! {chances - 1} Chances Left.";
        }

        return $"National ID or Password is incorrect! {chances - 1} Chance Left.";
    }
}