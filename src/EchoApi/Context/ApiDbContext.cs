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

        public DbSet<Message> Items { get; set; }
        //public DbSet<UserCredentials> Users { get; set; }

        public async virtual Task<List<Message>> GetMessagesAsync()
        {
          return await Items
            .OrderBy(message => message.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
        }

        // https://github.com/dotnet/AspNetCore.Docs.Samples/blob/main/test/integration-tests/8.x/IntegrationTestsSample/src/RazorPagesProject/Data/ApplicationDbContext.cs
        // ...
    }

    /*
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserCredentials>()
            .HasNoKey();
    } */
}
