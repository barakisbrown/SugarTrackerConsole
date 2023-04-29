namespace STConsole.UserInput;

public class Menu
{
    private readonly static string MenuInputString = "Please Select (1-4) OR 0 to exit";

    public static void GetMenu()
    {
        string menu = @"
        
        Main Menu
        What would you like to do?
        
        Type 0 to Close Sugar Tracker App.
        Type 1 to View All Readings.
        Type 2 to Add Reading
        Type 3 to Delete Reading
        Type 4 to Update Reading
        Type 5 to Show Detailed Report
        ----------------------------------
        ";

        Console.WriteLine(menu);
    }

    public static int GetMenuSelection()
    {
        Console.Write(MenuInputString);
        int option;

        while (true)
        {
            ConsoleKeyInfo input = Console.ReadKey(true);
            try
            {
                option = int.Parse(input.KeyChar.ToString());
                return option;
            }
            catch (FormatException _)
            {
                Console.WriteLine();
                Console.WriteLine("Input needs to be between 0 and 4 only. Please try again.");
                Console.Write(MenuInputString);
            }
        }
    }

    public static void Add() { }
    public static void Update() { }
    public static void Delete() { }
    public static void ShowAll() { }
    public static void ShowReport() { }
}
