using BRTAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace BRTAPI.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options) { }

        public DbSet<Container> Containers { get; set; }
        public DbSet<AspNetUser> AspNetUsers { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<AspNetRole> AspNetRoles { get; set; }
    }

    public class Container
    {
        public int Id { get; set; }
        public string ContainerNo { get; set; }
    }
}
