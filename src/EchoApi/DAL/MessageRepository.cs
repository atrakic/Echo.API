using EchoApi.Model;
using EchoApi.Context;

using Microsoft.EntityFrameworkCore;
using System;

namespace EchoApi.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApiDbContext context;
        public MessageRepository(ApiDbContext context)
        {
            this.context = context;

            if (context.Items.Any())
            {
                return;
            }

            var messageItems = new Message[]
            {
                new Message { Name = "Item1" },
                new Message { Name = "Item2" },
                new Message { Name = "Item3" },
            };

            context.Items.AddRange(messageItems);
            context.SaveChanges();
        }

        public void AddItem(Message item)
        {
            context.Items.Add(item);
        }

        public void UpdateItem(Message item)
        {
            context.Items.Update(item);
        }

        public Message? GetItem(int id)
        {
            return context.Items.FirstOrDefault(x => x.Id.Equals(id));
        }

        public IEnumerable<Message> GetItems()
        {
            return context.Items.ToList();
        }

        public void RemoveItem(Message item)
        {
            context.Items.Remove(item);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
