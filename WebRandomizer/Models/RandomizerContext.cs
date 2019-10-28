﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace WebRandomizer.Models {
    public class RandomizerContext : DbContext {
        public RandomizerContext(DbContextOptions<RandomizerContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.Entity<Client>().UseXminAsConcurrencyToken();
        }

        public DbSet<Session> Sessions { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Seed> Seeds { get; set; }
        public DbSet<World> Worlds { get; set; }
        public DbSet<Event> Events { get; set; }

    }
}
