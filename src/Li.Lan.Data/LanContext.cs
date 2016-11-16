using Li.Lan.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Li.Lan.Data
{
    public class LanContext : DbContext
    {
        public LanContext(string connectionString)
            : base(connectionString)
        {
            // Do not attempt to create database
            Database.SetInitializer<LanContext>(null);
        }

        public DbSet<Voter> Voters { get; set; }

        public DbSet<Precinct> Precincts { get; set; }

        public DbSet<IssueTag> IssueTags { get; set; }

        public DbSet<VoterIssueTag> VoterIssueTags { get; set; }

        public DbSet<Election> Elections { get; set; }

        public DbSet<Candidate> Candidates { get; set; }

        public DbSet<VoterCandidatePreference> VoterCandidatePreferences { get; set; }

        public DbSet<VoterElection> VoterElections { get; set; }

        public DbSet<CaucusPreparation> CaucusPreparations { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // table names are not plural in DB, remove the convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}