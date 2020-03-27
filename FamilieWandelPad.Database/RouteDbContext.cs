using FamilieWandelPad.Database.Model;
using FamilieWandelPad.Database.Model.waypoints;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FamilieWandelPad.Database
{
    public class RouteDbContext : DbContext
    {
        public RouteDbContext(DbContextOptions options) : base(options)
        {
        }

        public static RouteDbContext GetContext(string path)
        {
            var connectionString = new SqliteConnectionStringBuilder()
            {
                DataSource = path,
                Mode = SqliteOpenMode.ReadWriteCreate
            }.ConnectionString;

            var options = new DbContextOptionsBuilder<RouteDbContext>()
                .UseSqlite(connectionString)
                .Options;

            using (var context = new RouteDbContext(options))
            {
                context.Database.EnsureCreated();
            }
            
            return new RouteDbContext(options);
        }

        public DbSet<Route> Route { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PointOfInterest>();
            modelBuilder.Entity<WayPoint>();
            
            base.OnModelCreating(modelBuilder);
        }
    }
}