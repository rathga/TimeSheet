
namespace AxiTimeSheet.CLI;

public class ErrorStatusRow
{
    private readonly int rowNum;
    private readonly ConsoleColor defaultConsoleColor;
    private int statusLength;

    public ErrorStatusRow(int rowNum, ConsoleColor defaultConsoleColor)
    {
        this.rowNum = rowNum;
        this.defaultConsoleColor = defaultConsoleColor;
    }

    public void SetStatus(string status, bool error = false)
    {
        Console.SetCursorPosition(0, rowNum);
        Console.Write(new string(' ', statusLength));
        statusLength = status.Length;
        Console.SetCursorPosition(0, rowNum);
        if (error)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write("ERR: ");
            Console.ForegroundColor = defaultConsoleColor;
            statusLength += 5;
        }
        Console.Write(status);
    }
}