namespace Main;

public class Writer
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
            return;
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _Write);
            return;
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _Write);
            return;
        }

        if (_lowerCaseColor.Any()) // if random color input, write with white color
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
            return;
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _Write, limit);
            return;
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _Write, limit);
            return;
        }

        if (_lowerCaseColor.Any()) // if random color input, write with white color
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
            return;
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _WriteLine);
            return;
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _WriteLine);
            return;
        }

        if (_lowerCaseColor.Any()) // if random color input, write with white color
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
            return;
        }

        if (_lowerCaseColor.Equals(Red))
        {
            WriteRed(line, _WriteLine, limit);
            return;
        }

        if (_lowerCaseColor.Equals(White))
        {
            WriteWhite(line, _WriteLine, limit);
            return;
        }

        if (_lowerCaseColor.Any()) // if random color input, write with white color
        {
            WriteWhite(line, _WriteLine, limit);
        }
    }


    private static void WriteWhite(string line, string type)
    {
        if (type.Equals(_Write))
        {
            Console.Write(Font.White(line));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(Font.White(line));
        }
    }

    private static void WriteGreen(string line, string type)
    {
        if (type.Equals(_Write))
        {
            Console.Write(Font.Green(line));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(Font.Green(line));
        }
    }

    private static void WriteRed(string line, string type)
    {
        if (type.Equals(_Write))
        {
            Console.Write(Font.Red(line));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(Font.Red(line));
        }
    }

    private static void WriteRed(string line, string type, int limit)
    {
        if (type.Equals(_Write))
        {
            Console.Write(Font.Red(line + " " + Messenger.DisplayChances(limit)));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(Font.Red(line + " " + Messenger.DisplayChances(limit)));
        }
    }

    private static void WriteGreen(string line, string type, int limit)
    {
        if (type.Equals(_Write))
        {
            Console.Write(Font.Green(line + " " + Messenger.DisplayChances(limit)));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(Font.Green(line + " " + Messenger.DisplayChances(limit)));
        }
    }

    private static void WriteWhite(string line, string type, int limit)
    {
        if (type.Equals(_Write))
        {
            Console.Write(Font.White(line + " " + Messenger.DisplayChances(limit)));
        }

        if (type.Equals(_WriteLine))
        {
            Console.WriteLine(Font.White(line + " " + Messenger.DisplayChances(limit)));
        }
    }
}