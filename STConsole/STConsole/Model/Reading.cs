namespace STConsole.Model;

public class Reading
{
    public int Id { get; set; }
    public int Amount { get; set; }
    public DateTime Added { get; set; }

    public override string ToString()
    {
        return $"ID = {Id}, Amount = {Amount}, Date Added = {Added.ToShortDateString()}";
    }
}
