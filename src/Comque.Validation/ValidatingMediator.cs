using System.Threading.Tasks;

namespace Comque.Validation
{
    public class ValidatingMediator : Mediator
    {
        private readonly DataAnnotationsValidator validator;

        public ValidatingMediator(HandlerFactory handlerFactory, DataAnnotationsValidator validator)
            : base(handlerFactory)
        {
            this.validator = validator;
        }

        public override TResult Execute<TResult>(IQuery<TResult> query)
        {
            validator.Validate(query);
            return base.Execute<TResult>(query);
        }

        public override Task<TResult> ExecuteAsync<TResult>(IQuery<TResult> query)
        {
            validator.Validate(query);
            return base.ExecuteAsync<TResult>(query);
        }


        public override void Execute(ICommand command)
        {
            validator.Validate(command);
            base.Execute(command);
        }

        public override Task ExecuteAsync(ICommand command)
        {
            validator.Validate(command);
            return base.ExecuteAsync(command);
        }


        public override TResult Execute<TResult>(ICommand<TResult> command)
        {
            validator.Validate(command);
            return base.Execute<TResult>(command);
        }

        public override Task<TResult> ExecuteAsync<TResult>(ICommand<TResult> command)
        {
            validator.Validate(command);
            return base.ExecuteAsync<TResult>(command);
        }
    }
}
