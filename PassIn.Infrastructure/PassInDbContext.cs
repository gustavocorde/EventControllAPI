﻿using Microsoft.EntityFrameworkCore;
using PassIn.Infrastructure.Entities;

namespace PassIn.Infrastructure
{
    public class PassInDbContext : DbContext
    {

        public DbSet<Event> Events { get; set; }
        public DbSet<Attendee> Attendees { get; set; }
        public DbSet<CheckIn> CheckIns { get; set; }

        //funciton that will link this context to the database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=C:\\Users\\idiot\\Documents\\Db\\PassInDb.db");
        }

    }
}
