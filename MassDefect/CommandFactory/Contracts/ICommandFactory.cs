namespace MassDefect.CommandFactory.Contracts
{
    using Commands.Contracts;

    public interface ICommandFactory
    {
        ICommand GetCommand(string commandIdentifier);
    }
}
