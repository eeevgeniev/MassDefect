namespace MassDefect.DBModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Planet
    {
        private ICollection<Person> persons;
        private ICollection<Anomaly> originAnomalies;
        private ICollection<Anomaly> teleprotAnomalies;

        public Planet()
        {
            this.persons = new HashSet<Person>();
            this.originAnomalies = new HashSet<Anomaly>();
            this.teleprotAnomalies = new HashSet<Anomaly>();
        }

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Planet name is required.")]
        [MaxLength(200, ErrorMessage = "Planet name length is more than 200 symbols.")]
        public string Name { get; set; }

        public int? SunId { get; set; }

        public int? SolarSystemId { get; set; }

        [ForeignKey("SunId")]
        public virtual Star Sun { get; set; }

        [ForeignKey("SolarSystemId")]
        public virtual SolarSystem SolarSystem { get; set; }

        public virtual ICollection<Person> Persons
        {
            get { return this.persons; }

            set { this.persons = value; }
        }

        public virtual ICollection<Anomaly> OriginAnomalies
        {
            get { return this.originAnomalies; }

            set { this.originAnomalies = value; }
        }

        public virtual ICollection<Anomaly> TeleprotAnomalies
        {
            get { return this.teleprotAnomalies; }

            set { this.teleprotAnomalies = value; }
        }
    }
}
