using System;

namespace Comque.Autofac.TestConsole
{
    public class WriteToConsoleHandler : ICommandHandler<WriteToConsole, Result>
    {
        public Result Handle(WriteToConsole command)
        {
            Console.WriteLine(command.Message);

            return Result.Success();
        }
    }
}
