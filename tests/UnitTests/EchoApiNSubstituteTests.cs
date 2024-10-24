using Microsoft.AspNetCore.Http.HttpResults;

using EchoApi;
using EchoApi.Services;
using EchoApi.Model;
using EchoApi.DAL;
using NSubstitute;

namespace UnitTests;

public class EchoApiNSubstituteTests
{
    [Fact]
    public void MessageRepositoryTest()
    {
        // Arrange
        var msgRepo = Substitute.For<IMessageRepository>();
        //msgRepo.GetMessages().Returns(new List<Message> { new Message { Id = 1, Text = "Hello" } });

        // Act
        //var handler = new MessageHandler(msgRepo);
        //var result = handler.GetMessages();

        // Assert
        //Assert.NotNull(result);
    }
}
