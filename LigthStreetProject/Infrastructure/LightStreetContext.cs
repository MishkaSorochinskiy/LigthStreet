using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure
{
    public class LightStreetContext: DbContext
    {
        public DbSet<Point> Points { get; set; }

        public LightStreetContext(DbContextOptions<LightStreetContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
