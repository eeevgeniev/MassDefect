namespace MassDefect.Commands
{
    using System;
    using System.IO;
    using System.Linq;
    using Attributes;
    using Contexts;
    using Contracts;
    using DefectIO.Contracts;
    using Newtonsoft.Json;

    [Command("3")]
    public class ExportPlanetsWhichAreNotAnomalyOriginsCommand : ICommand
    {
        private const string Path = "../../planet.json";

        [Injector]
        private MassDefectContext context;

        [Injector]
        private IReadeableWriteable io;

        [Injector]
        private IFileReadableWriteable fileIO;

        public void Execute()
        {
            var planets = this.context.Planets
                .Where(p => !p.OriginAnomalies.Any())
                .Select(p => new
                {
                    name = p.Name
                });

            var jsonPlanets = JsonConvert.SerializeObject(planets, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(Path))
            {
                writer.Write(jsonPlanets);
            }

            this.io.Write("Exported to planet.json.");
        }
    }
}
