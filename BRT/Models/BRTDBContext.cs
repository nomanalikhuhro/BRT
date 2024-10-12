using BRT.Models.Locations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

using Microsoft.EntityFrameworkCore.Metadata;
using System.Reflection;
using System.Linq.Expressions;

namespace BRT.Models
{
    public class BRTDBContext:DbContext
    {
        public BRTDBContext(DbContextOptions<BRTDBContext> options)
        : base(options)
        {
        }

        public DbSet<BRTMAINLOCATIONS> BRTMAINLOCATIONS { get; set; }

        public DbSet<BRTSUBLOCATIONS> BRTSUBLOCATIONS { get; set; }

        public DbSet<ContainerLocation> ContainerLocation { get; set; }
        
        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuring the relationships
            modelBuilder.Entity<BRTMAINLOCATIONS>()
                .HasMany(m => m.SubLocations)
                .WithOne(s => s.MainLocation)
                .HasForeignKey(s => s.MainLocationId);

            // Other configurations if needed
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var directory = System.IO.Directory.GetCurrentDirectory();

                IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(directory)
                    .AddJsonFile("appsettings.json")
                    .Build();

                var res = configuration.GetConnectionString("DefaultConnection");
                //optionsBuilder.UseSqlServer(
                //     res
                //    , options => options.ExecutionStrategy(c => new MyExecutionStrategy(c, 2, TimeSpan.FromSeconds(5)))
                //    );

            }
        }

    }
}
