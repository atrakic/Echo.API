using Message.Model;

using Microsoft.EntityFrameworkCore;
using System;

namespace Message.DAL
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
        {
        }

        public DbSet<MessageItem> Items { get; set; }
    }
}
