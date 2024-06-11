using EchoApi.Model;

namespace EchoApi.Context
{
    public class SeedData
    {
        public static void Initialize(MessageDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.Items.Any())
            {
                return;
            }

            if (context.Items.Any())
            {
                return;
            }

            var messageItems = new MessageItem[]
            {
                new MessageItem { Name = "Hello World!" },
                new MessageItem { Name = "Hello Universe!" },
                new MessageItem { Name = "Hello Galaxy!" },
            };

            context.Items.AddRange(messageItems);
            context.SaveChanges();
        }
    }
}
