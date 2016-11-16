using Li.Lan.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Li.Lan.Data
{
    public class UserContext : DbContext
    {
        public UserContext(string connectionString)
            : base(connectionString)
        {
            // Do not attempt to create database
            Database.SetInitializer<UserContext>(null);
        }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // table names are not plural in DB, remove the convention
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            // set up UserProfile Role Many-to-Many relationship
            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.Roles)
                .WithMany(x => x.UserProfiles)
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("RoleId");
                    m.ToTable("webpages_UsersInRoles");
                });

            modelBuilder.Entity<UserProfile>()
                .HasMany(x => x.PrecinctTags)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("PrecinctTagId");
                    m.ToTable("UserProfilePrecinctTag");
                });

            base.OnModelCreating(modelBuilder);
        }
    }
}