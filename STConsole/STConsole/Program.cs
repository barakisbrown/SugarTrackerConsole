using Serilog;
using STConsole.DataLayer;
using STConsole.UserInput;

// Following will make sure that Crtl/C can not be used to exit the application
Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
{
    e.Cancel = true;
};
// BEING LOG SETUP
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.File("app.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();
// END LOG SETUP
// CHECKING IF DATABASE AND TABLES EXIST
_ = new ReadingController();
Log.Information("Sugar Tracker App Starting up");
int option = -1;
while (option != 0)
{
    Menu.GetMenu();
    option = Menu.GetMenuSelection();
    switch (option)
    {
        case 0:
            break;

        case 1:
            Log.Debug("Display all readings");
            Menu.ShowAll();
            break;

        case 2:
            Log.Debug("Add new reading");
            Menu.Add();
            break;

        case 3:
            Log.Debug("Delete a reading");
            Menu.Delete();
            break;

        case 4:
            Log.Debug("Update Readings");
            Menu.Update();
            break;

        case 5:
            Log.Debug("Quick Report");
            Menu.ShowReport();
            break;
    }
}
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Thank you for using Sugar Tracker.");
Log.Information("Sugar Tracker App Shutting Down");