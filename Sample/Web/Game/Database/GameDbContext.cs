using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using OddMud.Web.Game.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OddMud.Web.Game.Database
{
    public class GameDbContext : DbContext
    {

        public DbSet<DbMap> Maps { get; set; }
        public DbSet<DbMapExit> MapExits { get; set; }
        public DbSet<DbItem> Items { get; set; }
        public DbSet<DbItemStat> ItemStats { get; set; }

        public DbSet<DbSpawner> Spawners { get; set; }
        public DbSet<DbEntity> Entities { get; set; }

        public DbSet<DbItemTypes> ItemsAssignedTypes { get; set; }


        public DbSet<DbPlayer> Players { get; set; }
        public DbSet<DbEntityItem> PlayerItems { get; set; }
        public DbSet<DbPlayerItemStat> PlayerItemStats { get; set; }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connection = "";
            const string configKey = "Data:DefaultConnection:ConnectionStringGame";

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            //assume running from testing projects and get test configuration
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env}.json", true);

            var configuration = builder.Build();
            connection = configuration[configKey];

            optionsBuilder.UseSqlServer(connection)
                .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.QueryClientEvaluationWarning));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ForSqlServerUseIdentityColumns();

            modelBuilder.Entity<DbPlayer>().HasIndex(p => p.Name).IsUnique();

        }

    }
}
