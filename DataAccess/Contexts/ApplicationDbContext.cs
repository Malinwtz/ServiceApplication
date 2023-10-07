using DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Contexts
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {

        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            // automatiskt skapa databasen
            Database.EnsureCreated();
            try
            {
                Database.Migrate(); // Läser migrationsfilen och skapar databasen om den inte redan finns. 
            }
            catch (Exception ex) { }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();
        }

        public DbSet<ApplicationSettings> Settings { get; set; }

    }
}
