# Comque
Comque is a .NET command query execution library using the mediator pattern.
It is a PCL library so it can be used in PCL projects.

## Examples
### Command

    public class WriteToConsole : ICommand
    {
        public string Message { get; set; }
    }

### Command handler

    public class WriteToConsoleHandler : ICommandHandler<WriteToConsole, Result>
    {
        public Result Handle(WriteToConsole command)
        {
            Console.WriteLine(command.Message);
          
            return Result.Success();
        }
    }

### Command execution

    // Synchronous
    mediator.Execute(new WriteToConsole { Message = message });
    
    // Asynchronous
    await mediator.ExecuteAsync(new WriteToConsole { Message = message });
    
