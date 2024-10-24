using Microsoft.EntityFrameworkCore;
using EchoApi.Context;

namespace UnitTests.Helpers;

public class UnitDb : IDbContextFactory<ApiDbContext>
{
    public ApiDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<ApiDbContext>()
            .UseInMemoryDatabase($"InMemoryTestDb-{DateTime.Now.ToFileTimeUtc()}")
            .Options;

        return new ApiDbContext(options);
    }
}
