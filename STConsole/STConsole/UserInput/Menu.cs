﻿using ConsoleTableExt;
using CsvHelper;
using Serilog;
using STConsole.DataLayer;
using STConsole.Model;
using System.Globalization;

namespace STConsole.UserInput;

public static class Menu
{
    private readonly static string MenuInputString = "\tPlease Select (1-7)OR 0 to exit. :> ";

    public static void GetMenu()
    {
        Console.WriteLine("Welcome to Sugar Tracker.  A blood sugar tracking application.");
        string menu = @"
        
        Main Menu
        What would you like to do?
        
        Type 0 to Close Sugar Tracker App.
        Type 1 to View All Readings.
        Type 2 to Add Reading
        Type 3 to Delete Reading
        Type 4 to Update Reading
        Type 5 to Show Lifetime Report
        Type 6 to Show 30/60/90 Day Report
        Type 7 to write reading to a csv file
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
                Console.WriteLine("Input needs to be between 0 and 7 only. Please try again.");
                Console.Write(MenuInputString);
            }
        }
    }

    public static void Add()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Adding a new blood sugar reading(-1 to exit).");
            int amount = Input.GetAmount();
            if (amount != -1)          
            {
                DateOnly date = DateOnly.FromDateTime(Input.GetDate());
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
                if (Input.GetYesno())
                    continue;
                else
                    break;
            }
            break;
        }
        Console.WriteLine("\nReturning back to the main menu.");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public static void Update()
    {       
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Updating Information either by the Amount or Date Added.");
            ReadingController.DisplayAllRecords();
            Console.WriteLine();
            int id = Input.GetID();
            if (id != -1)
            {
                var sel = ReadingController.Query(id);
                if (sel is null)
                {
                    Console.WriteLine("Please try again. Select an # from the list.");
                    continue;
                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("The row that we are updating is => {0}", sel);
                    bool choice = Input.GetAmountOrDate();
                    Console.WriteLine();
                    // TRUE IS AMOUNT / FALSE IS DATE
                    if (choice)
                    {
                        int updatedAmount = Input.GetAmount();
                        Console.WriteLine($"Do you wish to change the Old Amount = {sel.Amount} with New Amount = {updatedAmount} (Y/N)?");
                        if (Input.GetYesno())
                        {
                            Reading updateReading = new() { Id = sel.Id, Amount = updatedAmount, Added = sel.Added };
                            if (ReadingController.UpdateAmount(updateReading))
                            {
                                Console.WriteLine("Amount has been updated.");
                            }
                        }
                    }
                    else
                    {
                        DateOnly updatedDate = DateOnly.FromDateTime(Input.GetDate());
                        Console.WriteLine($"Do you want to change the old date {sel.Added} with {updatedDate} (Y/N)");
                        if (Input.GetYesno())
                        {
                            Reading updateReading = new() { Id = sel.Id, Amount = sel.Amount, Added = updatedDate };
                            if (ReadingController.UpateDate(updateReading))
                            {
                                Console.WriteLine("Date has been updated.");
                            }
                        }
                    }
                }
                Console.WriteLine("Do you wish to change anything else (Y/N)?");
                if (Input.GetYesno())
                    continue;
                else
                    break;
            }
            break;
        }
        Console.WriteLine("\nReturning back to the main menu.");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public static void Delete()
    {
        Console.Clear();
        Console.WriteLine("Deleting a Row");
        ReadingController.DisplayAllRecords();
        Console.WriteLine();
        while (true)
        {
            int id = Input.GetID();
            if (id != -1)
            {
                var sel = ReadingController.Query(id);
                if (sel is null)
                {
                    Console.WriteLine("Please try again. Select an # from the list.");
                    continue;
                }
                Console.WriteLine($"Do you wish to delete the following: {sel}  (Y/N)");
                if (Input.GetYesno())
                {
                    if (ReadingController.Delete(sel))
                    {
                        Console.WriteLine("Row was removed successfully.");
                        Log.Information("Row {id} was deleted", sel.Id);
                    }
                }
                else
                {
                    Console.WriteLine("No removal was done.");
                    continue;
                }
            }
            break;
        }
        Console.WriteLine("\nReturning back to the main menu.");
        Thread.Sleep(1000);
        Console.Clear();
    }

    public static void ShowAll()
    {
        Console.Clear();
        Console.WriteLine("VIEWING ALL BLOOD RESULTS\n");

        if (ReadingController.IsEmpty())        
            Console.WriteLine("No reading entered. Please add some readings.");
        else 
            ReadingController.DisplayAllRecords();
        Console.WriteLine();
        Console.WriteLine("Press any key to return back to the main menu.");
        Console.ReadKey();
        Console.Clear();
    }
    public static void ShowLifeTimeReport()
    {
        Console.Clear();

        ReportData? reportData = ReadingController.GetReportData();

        if (reportData is null)
        {
            Console.WriteLine("No report generated due to no data been added.");
            Console.WriteLine("Add some data and come back to see report");
        }
        else
        {
            List<ReportData> quickReport = new()
            {
                reportData
            };

            ConsoleTableBuilder.From(quickReport)
                .WithTitle("Lifetime Results", ConsoleColor.Red, ConsoleColor.Gray)
                .WithColumn("# READINGS","MIN", "MAX", "AVG", "Over 200")
                .WithTextAlignment(new Dictionary<int, TextAligntment> 
                {
                    {0, TextAligntment.Center },
                    {4, TextAligntment.Center }
                })
                .ExportAndWriteLine();

            Console.WriteLine();
        }
        Console.WriteLine("Press any key to return back to the main menu.");
        Console.ReadKey();
        Console.Clear();
    }

    public static void ShowQuickReport()
    {
        Console.Clear();
        if (ReadingController.IsEmpty())
        {
            Console.WriteLine("No report can be generated due to no data present.");
            Console.WriteLine("Add some data and come back to see report");
            Console.WriteLine();
        }
        else
        {
            var first30 = ReadingController.GetReportByDays(30);
            var first60 = ReadingController.GetReportByDays(60);
            var first90 = ReadingController.GetReportByDays(90);

            var data = new List<ReportData>();

            if (first30 is not null)
            {
                data.Add(first30);
            }

            if (first60 is not null)
            {
                data.Add(first60);
            }


            if (first90 is not null)
            {
                data.Add(first90);
            }

            ConsoleTableBuilder.From(data)
                .WithTitle("Below are your 30/60/90 day report", ConsoleColor.Red, ConsoleColor.Gray)
                .WithColumn("# READINGS", "MIN", "MAX", "AVG", "Over 200")
                .WithTextAlignment(new Dictionary<int, TextAligntment>
                {
                {0, TextAligntment.Center },
                {4, TextAligntment.Center }
                })
                .ExportAndWriteLine();           
        }
        Console.WriteLine("Press any key");
        Console.ReadKey();
        Console.Clear();
    }

    public static void WriteCsv()
    {
        var csvFileName = "latest.csv";
        var count = ReadingController.Count();
        Console.Clear();
        if (count == 0) 
        {
            Console.WriteLine("There are currently no records that can be written.");
            Console.WriteLine("Once you have added some, come back.");
        }
        else
        {            
            if (File.Exists(csvFileName))
            {
                Console.WriteLine($"File {csvFileName} exist. Do you wish to overwrite it? (Y/N) |> ");
                if (Input.GetYesno())
                {
                    Console.WriteLine($"There are currently {count} records to be written to a csv file.");
                    ReadingController.WriteCSV();
                    Console.WriteLine($"{csvFileName} was written successfully.");
                }
            }
            else
            {
                Console.WriteLine($"There are currently {count} records to be written to a csv file.");
                ReadingController.WriteCSV();
                Console.WriteLine($"{csvFileName} was written successfully.");
            }
        }
        Console.WriteLine("Hit any key to return back to the main menu.");
        Console.ReadKey(true);
        Thread.Sleep(500);
        Console.Clear();
    }
}