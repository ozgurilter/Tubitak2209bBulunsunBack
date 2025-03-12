using DemoSc.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DemoSc.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Scooter> Scooters { get; set; }
    }
}
