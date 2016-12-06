namespace MassDefect.Engine
{
    using System;
    using CommandFactory.Contracts;
    using CommandParser.Contracts;
    using Commands.Contracts;
    using Contracts;
    using DefectIO.Contracts;

    public class EngineDefect : IEngine
    {
        private readonly ICommandFactory commandFactory;
        private readonly ICommandParsable commandParser;
        private readonly IReadeableWriteable io;

        public EngineDefect(
            ICommandFactory commandFactory,
            ICommandParsable commandParser,
            IReadeableWriteable io)
        {
            this.commandFactory = commandFactory;
            this.commandParser = commandParser;
            this.io = io;
        }

        protected ICommandFactory CommandFactory => this.commandFactory;

        protected ICommandParsable CommandParser => this.commandParser;

        protected IReadeableWriteable IO => this.io;

        public void Run()
        {
            ICommand helpCommand = this.CommandFactory.GetCommand("help");

            helpCommand.Execute();

            while (true)
            {
                try
                {
                    string commandText = this.IO.Read();

                    string commandIdentifier = this.commandParser.GetCommand(commandText);

                    ICommand command = this.CommandFactory.GetCommand(commandIdentifier);

                    command.Execute();
                }
                catch (ArgumentException argEx)
                {
                    this.io.Write(argEx.Message);
                }
            }
        }
    }
}
