namespace Main;

public class Messenger
{
    private static readonly string _Write = "write";
    private static readonly string _WriteLine = "writeLine";

    private static readonly string White = "white";
    private static readonly string Green = "green";
    private static readonly string Red = "red";

    private static string _lowerCaseColor;


    public static void Write(string line, string color)
    {
        _lowerCaseColor = color.ToLower();

        if (_lowerCaseColor.Equals(Green))
        {
            WriteGreen(line, _Write);
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _Write);
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _Write);
        }
    }

    public static void Write(string line, string color, int limit)
    {
        _lowerCaseColor = color.ToLower();

        if (_lowerCaseColor.Equals(Green))
        {
            WriteGreen(line, _Write, limit);
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _Write, limit);
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _Write, limit);
        }
    }

    public static void WriteLine(string line, string color)
    {
        _lowerCaseColor = color.ToLower();

        if (_lowerCaseColor.Equals(Green))
        {
            WriteGreen(line, _WriteLine);
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _WriteLine);
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _WriteLine);
        }
    }

    public static void WriteLine(string line, string color, int limit)
    {
        _lowerCaseColor = color.ToLower();

        if (_lowerCaseColor.Equals(Green))
        {
            WriteGreen(line, _WriteLine, limit);
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _WriteLine, limit);
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _WriteLine, limit);
        }
    }


    private static void WriteWhite(string line, string type)
    {
        if (type.Equals(_Write))
        {
            Console.Write(FontStyle.White(line));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(FontStyle.White(line));
        }
    }

    private static void WriteGreen(string line, string type)
    {
        if (type.Equals(_Write))
        {
            Console.Write(FontStyle.Green(line));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(FontStyle.Green(line));
        }
    }

    private static void WriteRed(string line, string type)
    {
        if (type.Equals(_Write))
        {
            Console.Write(FontStyle.Red(line));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(FontStyle.Red(line));
        }
    }

    private static void WriteRed(string line, string type, int limit)
    {
        if (type.Equals(_Write))
        {
            Console.Write(FontStyle.Red(line+ " " + ValidatorMessenger.DisplayChances(limit)));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(FontStyle.Red(line+ " " + ValidatorMessenger.DisplayChances(limit)));
        }
    }

    private static void WriteGreen(string line, string type, int limit)
    {
        if (type.Equals(_Write))
        {
            Console.Write(FontStyle.Green(line+ " " + ValidatorMessenger.DisplayChances(limit)));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(FontStyle.Green(line+ " " + ValidatorMessenger.DisplayChances(limit)));
        }
    }

    private static void WriteWhite(string line, string type, int limit)
    {
        if (type.Equals(_Write))
        {
            Console.Write(FontStyle.White(line + " " + ValidatorMessenger.DisplayChances(limit)));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(FontStyle.White(line+ " " + ValidatorMessenger.DisplayChances(limit)));
        }
    }
}