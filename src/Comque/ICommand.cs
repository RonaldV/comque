namespace Comque
{
    public interface ICommand<out TContent>
    {
    }

    public interface ICommand : ICommand<Result>
    {
    }
}
