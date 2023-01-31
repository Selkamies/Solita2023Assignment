using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;
using Microsoft.EntityFrameworkCore;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Data
{
    public class Solita2023AssignmentContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlite(Configuration.GetConnectionString("Solita2023AssignmentContext"));
            // TODO: Read this from appsettings.json?
            optionsBuilder.UseSqlite("Data Source=./Database/JourneyDatabase.db");
        }

        public DbSet<Station> Station { get; set; } = default!;
        public DbSet<Journey> Journey { get; set; } = default!;
    }
}
