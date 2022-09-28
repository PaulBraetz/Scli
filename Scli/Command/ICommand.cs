namespace Scli.Command
{
    public interface ICommand
    {
        string GetSelfNavigation();
        string NavigationKey { get; }
        void Run();
    }
}
