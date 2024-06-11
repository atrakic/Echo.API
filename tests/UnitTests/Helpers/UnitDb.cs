using Microsoft.EntityFrameworkCore;
using EchoApi.Context;

namespace UnitTests.Helpers;

public class UnitDb : IDbContextFactory<MessageDbContext>
{
    public MessageDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<MessageDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new MessageDbContext(options);
    }
}