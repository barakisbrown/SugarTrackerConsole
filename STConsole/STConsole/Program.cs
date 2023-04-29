using STConsole.UserInput;

Console.CancelKeyPress += delegate (object? sender, ConsoleCancelEventArgs e)
{
    e.Cancel = true;
};

int option = -1;
while(option != 0)
{
    Console.WriteLine("Welcome to Sugar Track.  This will track my blood which I can track my blood sugar.");
    Menu.GetMenu();
    option = Menu.GetMenuSelection();
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