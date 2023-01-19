using UlmApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace UlmApi.Infra.Data
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options) : base(options) { }

        public DbSet<License> Licenses { get; set; }
        public DbSet<RequestLicense> RequestLicenses { get; set; }
        public DbSet<Application> Applications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Solution> Solutions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var urlDb = Environment.GetEnvironmentVariable("DB_URL") ?? "localhost";
            var user = Environment.GetEnvironmentVariable("POSTGRES_USER") ?? "postgres";
            var password = Environment.GetEnvironmentVariable("POSTGRES_PASSWORD") ?? "adm";
            var db = Environment.GetEnvironmentVariable("POSTGRES_DB") ?? "ULMDB_DEV";
            var port = Environment.GetEnvironmentVariable("EXTERNAL_PORT_DB") ?? "5432";
            optionsBuilder.UseNpgsql($"Host={urlDb};Port={port};Username={user};Password={password};Database={db};"); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(Context).Assembly);
        }
    }
}
