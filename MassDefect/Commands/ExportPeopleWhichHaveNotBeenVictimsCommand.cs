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

    [Command("4")]
    public class ExportPeopleWhichHaveNotBeenVictimsCommand : ICommand
    {
        private const string Path = "../../people.json";

        [Injector]
        private MassDefectContext context;

        [Injector]
        private IReadeableWriteable io;

        [Injector]
        private IFileReadableWriteable fileIO;

        public void Execute()
        {
            var people = this.context.Persons
                .Where(p => p.Anomalies.Count == 0)
                .Select(p => new
                {
                    name = p.Name,
                    homePlanet = new { name = p.HomePlanet.Name }
                });

            var jsonPersons = JsonConvert.SerializeObject(people, Formatting.Indented);

            this.fileIO.Write(Path, jsonPersons);

            this.io.Write("Exported to people.json.");
        }
    }
}
