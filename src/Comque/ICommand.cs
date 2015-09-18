namespace Comque
{
    public interface ICommand
    {
    }

    public interface ICommand<out TContent>
    {
    }
}
