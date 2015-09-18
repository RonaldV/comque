using System.Threading.Tasks;

namespace Comque
{
    public interface IMediator
    {
        void Execute(ICommand command);
        Task ExecuteAsync(ICommand command);

        TResult Execute<TResult>(ICommand<TResult> command);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command);

        TResult Execute<TResult>(IQuery<TResult> query);
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query);
    }
}
