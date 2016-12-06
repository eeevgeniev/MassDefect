namespace MassDefect.Commands
{
    using System;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using Attributes;
    using Contexts;
    using Contracts;
    using DBModels;
    using DefectIO.Contracts;

    [Command("2")]
    public class XmlImport : ICommand
    {
        private const string XmlPath = "../../datasets/new-anomalies.xml";

        [Injector]
        private MassDefectContext context;

        [Injector]
        private IReadeableWriteable io;

        public void Execute()
        {
            var xml = XDocument.Load(XmlPath);

            var anomalies = xml.XPathSelectElements("anomalies/anomaly");

            foreach (var anomaly in anomalies)
            {
                this.ImportAnomalyAndVictems(anomaly);
            }
        }

        private void ImportAnomalyAndVictems(XElement anomalyNode)
        {
            var originPlanetAttribute = anomalyNode.Attribute("origin-planet");
            var teleportPlanetAttribute = anomalyNode.Attribute("teleport-planet");

            if (originPlanetAttribute == null || teleportPlanetAttribute == null)
            {
                this.io.Write("Error: Invalid data.");

                return;
            }

            string originPlanetAsString = originPlanetAttribute.Value;
            string teleportPlanetAsString = teleportPlanetAttribute.Value;

            var planets = this.context.Planets
                    .Where(p => p.Name == originPlanetAsString || p.Name == teleportPlanetAsString)
                    .ToList();

            if (planets.Count != 2)
            {
                this.io.Write("Error: Invalid data.");

                return;
            }

            Planet originPlanet = planets.Where(p => p.Name == originPlanetAsString).FirstOrDefault();
            Planet teleportPlanet = planets.Where(p => p.Name == teleportPlanetAsString).FirstOrDefault();

            Anomaly anomaly = new Anomaly()
            {
                OriginPlanet = originPlanet,
                TeleportPlanet = teleportPlanet
            };

            this.context.Anomalies.Add(anomaly);

            var victimElements = anomalyNode.XPathSelectElements("victims/victim");

            foreach (var victimNode in victimElements)
            {
                this.ImportVictim(victimNode, anomaly);
            }

            this.context.SaveChanges();

            this.io.Write("Successfully imported anomaly.");
        }

        private void ImportVictim(XElement victimNode, Anomaly anomaly)
        {
            var nameAttribute = victimNode.Attribute("name");

            if (nameAttribute == null)
            {
                return;
            }

            string personName = nameAttribute.Value;

            var person = this.context.Persons
                .Where(p => p.Name == personName)
                .FirstOrDefault();

            if (person == default(Person))
            {
                return;
            }

            anomaly.Persons.Add(person);
        }
    }
}
