namespace BankingSelfServiceMachine.UI;

using System.Text.RegularExpressions;

public class InputsFilter
{
    public static bool IsItName(string name)
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

    public static bool IsItPassword(string password)
    {
        string pattern = "^[A-Za-z\\d]{8,20}$";
        Regex regexPasswordFilter = new Regex(pattern);
        bool isItpassword = regexPasswordFilter.IsMatch(password.ToString());

        return isItpassword;
    }

    // public bool Withdraw()
    // {
    //     return true;//
    // }

    public bool IsItMultipleOf50Or100(int amount)
    {
        if (!((amount % 50 == 0 || amount % 100 == 0) && amount != 0 && amount > 0))
        {
            return false;
        }

        return true;
    }

    public string NotMultipleOf50Or100Message()
    {
        return FontStyle.ANSI_RED + FontStyle.BOLD + "Please enter a value that is a multiple of 50 or 100.\n\n" +
               FontStyle.ANSI_RESET;
    }
}