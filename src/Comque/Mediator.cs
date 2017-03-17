using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace Comque
{
    // TODO for Notify mediator method use a CollectionHandlerFactory, return IEnumerable<object> (https://stackoverflow.com/questions/1406148/autofac-resolve-all-instances-of-a-type)

    public delegate object HandlerFactory(Type handlerType);

    public class Mediator : IMediator
    {
        private const string HandlerError = "Could not find handler for message {0}. Handler might not be registered or not registered correctly in the IoC container.";
        private const string HandleMethodName = "Handle";
        private const string HandleAsyncMethodName = "HandleAsync";
        private readonly HandlerFactory handlerFactory;

        public Mediator(HandlerFactory handlerFactory)
        {
            this.handlerFactory = handlerFactory;
        }

        private Type CreateHandlerType(Type emptyHandlerType, Type resultType, Type messageType)
        {
            if (resultType == null)
            {
                return emptyHandlerType.MakeGenericType(messageType);
            }
            return emptyHandlerType.MakeGenericType(messageType, resultType);
        }

        private object GetHandler(Type handlerType, Type messageType)
        {
            object handler;

            try
            {
                handler = handlerFactory(handlerType);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(string.Format(HandlerError, messageType), e);
            }

            if (handler == null)
            {
                throw new InvalidOperationException(string.Format(HandlerError, messageType));
            }

            return handler;
        }

        private object InvokeHandleMethod(Type emptyHandlerType, Type resultType, object message)
        {
            var messageType = message.GetType();
            var handlerType = CreateHandlerType(emptyHandlerType, resultType, messageType);
            var handler = GetHandler(handlerType, messageType);

            // Specific for sync methods
            var handleMethod = handler.GetType().GetRuntimeMethod(HandleMethodName, new[] { messageType });
            return handleMethod.Invoke(handler, new[] { message });
        }

        private object InvokeHandleAsyncMethod(Type emptyHandlerType, Type resultType,  object message, CancellationToken cancellationToken)
        {
            var messageType = message.GetType();
            var handlerType = CreateHandlerType(emptyHandlerType, resultType, messageType);
            var handler = GetHandler(handlerType, messageType);

            // Specific for async methods
            var handleMethod = handler.GetType().GetRuntimeMethod(HandleAsyncMethodName, new[] { messageType, cancellationToken.GetType() });
            return handleMethod.Invoke(handler, new[] { message, cancellationToken });
        }



        public virtual void Execute(ICommand command)
        {
            InvokeHandleMethod(typeof(ICommandHandler<>), null, command);
        }

        public virtual Task ExecuteAsync(ICommand command, CancellationToken cancellationToken)
        {
            var result = (Task)InvokeHandleAsyncMethod(typeof(IAsyncCommandHandler<>), null, command, cancellationToken);
            return result;
        }

        public virtual TResult Execute<TResult>(ICommand<TResult> command)
        {
            var result = (TResult)InvokeHandleMethod(typeof(ICommandHandler<,>), typeof(TResult), command);
            return result;
        }

        public virtual Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken)
        {
            var result = (Task<TResult>)InvokeHandleAsyncMethod(typeof(IAsyncCommandHandler<,>), typeof(TResult), command, cancellationToken);
            return result;
        }

        public virtual TResult Execute<TResult>(IQuery<TResult> query)
        {
            var result = (TResult)InvokeHandleMethod(typeof(IQueryHandler<,>), typeof(TResult), query);
            return result;
        }

        public virtual Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken)
        {
            var result = (Task<TResult>)InvokeHandleAsyncMethod(typeof(IAsyncQueryHandler<,>), typeof(TResult), query, cancellationToken);
            return result;
        }
    }
}
