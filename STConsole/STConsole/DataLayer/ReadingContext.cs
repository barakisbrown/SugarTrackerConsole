namespace STConsole.DataLayer;

using Microsoft.EntityFrameworkCore;
using STConsole.Model;

public class ReadingContext : DbContext
{
    public DbSet<Reading> Readings { get; set; }
    public string dbPath;

    public ReadingContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        dbPath = System.IO.Path.Join(path, "readings.db");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite($"Data Source={dbPath}");

}
