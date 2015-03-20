using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Comque
{
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

        public virtual TResult Execute<TResult>(ICommand<TResult> command)
        {
            var result = (TResult)InvokeHandleMethod<TResult>(typeof(ICommandHandler<,>), HandleMethodName, command);
            return result;
        }

        public virtual Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            var result = (Task<TResult>)InvokeHandleMethod<TResult>(typeof(IAsyncCommandHandler<,>), HandleAsyncMethodName, command);
            return result;
        }

        public virtual TResult Execute<TResult>(IQuery<TResult> query)
        {
            var result = (TResult)InvokeHandleMethod<TResult>(typeof(IQueryHandler<,>), HandleMethodName, query);
            return result;
        }

        public virtual Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            var result = (Task<TResult>)InvokeHandleMethod<TResult>(typeof(IAsyncQueryHandler<,>), HandleAsyncMethodName, query);
            return result;
        }

        private object InvokeHandleMethod<TResult>(Type emptyHandlerType, string handleMethodName, object message)
        {
            var messageType = message.GetType();
            var handlerType = CreateHandlerType<TResult>(emptyHandlerType, messageType);
            var handler = GetHandler(handlerType, messageType);

            var handleMethod = GetHandleMethod(handler.GetType(), handleMethodName, messageType);
            return handleMethod.Invoke(handler, new[] { message });
        }

        private Type CreateHandlerType<TResult>(Type emptyHandlerType, Type messageType)
        {
            return emptyHandlerType.MakeGenericType(messageType, typeof(TResult));
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
