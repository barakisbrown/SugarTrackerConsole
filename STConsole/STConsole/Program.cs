using Serilog;
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
Log.Information("Sugar Tracker Console App Starting up");
int option = -1;
while(option != 0)
{
    Console.WriteLine("Welcome to Sugar Tracker.  This will help track my blood sugar so I can see how I am doing.");
    Menu.GetMenu();
    option = Menu.GetMenuSelection();
    switch(option)
    {
        case 0:            
            break;
        case 1:
            Log.Debug("Displaying all records");
            Menu.ShowAll();
            break;
        case 2:
            Log.Debug("Adding new records");
            Menu.Add();
            break;
        case 3:
            Log.Debug("Deleting Records");
            Menu.Delete();
            break;
        case 4:
            Log.Debug("Updating Records");
            Menu.Update();
            break;
        case 5:
            Log.Debug("Detailed Report");
            Menu.ShowReport();
            break;
    }
}
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Thank you for using Sugar Tracker.");
Log.Information("Sugar Tracker Console App Shutting Down");