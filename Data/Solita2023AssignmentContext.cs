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

        /*protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Journey>()
                .HasOne(j => j.DepartureStation)
                .WithOne()
                .HasForeignKey<Journey>(j => j.DepartureStationID)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Journey>()
                .HasOne(x => x.ArrivalStation)
                .WithOne()
                .HasForeignKey<Journey>(x => x.ID)
                .OnDelete(DeleteBehavior.Restrict);
        }*/

        public DbSet<Station> Station { get; set; } = default!;
        public DbSet<Journey> Journey { get; set; } = default!;
    }
}
