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

    [Command("5")]
    public class ExportTopAnomalyCommand : ICommand
    {
        private const string Path = "../../anomaly.json";

        [Injector]
        private MassDefectContext context;

        [Injector]
        private IReadeableWriteable io;

        [Injector]
        private IFileReadableWriteable fileIO;

        public void Execute()
        {
            var anomaly = this.context.Anomalies
                .Select(a => new
                {
                    id = a.Id,
                    originPlanet = new { name = a.OriginPlanet.Name },
                    teleporterPlanet = new { name = a.TeleportPlanet.Name },
                    victimsCount = a.Persons.Count
                })
                .OrderByDescending(a => a.victimsCount)
                .FirstOrDefault();

            var jsonAnomaly = JsonConvert.SerializeObject(anomaly, Formatting.Indented);

            this.fileIO.Write(Path, jsonAnomaly);

            this.io.Write("Exported to anomaly.json.");
        }
    }
}
