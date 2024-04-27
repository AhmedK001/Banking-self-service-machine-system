using System.Text.RegularExpressions;

namespace Main;

public class InputsFilter
{
    public static bool IsItName(string? name)
    {
        string pattern = @"^[a-zA-Z]{2,20}$";
        Regex regexNameFilter = new Regex(pattern);
        bool isItName = regexNameFilter.IsMatch(name);

        return isItName;
    }

    public static bool IsItNationalId(int id)
    {
        string pattern = "^\\d{8,8}";
        Regex regexNationalIdFilter = new Regex(pattern);
        bool isItNationalId = regexNationalIdFilter.IsMatch(id.ToString());

        return isItNationalId;
    }

    public static bool IsItPassword(string? password)
    {
        string pattern = "^[A-Za-z\\d]{8,20}$";
        Regex regexPasswordFilter = new Regex(pattern);
        bool isItPassword = regexPasswordFilter.IsMatch(password);

        return isItPassword;
    }

    public static bool IsItMultipleOf50Or100(double amount)
    {
        if (!((amount % 50 == 0 || amount % 100 == 0) && amount != 0 && amount > 0))
        {
            return false;
        }

        return true;
    }

    public static string NotMultipleOf50Or100Message()
    {
        return "Please enter a value multiple of 50 or 100.";
    }

    public static bool IsItLessThan5001(double amount)
    {
        if (amount < 5001)
        {
            return true;
        }

        return false;
    }

    public static string BiggerThan5000Message()
    {
        return "Amount should be equal to or less than 5000$!";
    }

    public static bool IsItLessThan10001(double amount)
    {
        if (amount < 10001)
        {
            return true;
        }

        return false;
    }

    public static string BiggerThan10000Message()
    {
        return "Amount should be less than 10000$";
    }
    
    public static bool IsItBiggerThan49(double amount)
    {
        if (amount > 49)
        {
            return true;
        }

        return false;
    }

    public static string LessThan50Message()
    {
        return "Amount should be equal to or bigger than 50$!";
    }

    public static string InvalidInputMessage()
    {
        return "Input is invalid!";
    }
}