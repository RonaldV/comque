# Comque
Comque is a .NET command query execution library using the mediator pattern.
It is a PCL library so it can be used in PCL projects.

## Examples
### Query

    public class GetHelloName : IQuery<string>
    {
        public string Name { get; set; }
    }
    
### Query handler

    public class GetHelloNameHandler : IQueryHandler<GetHelloName, string>
    {
        public string Handle(GetHelloName query)
        {
            return string.Format("Hello {0}!", query.Name);
        }
    }
    
### Async query handler

    public class GetHelloNameHandler : IAsyncQueryHandler<GetHelloName, string>
    {
        public async Task<string> HandleAsync(GetHelloName query)
        {
            // await some long running process
            return string.Format("Hello {0}!", query.Name);
        }
    }
    
### Query execution

    // Synchronous
    var message = mediator.Execute(new GetHelloName { Name = "world" });
    
    // Asynchronous
    var message = await mediator.ExecuteAsync(new GetHelloName { Name = "world" });
    
***

### Command

    public class WriteToConsole : ICommand
    {
        public string Message { get; set; }
    }

### Command handler
Needed for synchronous execution

    public class WriteToConsoleHandler : ICommandHandler<WriteToConsole, Result>
    {
        public Result Handle(WriteToConsole command)
        {
            Console.WriteLine(command.Message);
          
            return Result.Success();
        }
    }

### Async command handler
Needed for asynchronous execution

    public class WriteToConsoleAsyncHandler : IAsyncCommandHandler<WriteToConsole, Result>
    {
        public async Task<Result> HandleAsync(WriteToConsole command)
        {
            // await some long running process
            Console.WriteLine(command.Message);
            
            return Result.Success();
        }
    }

### Command execution

    // Synchronous
    mediator.Execute(new WriteToConsole { Message = message });
    
    // Asynchronous
    await mediator.ExecuteAsync(new WriteToConsole { Message = message });
