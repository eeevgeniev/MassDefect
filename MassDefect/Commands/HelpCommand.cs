namespace MassDefect.Commands
{
    using System;
    using System.Text;
    using Attributes;
    using Contracts;
    using DefectIO.Contracts;

    [Command("help")]
    public class HelpCommand : ICommand
    {
        [Injector]
        private IReadeableWriteable io;

        public void Execute()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("Press help for help.");
            builder.AppendLine("Press exit to exit.");
            builder.AppendLine("Press 1 to import json data.");
            builder.AppendLine("Press 2 to import xml data.");
            builder.AppendLine("Press 3 to export planets which are not anomaly origins command.");
            builder.AppendLine("Press 4 to export people which have not been victims.");
            builder.AppendLine("Press 5 to export top anomaly.");
            builder.Append("Press 6 to export xml.");

            io.Write(builder.ToString());
        }
    }
}
