namespace Comque.Autofac.TestConsole
{
    public class GetHelloNameHandler : IQueryHandler<GetHelloName, string>
    {
        public string Handle(GetHelloName query)
        {
            return string.Format("Hello {0}!", query.Name);
        }
    }
}
