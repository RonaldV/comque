using System;
using System.Reflection;
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

        public virtual void Execute(ICommand command)
        {
            InvokeHandleMethod(typeof(ICommandHandler<>), null, HandleMethodName, command);
        }

        public virtual Task ExecuteAsync(ICommand command)
        {
            var result = (Task)InvokeHandleMethod(typeof(IAsyncCommandHandler<>), null, HandleAsyncMethodName, command);
            return result;
        }

        public virtual TResult Execute<TResult>(ICommand<TResult> command)
        {
            var result = (TResult)InvokeHandleMethod(typeof(ICommandHandler<,>), typeof(TResult), HandleMethodName, command);
            return result;
        }

        public virtual Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            var result = (Task<TResult>)InvokeHandleMethod(typeof(IAsyncCommandHandler<,>), typeof(TResult), HandleAsyncMethodName, command);
            return result;
        }

        public virtual TResult Execute<TResult>(IQuery<TResult> query)
        {
            var result = (TResult)InvokeHandleMethod(typeof(IQueryHandler<,>), typeof(TResult), HandleMethodName, query);
            return result;
        }

        public virtual Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            var result = (Task<TResult>)InvokeHandleMethod(typeof(IAsyncQueryHandler<,>), typeof(TResult), HandleAsyncMethodName, query);
            return result;
        }

        private object InvokeHandleMethod(Type emptyHandlerType, Type resultType, string handleMethodName, object message)
        {
            var messageType = message.GetType();
            var handlerType = CreateHandlerType(emptyHandlerType, resultType, messageType);
            var handler = GetHandler(handlerType, messageType);

            var handleMethod = GetHandleMethod(handler.GetType(), handleMethodName, messageType);
            return handleMethod.Invoke(handler, new[] { message });
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

        private MethodInfo GetHandleMethod(Type handlerType, string handleMethodName, Type messageType)
        {
            return handlerType.GetMethod(handleMethodName, new[] { messageType });
        }
    }
}
