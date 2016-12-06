namespace MassDefect
{
    using System.Globalization;
    using System.Threading;
    using CommandFactory;
    using CommandFactory.Contracts;
    using CommandParser;
    using CommandParser.Contracts;
    using Commands.Contracts;
    using Contexts;
    using DefectIO;
    using DefectIO.Contracts;
    using Engine;
    using Engine.Contracts;

    public class Startup
    {
        public static void Main()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            MassDefectContext context = new MassDefectContext();

            ICommandParsable commandParser = new CommandParserDefect();
            IReadeableWriteable io = new ConsoleIO();
            IFileReadableWriteable fileIO = new FileIO();

            ICommandFactory commandFactory = new CommandFactoryDefect(context, io, fileIO);
            IEngine engine = new EngineDefect(commandFactory, commandParser, io);

            engine.Run();
        }
    }
}
