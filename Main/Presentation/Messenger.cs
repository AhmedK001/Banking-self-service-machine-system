namespace Main;

public class ValidatorMessenger
{
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
    
    public static string IncorrectPassword(int chances)
    {
        if (chances - 1 > 1)
        {
            return $"Password is incorrect! {chances - 1} Chances Left.";
        }

        return $"Password is incorrect! {chances - 1} Chance Left.";
    }

    public static string IdNotForCurrentUser(int chances)
    {
        if (chances - 1 > 1)
        {
            return $"Id is not for current account! {chances - 1} Chances Left.";
        }

        return $"Id is not for current account! {chances - 1} Chance Left.";
    }
    
    public static string DisplayChances(int chances)
    {
        if (chances - 1 > 1)
        {
            return $"{chances - 1} Chances Left.";
        }

        return $"{chances - 1} Chance Left.";
    }
}