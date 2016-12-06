﻿namespace MassDefect.DBModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Person
    {
        private ICollection<Anomaly> anomalies;

        public Person()
        {
            this.anomalies = new HashSet<Anomaly>();
        }

        [Key]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Person name is required.")]
        [MaxLength(200, ErrorMessage = "Person name length is more than 200 symbols.")]
        public string Name { get; set; }

        public int? HomePlanetId { get; set; }

        [ForeignKey("HomePlanetId")]
        public virtual Planet HomePlanet { get; set; }

        public virtual ICollection<Anomaly> Anomalies
        {
            get { return this.anomalies; }

            set { this.anomalies = value; }
        }
    }
}