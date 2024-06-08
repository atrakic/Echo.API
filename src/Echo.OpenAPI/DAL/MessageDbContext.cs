using Echo.Model;

using Microsoft.EntityFrameworkCore;
using System;

namespace Echo.DAL
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
        {
        }

        public DbSet<MessageItem> Items { get; set; }
    }
}
