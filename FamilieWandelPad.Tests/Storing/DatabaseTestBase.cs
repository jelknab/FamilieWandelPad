using System;
using FamilieWandelPad.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace FamilieWandelPad.Tests.Storing
{
    public class DatabaseTestBase : IDisposable
    {
        private DbContextOptions<RouteDbContext> _options { get; set; }
        private SqliteConnection _connection { get; set; }
        
        public DatabaseTestBase()
        {
            _connection = new SqliteConnection("DataSource=:memory:");
            _connection.Open();

            _options = new DbContextOptionsBuilder<RouteDbContext>()
                .UseSqlite(_connection)
                .Options;

            using (var context = new RouteDbContext(_options))
            {
                context.Database.EnsureCreated();
            }
        }
        
        internal RouteDbContext getContext()
        {
            return new RouteDbContext(_options);
        }
        
        public void Dispose()
        {
            _connection.Close();
        }
    }
}