using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Solita2023Assignment.Models;

namespace Solita2023Assignment.Data
{
    public class Solita2023AssignmentContext : DbContext
    {
        public Solita2023AssignmentContext (DbContextOptions<Solita2023AssignmentContext> options)
            : base(options)
        {
        }

        public DbSet<Station> Station { get; set; } = default!;
        public DbSet<Journey> Journey { get; set; } = default!;
    }
}
