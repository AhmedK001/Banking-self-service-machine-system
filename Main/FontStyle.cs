namespace Main;

public class FontStyle
{
    private const string Bold = "\u001B[1m";
    private const string Reset = "\u001B[0m";
    private const string BrightRed = "\u001B[31m";
    private const string BrightGreen = "\u001B[92m";
    private const string BrightWhite = "\u001B[97m";

    public static string SpaceLine()
    {
        return White("*======================*");
    }

    public static string White(string text)
    {
        return BrightWhite + Bold + text + Reset;
    }

    public static string Red(string text)
    {
        return BrightRed + Bold + text + Reset;
    }

    public static string Green(string text)
    {
        return BrightGreen + Bold + text + Reset;
    }
}