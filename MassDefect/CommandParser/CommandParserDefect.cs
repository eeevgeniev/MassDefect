namespace MassDefect.CommandParser
{
    using System;
    using Contracts;

    public class CommandParserDefect : ICommandParsable
    {
        public string GetCommand(string command)
        {
            return command.Trim();
        }
    }
}
