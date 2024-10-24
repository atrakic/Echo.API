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

        public DbSet<Messages> Items { get; set; }

        public async virtual Task<List<Messages>> GetMessagesAsync()
        {
            return await Items.ToListAsync();
        }

        public async virtual Task AddMessageAsync(Messages message)
        {
            await Items.AddAsync(message);
            await SaveChangesAsync();
        }
    }
}
