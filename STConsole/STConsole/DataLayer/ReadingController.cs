namespace STConsole.DataLayer;

using ConsoleTableExt;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using STConsole.Model;
using System;
using System.Globalization;

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

    public static bool UpdateAmount(Reading updatedRow)
    {
        using var _context = new ReadingContext();
        _context.Readings.Update(updatedRow);
        return _context.SaveChanges() == 1;
    }

    public static bool UpateDate(Reading updateReading)
    {
        using var _context = new ReadingContext();
        _context.Readings.Update(updateReading);
        return _context.SaveChanges() == 1;
    }

    public static bool Delete(Reading deletedRow)
    {
        using var _context = new ReadingContext();
        _context.Readings.Remove(deletedRow);
        return _context.SaveChanges() == 1;
    }

    public static Reading? Query(int _id)
    {
        using var _context = new ReadingContext();
        try
        {
            var row = _context.Readings.Where(b => b.Id == _id).First();
            return row;
        }
        catch (Exception)
        {
            Log.Error("{id} Supplied does not exist. Returning NULL back to the function", _id);
            return null;
        }
    }

    public static void DisplayAllRecords()
    {
        using var _context = new ReadingContext();
        List<Reading> rows = _context.Readings.Where(b => b.Id != -1).OrderByDescending(b => b.Added).ToList();
        Log.Debug("Number of Rows in the List is {0}", rows.Count);


        ConsoleTableBuilder.From(rows)
          .WithTitle("Blood Sugar Readings", ConsoleColor.Yellow, ConsoleColor.DarkGray)
          .WithColumn("Number","Amount","Date Added")
          .WithFormat(ConsoleTableBuilderFormat.Minimal)
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
            int count = _context.Readings.Count();
            int max = _context.Readings.Max(x => x.Amount);
            int min = _context.Readings.Min(x => x.Amount);
            int avg = (int)_context.Readings.Average(x => x.Amount);
            int ovr = _context.Readings.Count(x => x.Amount > 200);
            return new ReportData { Count = count, MAX = max, MIN = min, AVG = avg, Over200 = ovr };
        }
        catch (Exception ex)
        {
            Log.Error("Table has no data. I have not inserted anything yet");
            return null;
        }
    }

    public static ReportData? GetReportByDays(int dayAmount)
    {
        // SELECT COUNT(*), MIN(Amount),AVG(Amount),Max(Amount) FROM Readings WHERE Added > datetime('now', '-{dayAmount} day')
        using var conn = new ReadingContext();

        try
        {
            var comp = DateOnly.FromDateTime(DateTime.Now.AddDays(-dayAmount));
            var result = conn.Readings.Where(r => r.Added > comp);

            ReportData data = new()
            {
                Count = result.Count(),
                MIN = result.Min(x => x.Amount),
                MAX = result.Max(x => x.Amount),
                AVG = (int)result.Average(x => x.Amount),
                Over200 = result.Count(x => x.Amount > 200)
            };

            return data;
        }
        catch (Exception)
        {
            Log.Error("Table has no data. I have not inserted anything yet");
            return null;
        }
    }

    public static bool IsEmpty()
    {
        using var ctx = new ReadingContext();
        return !ctx.Readings.Any();
    }

    public static int Count()
    {
        using var ctx = new ReadingContext();
        return ctx.Readings.Count();
    }

    public static List<Reading> GetAlll()
    {
        using var ctx = new ReadingContext();
        var records = ctx.Readings.Where(b => b.Id > 0).ToList();
        return records;
    }

    public static void WriteCSV()
    {
        var csvFileName = "latest.csv";
        var records = GetAlll();
        using var writer = new StreamWriter(csvFileName);
        using var csv = new CsvWriter(writer, CultureInfo.InvariantCulture);
        csv.WriteRecords(records);
    }
}