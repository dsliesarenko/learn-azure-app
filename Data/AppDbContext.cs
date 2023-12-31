﻿using Microsoft.EntityFrameworkCore;

namespace learn_azure_app.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options)
        {
        }

        public DbSet<Person> Persons { get; set; }
    }
}
