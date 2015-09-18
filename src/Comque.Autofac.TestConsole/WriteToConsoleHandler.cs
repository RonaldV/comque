using System;

namespace Comque.Autofac.TestConsole
{
    public class WriteToConsoleHandler : ICommandHandler<WriteToConsole>
    {
        public void Handle(WriteToConsole command)
        {
            Console.WriteLine(command.Message);
        }
    }
}
