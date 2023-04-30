using Serilog;
using STConsole.DataLayer;
using STConsole.Model;

namespace STConsole.UserInput;

public class Menu
{
    private readonly static string MenuInputString = "Please Select (1-5) OR 0 to exit";

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
                Console.WriteLine("Input needs to be between 0 and 5 only. Please try again.");
                Console.Write(MenuInputString);
            }
        }
    }

    public static void Add() 
    {        
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Adding a new blood sugar reading.");
            int amount = Input.GetAmount();
            DateTime date = Input.GetDate();
            Log.Debug("Blood Sugar Amount => {0}", amount);
            Log.Debug("Date Added => {date}", date.ToShortDateString());
            Reading reading = new() { Amount = amount, Added = date };
            if (ReadingController.Insert(reading))
            {
                Console.WriteLine("Blood Sugar Reading has been successfully Added.");
                Log.Debug("Row was inserted successfully");
            }
            else
            {
                Console.WriteLine("Blood Suagr was not successfully not added.");
                Log.Error("Something happened where the log could not been inserted");
            }

            Console.WriteLine("Do you wish to add another? (Y/N)");
            if (!Input.GetYesno())
                break;
        }
        Thread.Sleep(1000);
        Console.Clear();
    }
    public static void Update() { }
    public static void Delete() { }
    public static void ShowAll() { }
    public static void ShowReport() { }
}
