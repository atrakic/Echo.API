using EchoApi.Model;

using Microsoft.EntityFrameworkCore;
using System;

namespace EchoApi.Context
{
    public class MessageDbContext : DbContext
    {
        public MessageDbContext(DbContextOptions<MessageDbContext> options) : base(options)
        {
        }

        public DbSet<MessageItem> Items { get; set; }
    }
}
