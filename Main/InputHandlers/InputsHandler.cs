namespace Main;

public class InputsHandler
{
    public static int? GetInt(string notifyMessage,int limit)
    {
        // notify user what to input
        Messenger.Write(notifyMessage,"green");

        int? input = null;

        try
        {
            input = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            ValidatorMessenger.InvalidInput(limit);
        }

        return input;
    }
    
    public static string? GetString(string notifyMessage,int limit)
    {
        // notify user what to input
        Messenger.Write(notifyMessage,"green");
        
        string? input = null;
        try
        {
            input = Console.ReadLine();
        }
        catch (Exception)
        {
            ValidatorMessenger.InvalidInput(limit);
        }

        return input;
    }
}