using System.Globalization;

namespace STConsole.UserInput;

public static class Input
{
    private readonly static string AmountInputString = "Enter Blood Reading: ";
    private readonly static string DateInputString = "Enter the date [MM-dd-yyyy] OR Enter for todays: ";
    private readonly static string IdInputString = "Select # to Update/Delete or -1 to return back to the main menu?  > ";
    private readonly static string UpdateString = "Update the A)mount or D)ate of this reading? (A/D)";

    public static bool GetYesno()
    {
        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            if (input.Key == ConsoleKey.Y)
                return true;
            else if (input.Key == ConsoleKey.N)
                return false;
        }
    }

    public static int GetAmount()
    {
        Console.Write(AmountInputString);
        string? result = Console.ReadLine();
        int amount;

        while (string.IsNullOrEmpty(result) || !Int32.TryParse(result, out amount) || amount < -1)
        {
            Console.WriteLine("Your answer needs to be a positive interger.");
            Console.Write(AmountInputString);
            result = Console.ReadLine();
        }
        return amount;
    }

    public static DateTime GetDate()
    {
        Console.Write(DateInputString);
        string? result = Console.ReadLine();
        while (true)
        {
            if (result == string.Empty)
                return DateTime.UtcNow;
            try
            {
                DateTime.TryParseExact(result, "MM-dd-yyyy", new CultureInfo("en-us"), DateTimeStyles.None, out DateTime date);
                return date;
            }
            catch (FormatException _)
            {
                Console.WriteLine("Date Result needs to be entered via MM-dd-yyyy");
                Console.WriteLine("Please try again.");
                Console.Write(DateInputString);
                result = Console.ReadLine();
            }
        }
    }

    public static int GetID()
    {
        Console.Write(IdInputString);
        string? result = Console.ReadLine();
        int id;
        while (string.IsNullOrEmpty(result) || !Int32.TryParse(result, out id))
        {
            Console.WriteLine("ID Needs to be number. Please try again");
            result = Console.ReadLine();
        }
        return id;
    }

    public static bool GetAmountOrDate()
    {
        Console.Write(UpdateString);

        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            if (input.Key == ConsoleKey.A)
                return true;
            else if (input.Key == ConsoleKey.D)
                return false;
        }
    }
}
