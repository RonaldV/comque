using System.Threading.Tasks;

namespace Comque
{
    public interface ICommandHandler<in TCommand, out TResult> // : IMessageHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        TResult Handle(TCommand command);
    }

    public interface IAsyncCommandHandler<in TCommand, TResult> // : IAsyncMessageHandler<TCommand, TResult>
        where TCommand : ICommand<TResult>
    {
        Task<TResult> HandleAsync(TCommand command);
    }
}
