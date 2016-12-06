namespace MassDefect.Commands
{
    using System;
    using Attributes;
    using Contracts;

    [Command("exit")]
    public class ExitCommand : ICommand
    {
        public void Execute()
        {
            Environment.Exit(0);
        }
    }
}
