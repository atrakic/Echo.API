using Echo.Model;

using Microsoft.EntityFrameworkCore;
using System;
using System.Xml.Linq;

namespace Echo.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private readonly MessageDbContext context;
        public MessageRepository(MessageDbContext context)
        {
            this.context = context;

            if (context.Items.Any())
            {
                return;
            }

            var messageItems = new MessageItem[]
            {
                new MessageItem { Name = "Item1" },
                new MessageItem { Name = "Item2" },
                new MessageItem { Name = "Item3" },
            };

            context.Items.AddRange(messageItems);
            context.SaveChanges();
        }

        public void AddItem(MessageItem item)
        {
            context.Items.Add(item);
        }

        public void UpdateItem(MessageItem item)
        {
            context.Items.Update(item);
        }

        public MessageItem? GetItem(int id)
        {
            return context.Items.FirstOrDefault(x => x.Id.Equals(id));
        }

        public IEnumerable<MessageItem> GetItems()
        {
            return context.Items.ToList();
        }

        public void RemoveItem(MessageItem item)
        {
            context.Items.Remove(item);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
