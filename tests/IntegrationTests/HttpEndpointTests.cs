
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using EchoApi.DAL;

using IntegrationTests.Helpers;
using System.Net;

namespace IntegrationTests;

public class HttpEndpointTests
{
    private readonly HttpClient _client;
    private readonly TestWebApplicationFactory<Program> _factory;

    public HttpEndpointTests()
    {
        _factory = new TestWebApplicationFactory<Program>();
        _client = _factory.CreateClient();
    }

    [Fact]
    public void RoutesExistsTest()
    {
        new string[] {
            "/api/messages",
            "/healthz" }
            .ToList()
            .ForEach(async path =>
            {
                var response = await _client.GetAsync(path);
                response.EnsureSuccessStatusCode();
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            });
    }
}
