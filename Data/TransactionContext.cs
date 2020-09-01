﻿using Microsoft.EntityFrameworkCore;
using SavioAPI.Models;

namespace SavioAPI.Data
{
    public class TransactionContext : DbContext
    {
        public TransactionContext(DbContextOptions<TransactionContext> options) : base(options)
        { }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<CategoryDto> Categories2 { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().ToTable("Transaction");
            modelBuilder.Entity<Category>().ToTable("Category");

        }
    }
}

