namespace Main;

public class InputsHandler
{
    public static int? GetInt(string notifyMessage,int limit)
    {
        // notify user what to input
        Writer.Write(notifyMessage,"green");

        int? input = null;

        try
        {
            input = Convert.ToInt32(Console.ReadLine());
        }
        catch (Exception e)
        {
            Messenger.InvalidInput(limit);
        }

        return input;
    }
    
    public static string? GetString(string notifyMessage,int limit)
    {
        // notify user what to input
        Writer.Write(notifyMessage,"green");
        
        string? input = null;
        try
        {
            input = Console.ReadLine();
        }
        catch (Exception)
        {
            Messenger.InvalidInput(limit);
        }

        return input;
    }
}