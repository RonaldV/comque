using System.Threading;
using System.Threading.Tasks;

namespace Comque
{
    public interface IMediator
    {
        void Execute(ICommand command);
        Task ExecuteAsync(ICommand command, CancellationToken cancellationToken);

        TResult Execute<TResult>(ICommand<TResult> command);
        Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken);

        TResult Execute<TResult>(IQuery<TResult> query);
        Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken);
    }
}
