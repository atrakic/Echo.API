using EchoApi.Model;
using EchoApi.Context;

namespace EchoApi.DAL
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApiDbContext context;
        private readonly ILogger<MessageRepository> logger;

        public MessageRepository(ApiDbContext context, ILogger<MessageRepository> logger)
        {
            this.context = context;
            this.logger = logger;

            if (context.Items.Any())
            {
                return;
            }

            logger.LogInformation("Seeding initial message items.");
            var messageItems = new Messages[]
            {
                new Messages { Message = "Hello World from Echo Api" },
            };
            context.Items.AddRange(messageItems);
            context.SaveChanges();
        }

        public void AddItem(Messages item)
        {
            logger.LogInformation("Adding a new message item.");
            context.Items.Add(item);
            context.SaveChanges();
        }

        public void UpdateItem(Messages item)
        {
            logger.LogInformation("Updating message item with ID {Id}.", item.Id);
            context.Items.Update(item);
            context.SaveChanges();
        }

        public Messages? GetItem(int id)
        {
            logger.LogInformation("Retrieving message item with ID {Id}.", id);
            return context.Items.FirstOrDefault(x => x.Id.Equals(id));
        }

        public IEnumerable<Messages> GetItems()
        {
            return context.Items.ToList();
        }

        public void RemoveItem(Messages item)
        {
            logger.LogInformation("Removing message item with ID {Id}.", item.Id);
            context.Items.Remove(item);
        }

        public void SaveChanges()
        {
            context.SaveChanges();
        }
    }
}
