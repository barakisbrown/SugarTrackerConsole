﻿namespace STConsole.DataLayer;

using ConsoleTableExt;
using STConsole.Model;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

public class ReadingController
{
    private readonly string TableName = "Readings";
    public ReadingController()
    {
        using var _context = new ReadingContext();
        if (!_context.Database.CanConnect())
        {
            Log.Error("Datase was not present. It is now created");
            _context.Database.EnsureCreated();
            Log.Error("Checking if Tables Exist.");
            CheckTableExist();
        }
        else
        {
            Log.Information("Database exist. Checking if Tables are present.");
            CheckTableExist();
        }
    }

    private void CheckTableExist()
    {
        using var _context = new ReadingContext();
        try
        {
            var first = $"SELECT 1 FROM {TableName}";
            _ = _context.Database.ExecuteSqlRaw(first);
            Log.Information("Tables were present.");
        }
        catch (Exception ex)
        {
            Log.Error("Database exist but initial table DOES NOT exist.");
            var script = _context.Database.GenerateCreateScript();
            _context.Database.ExecuteSqlRaw(script);
            Log.Information("Tables are now created");           
        }
    }

    public static bool Insert(Reading row)
    {
        using var _context = new ReadingContext();
        _context.Readings.Add(row);
        return _context.SaveChanges() == 1;
    }

    public static Reading Query(int _id)
    {
        using var _context = new ReadingContext();
        var row = _context.Readings.OrderBy(b => b.Id == _id).First();
        return row;
    }

    public static void DisplayAllRecords()
    {
        using var _context = new ReadingContext();
        List<Reading> rows = _context.Readings.Where(b => b.Id != -1).ToList();

        ConsoleTableBuilder.From(rows)
            .WithTitle("Blood Sugar Readings", ConsoleColor.Yellow, ConsoleColor.DarkGray)
            .WithTextAlignment(new Dictionary<int, TextAligntment>
            {
                {1, TextAligntment.Center },
                {2, TextAligntment.Center }
            }).ExportAndWriteLine();
    }

    public static ReportData? GetReportData()
    {
        using var _context = new ReadingContext();

        try
        {
            int max = _context.Readings.Max(x => x.Amount);
            int min = _context.Readings.Min(x => x.Amount);
            int avg = (int)_context.Readings.Average(x => x.Amount);
            int ovr = _context.Readings.Count(x => x.Amount > 200);
            return new ReportData { MAX = max, MIN = min, AVG = avg, Over200 = ovr };
        }
        catch (Exception ex)
        {
            Log.Error("Table has no data. I have not inserted anything yet");
            return null;
        }        
    }
}
