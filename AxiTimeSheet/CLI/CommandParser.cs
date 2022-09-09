using System.Reflection;
using CommandLine;
using CSharpFunctionalExtensions;
using static CSharpFunctionalExtensions.Result;

namespace AxiTimeSheet.CLI;

public class CommandParser
{
    private readonly Parser parser;
    private readonly Type[] commandTypes;

    public CommandParser()
    {
        parser = new Parser(config =>
        {
            config.CaseSensitive = false;
        });

        commandTypes = 
            Assembly.GetAssembly(typeof(CommandParser))!
                .GetTypes()
                .Where(type => type.IsAssignableTo(typeof(ICommand)))
                .ToArray();
    }

    public Result<ICommand> Parse(string commandStr)
    {
        Result<ICommand>? result = null;
        parser.ParseArguments(commandStr.Split(' '), commandTypes)
            .WithParsed<ICommand>(command => result = Success(command))
            .WithNotParsed(_ => result = Failure<ICommand>("I didn't get that!"));
        return result!.Value;
    }
}