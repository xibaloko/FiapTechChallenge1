﻿using FiapTechChallenge.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FiapTechChallenge.Infra.Data
{
    public class AppDbContext : DbContext
    {
        private readonly string _connectionString;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<DDD> DDDs { get; set; }
        public virtual DbSet<Person> People { get; set; }
        public virtual DbSet<Phone> Phones { get; set; }
        public virtual DbSet<PhoneType> PhoneTypes { get; set; }
        public virtual DbSet<Region> Regions { get; set; }
        public virtual DbSet<State> States { get; set; }
    }
}
