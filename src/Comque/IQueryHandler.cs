using System.Threading.Tasks;

namespace Comque
{
    public interface IQueryHandler<in TQuery, out TResult> // : IMessageHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        TResult Handle(TQuery query);
    }

    public interface IAsyncQueryHandler<in TQuery, TResult> // : IAsyncMessageHandler<TQuery, TResult>
        where TQuery : IQuery<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}
