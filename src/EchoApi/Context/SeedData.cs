using EchoApi.Model;

namespace EchoApi.Context
{
    public class SeedData
    {
        public static void Initialize(ApiDbContext context)
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

            var items = new Message[]
            {
                new Message { Name = "Hello World!" },
                new Message { Name = "Hello Universe!" },
                new Message { Name = "Hello Galaxy!" },
            };

            context.Items.AddRange(items);
            context.SaveChanges();
        }
    }
}
