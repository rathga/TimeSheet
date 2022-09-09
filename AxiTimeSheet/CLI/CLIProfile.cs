using System.Reflection;
using AutoMapper;
using CSharpFunctionalExtensions;
using MediatR;

namespace AxiTimeSheet.CLI
{
    public class CLIProfile : Profile
    {
        public CLIProfile()
        {
            var commandTypes =
                Assembly.GetAssembly(typeof(CommandParser))!
                    .GetTypes()
                    .Where(type => 
                        type.IsClass 
                        && !type.IsAbstract 
                        && type.IsAssignableTo(typeof(ICommand)) 
                        && type != typeof(QuitCommand))
                    .ToList();

            foreach (var commandType in commandTypes)
            {
                var commandInterface = commandType
                    .GetInterfaces()
                    .SingleOrDefault(inf =>
                        inf.IsConstructedGenericType && inf.GetGenericTypeDefinition() == typeof(IDomainCommand<,>));
 
                if (commandInterface is not null)
                    CreateMap(commandType, commandInterface.GetGenericArguments()[0]);
            }
        }
    }
}
