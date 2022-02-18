

// ReSharper disable UnusedAutoPropertyAccessor.Global

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Profilum.AccountService.DAL.EF.Entities;

namespace Profilum.AccountService.DAL.EF.Contexts
{
    public sealed class BaseContext : DbContext
    {
        public BaseContext(DbContextOptions options) : base(options)
        {}

        // All keys and constraints are performed here.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountEntity>(entity =>
            {
                // AccountEntity primary Key.
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).ValueGeneratedOnAdd();
            });
        }

        
        public DbSet<AccountEntity> AccountEntities { get; set; }
    }
}