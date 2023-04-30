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
    Log.Debug("option {option} has been selected",option);
    switch(option)
    {
        case 0:            
            break;
        case 1:
            Menu.ShowAll();
            break;
        case 2:
            Menu.Add();
            break;
        case 3:
            Menu.Delete();
            break;
        case 4:
            Menu.Update();
            break;
        case 5:
            Menu.ShowReport();
            break;
    }
}
Console.WriteLine();
Console.WriteLine();
Console.WriteLine("Thank you for using Sugar Tracker.");
Log.Information("Sugar Tracker Console App Shutting Down");