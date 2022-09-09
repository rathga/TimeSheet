
namespace AxiTimeSheet.CLI;

public class StatusRow
{
    private readonly int rowNum;
    private int statusLength;

    public StatusRow(int rowNum)
    {
        this.rowNum = rowNum;
    }

    public void SetStatus(string status)
    {
        Console.SetCursorPosition(0, rowNum);
        Console.Write(new string(' ', statusLength));
        statusLength = status.Length;
        Console.SetCursorPosition(0, rowNum);
        Console.Write(status);
    }
}