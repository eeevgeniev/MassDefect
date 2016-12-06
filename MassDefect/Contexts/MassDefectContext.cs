namespace MassDefect.Contexts
{
    using System.Data.Entity;
    using Migrations;
    using DBModels;

    public class MassDefectContext : DbContext
    {
        public MassDefectContext()
            : base("name=MassDefectContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<MassDefectContext, Configuration>());
        }

        public virtual DbSet<Anomaly> Anomalies { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

        public virtual DbSet<Planet> Planets { get; set; }

        public virtual DbSet<SolarSystem> SolarSystems { get; set; }

        public virtual DbSet<Star> Stars { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Anomaly>()
                .HasMany(a => a.Persons)
                .WithMany(p => p.Anomalies)
                .Map(m =>
                {
                    m.MapLeftKey("AnomalyId");
                    m.MapRightKey("PersonId");
                    m.ToTable("AnomalyVictims");
                });

            /*
            modelBuilder.Entity<Anomaly>()
                .HasRequired(a => a.OriginPlanet)
                .WithMany(p => p.OriginAnomalies)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Anomaly>()
                .HasRequired(a => a.TeleportPlanet)
                .WithMany(p => p.TeleprotAnomalies)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Star>()
                .HasRequired(s => s.SolarSystem)
                .WithMany(ss => ss.Stars)
                .WillCascadeOnDelete(false);
                */

            base.OnModelCreating(modelBuilder);
        }
    }
}