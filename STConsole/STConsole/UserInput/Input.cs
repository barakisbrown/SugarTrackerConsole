using System.Globalization;

namespace STConsole.UserInput;

public class Input
{
    private readonly static string AmountInputString = "Enter Blood Reading: ";
    private readonly static string DateInputString = "Enter the date [MM-dd-yyyy] OR Enter for todays: ";

    public static bool GetYesno()
    {
        ConsoleKeyInfo input = Console.ReadKey(true);
        if (input.Key == ConsoleKey.Y)
            return true;
        else
            return false;
    }

    public static int GetAmount()
    {
        Console.Write(AmountInputString);
        string? result = Console.ReadLine();
        int amount;

        while (string.IsNullOrEmpty(result) || !Int32.TryParse(result, out amount))
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
}
