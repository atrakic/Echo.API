using EchoApi.Model;

using Microsoft.EntityFrameworkCore;
using System;

namespace EchoApi.Context
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext> options) : base(options)
        {
        }

        public DbSet<MessageItem> Items { get; set; }
        //public DbSet<UserCredentials> Users { get; set; }
    }

    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCredentials>()
            .HasNoKey();
    } */
}
