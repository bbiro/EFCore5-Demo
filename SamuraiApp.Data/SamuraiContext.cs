using Microsoft.EntityFrameworkCore;
using SamutaiApp.Domain;
using Microsoft.Extensions.Logging;
using System;

namespace SamuraiApp.Data
{
    public class SamuraiContext: DbContext
    {
        public DbSet<Server> Servers { get; set; }
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Battle> Battles { get; set; }

        public SamuraiContext()
        {
            Database.EnsureCreated();
        }

        public SamuraiContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring (DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql("Server=localhost;Database=SamuraiApp;Uid=pomelo;Pwd=P0m3l0;Convert Zero Datetime=True", new MySqlServerVersion(new Version(8, 0, 11)))
                .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name }, LogLevel.Information)
                .EnableSensitiveDataLogging();
        } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(SamuraiContext).Assembly);
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Samurai>()
                .HasMany(s => s.Battles)
                .WithMany(b => b.Samurais)
                .UsingEntity<BattleSamurai>
                (bs => bs.HasOne<Battle>().WithMany(),
                 bs => bs.HasOne<Samurai>().WithMany())
                .Property(bs => bs.DateJoined)
                .ValueGeneratedOnAdd();
        }
        
        public class Server
        {
            public ulong Id { get; set; }
            public string Prefix { get; set; }
        }
    }
}
