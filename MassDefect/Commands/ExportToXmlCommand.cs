namespace MassDefect.Commands
{
    using System.IO;
    using System.Linq;
    using System.Xml.Linq;
    using Attributes;
    using Contexts;
    using Contracts;
    using DefectIO.Contracts;

    [Command("6")]
    public class ExportToXmlCommand : ICommand
    {
        private const string Path = "../../AnomalitiesVictims.xml";

        [Injector]
        private MassDefectContext context;

        [Injector]
        private IReadeableWriteable io;

        [Injector]
        private IFileReadableWriteable fileIO;

        public void Execute()
        {
            var anomalies = this.context.Anomalies
                .Select(a => new
                {
                    Id = a.Id,
                    OriginPlanet = a.OriginPlanet.Name,
                    TeleportPlanet = a.TeleportPlanet.Name,
                    Victims = a.Persons.Select(p => new { Name = p.Name })
                })
                .OrderBy(a => a.Id);

            XElement anomalitiesXml = new XElement("anomalities");

            foreach (var anomality in anomalies)
            {
                XAttribute idAttribute = new XAttribute("id", anomality.Id);
                XAttribute originPlanetAttribute = new XAttribute("origin-planet", anomality.OriginPlanet);
                XAttribute teleportPlanetAttribute = new XAttribute("teleport-planet", anomality.TeleportPlanet);

                XElement victimsElements = new XElement("victims");

                XElement anomalityElement = new XElement(
                    "anomality", 
                    idAttribute, 
                    originPlanetAttribute, 
                    teleportPlanetAttribute, 
                    victimsElements);

                foreach (var victim in anomality.Victims)
                {
                    XAttribute victimNameAttribute = new XAttribute("name", victim.Name);

                    XElement victimElement = new XElement("victim", victimNameAttribute);

                    victimsElements.Add(victimElement);
                }

                anomalitiesXml.Add(anomalityElement);
            }

            XDocument document = new XDocument(new XDeclaration("1.0", "utf-8", null), anomalitiesXml);

            document.Save(this.fileIO.GetWriter(Path));

            this.io.Write("Exported to AnomalitiesVictims.xml.");
        }
    }
}
