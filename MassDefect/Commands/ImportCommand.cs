namespace MassDefect.Commands
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Attributes;
    using Contexts;
    using Contracts;
    using DBModels;
    using DTOModels;
    using DefectIO.Contracts;
    using Newtonsoft.Json;

    [Command("1")]
    public class ImportCommand : ICommand
    {
        private const string SolarSystemPath = "../../datasets/solar-systems.json";
        private const string StarsPath = "../../datasets/stars.json";
        private const string PlanetsPath = "../../datasets/planets.json";
        private const string PersonsPath = "../../datasets/persons.json";
        private const string AnomaliesPath = "../../datasets/anomalies.json";
        private const string AnomaliesVictemsPath = "../../datasets/anomaly-victims.json";

        [Injector]
        private IReadeableWriteable io;

        [Injector]
        private MassDefectContext context;

        [Injector]
        private IFileReadableWriteable fileIO;
        public void Execute()
        {
            var solarSystems = this.GetDataFromJson<SolarSystemDTO>(SolarSystemPath);
            var stars = this.GetDataFromJson<StarDTO>(StarsPath);
            var planets = this.GetDataFromJson<PlanetDTO>(PlanetsPath);
            var persons = this.GetDataFromJson<PersonDTO>(PersonsPath);
            var anomalies = this.GetDataFromJson<AnomalyDTO>(AnomaliesPath);
            var anomalyVictems = this.GetDataFromJson<AnomalyVictimsDTO>(AnomaliesVictemsPath);

            this.CreateSolarSystems(solarSystems);
            this.CreateStars(stars);
            this.CreatePlanets(planets);
            this.CreatePersons(persons);
            this.CreateAnomalies(anomalies);
            this.CreateAnomaliesVictems(anomalyVictems);
        }

        private IEnumerable<T> GetDataFromJson<T>(string path)
        {
            var json = this.fileIO.Read(path);
            var solarSystems = JsonConvert.DeserializeObject<IEnumerable<T>>(json);

            return solarSystems;
        }

        private void CreateSolarSystems(IEnumerable<SolarSystemDTO> solarSystems)
        {
            foreach (var solarSystem in solarSystems)
            {
                if (solarSystem == null || string.IsNullOrWhiteSpace(solarSystem.Name))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                SolarSystem newSolarSystem = new SolarSystem()
                {
                    Name = solarSystem.Name
                };

                this.context.SolarSystems.Add(newSolarSystem);

                this.context.SaveChanges();

                this.io.Write($"Successfully imported {nameof(solarSystem)} {solarSystem.Name}.");
            }
        }

        private void CreateStars(IEnumerable<StarDTO> stars)
        {
            foreach (var star in stars)
            {
                if (star == null || 
                    string.IsNullOrWhiteSpace(star.Name) || 
                    string.IsNullOrWhiteSpace(star.SolarSystem))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                SolarSystem solarSystem = this.context.SolarSystems
                    .Where(ss => ss.Name == star.SolarSystem)
                    .FirstOrDefault();

                if (solarSystem == default(SolarSystem))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                Star newStar = new Star()
                {
                    Name = star.Name,
                    SolarSystem = solarSystem
                };

                this.context.Stars.Add(newStar);

                this.context.SaveChanges();

                this.io.Write($"Successfully imported {nameof(star)} {star.Name}.");
            }
        }

        private void CreatePlanets(IEnumerable<PlanetDTO> planets)
        {
            foreach (var planet in planets)
            {
                if (planet == null || 
                    string.IsNullOrWhiteSpace(planet.Name) ||
                    string.IsNullOrWhiteSpace(planet.SolarSystem) ||
                    string.IsNullOrWhiteSpace(planet.Sun))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                SolarSystem solarSystem = this.context.SolarSystems
                    .Where(ss => ss.Name == planet.SolarSystem)
                    .FirstOrDefault();

                if (solarSystem == default(SolarSystem))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                Star star = this.context.Stars
                    .Where(s => s.Name == planet.Sun)
                    .FirstOrDefault();

                if (star == default(Star))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                Planet newPlanet = new Planet()
                {
                    Name = planet.Name,
                    SolarSystem = solarSystem,
                    Sun = star
                };

                this.context.Planets.Add(newPlanet);

                this.context.SaveChanges();

                this.io.Write($"Successfully imported {nameof(planet)} {planet.Name}.");
            }
        }

        private void CreatePersons(IEnumerable<PersonDTO> persons)
        {
            foreach (var person in persons)
            {
                if (string.IsNullOrWhiteSpace(person.Name) ||
                    string.IsNullOrWhiteSpace(person.HomePlanet))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                Planet planet = this.context.Planets
                    .Where(p => p.Name == person.HomePlanet)
                    .FirstOrDefault();

                if (planet == default(Planet))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                Person newPerson = new Person()
                {
                    Name = person.Name,
                    HomePlanet = planet
                };

                this.context.Persons.Add(newPerson);

                this.context.SaveChanges();

                this.io.Write($"Successfully imported {nameof(person)} {person.Name}.");
            }
        }

        private void CreateAnomalies(IEnumerable<AnomalyDTO> anomalies)
        {
            foreach (var anomaly in anomalies)
            {
                if (string.IsNullOrWhiteSpace(anomaly.OriginPlanet) ||
                    string.IsNullOrWhiteSpace(anomaly.TeleportPlanet))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                var planets = this.context.Planets
                    .Where(p => p.Name == anomaly.OriginPlanet || p.Name == anomaly.TeleportPlanet)
                    .ToList();

                if (planets.Count != 2)
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                Planet originPlanet = planets.Where(p => p.Name == anomaly.OriginPlanet).FirstOrDefault();
                Planet teleportPlanet = planets.Where(p => p.Name == anomaly.TeleportPlanet).FirstOrDefault();

                Anomaly newAnomaly = new Anomaly()
                {
                    OriginPlanet = originPlanet,
                    TeleportPlanet = teleportPlanet
                };

                this.context.Anomalies.Add(newAnomaly);

                this.context.SaveChanges();

                this.io.Write("Successfully imported anomaly.");
            }
        }

        private void CreateAnomaliesVictems(IEnumerable<AnomalyVictimsDTO> anomalyVictims)
        {
            foreach (var anomalyVictim in anomalyVictims)
            {
                Anomaly anomaly = this.context.Anomalies.Find(anomalyVictim.Id);
                Person person = this.context.Persons.Where(p => p.Name == anomalyVictim.Person).FirstOrDefault();

                if (anomaly == null || person == default(Person))
                {
                    this.io.Write("Error: Invalid data.");

                    continue;
                }

                anomaly.Persons.Add(person);

                context.SaveChanges();

                this.io.Write("Successfully imported anomaly.");
            }
        }
    }
}