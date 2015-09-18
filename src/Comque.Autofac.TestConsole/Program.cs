using Autofac;
using Autofac.Features.Variance;
using System;

namespace Comque.Autofac.TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = BuildContainer();
            var mediator = container.Resolve<IMediator>();

            // https://stackoverflow.com/questions/1406148/autofac-resolve-all-instances-of-a-type
            var message = mediator.Execute(new GetHelloName { Name = "world" });
            mediator.Execute(new WriteToConsole { Message = message });

            Console.ReadLine();
        }
        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterSource(new ContravariantRegistrationSource());
            builder.RegisterType<Mediator>().AsImplementedInterfaces().SingleInstance();
            builder.Register<HandlerFactory>(ctx =>
            {
                var cc = ctx.Resolve<IComponentContext>();
                return t => cc.Resolve(t);
            });

            var assembly = typeof(Program).Assembly;

            //builder.RegisterType<WriteCommandHandler>().AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(typeof(Program).Assembly).AsImplementedInterfaces();
            //builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IMessageHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(ICommandHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IAsyncCommandHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IQueryHandler<,>));
            builder.RegisterAssemblyTypes(assembly).AsClosedTypesOf(typeof(IAsyncQueryHandler<,>));

            return builder.Build();
        }
    }
}
