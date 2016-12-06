namespace MassDefect.CommandFactory
{
    using System;
    using System.Linq;
    using System.Reflection;
    using Attributes;
    using CommandParser.Contracts;
    using Commands.Contracts;
    using Contexts;
    using Contracts;
    using DefectIO.Contracts;

    public class CommandFactoryDefect : ICommandFactory
    {
        private readonly MassDefectContext context;
        private readonly IReadeableWriteable io;
        private readonly IFileReadableWriteable fileIO;

        public CommandFactoryDefect(MassDefectContext context, 
            IReadeableWriteable io,
            IFileReadableWriteable fileIO)
        {
            this.context = context;
            this.io = io;
            this.fileIO = fileIO;
        }

        protected MassDefectContext Context => this.context;

        protected IReadeableWriteable IO => this.io;

        protected IFileReadable FileIO => this.fileIO;

        public ICommand GetCommand(string commandIdentifier)
        {
            var commands = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.IsDefined(typeof(CommandAttribute)))
                .ToList();

            foreach (var command in commands)
            {
                var commandAttr = command.GetCustomAttribute(typeof(CommandAttribute), true) as CommandAttribute;

                if (commandAttr.Name != commandIdentifier)
                {
                    continue;
                }

                ICommand executingCommand = (ICommand)Activator.CreateInstance(command);

                var executingCommandType = executingCommand.GetType();

                var fields = executingCommandType
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                    .Where(f => f.IsDefined(typeof(InjectorAttribute)))
                    .ToList();

                var fieldsToAdd = this.GetType()
                    .GetFields(BindingFlags.NonPublic | BindingFlags.Instance);

                foreach (var fieldToAdd in fieldsToAdd)
                {
                    foreach (var field in fields)
                    {
                        if (fieldToAdd.FieldType == field.FieldType)
                        {
                            field.SetValue(executingCommand, fieldToAdd.GetValue(this));
                        }
                    }
                }

                return executingCommand;
            }

            throw new ArgumentException("Invalid command.");
        }
    }
}
