using EchoApi.DAL;

namespace EchoApi.Services
{
    public class MessageHandler : IMessageHandler
    {
        private readonly IMessageRepository _msgRepository;

        public MessageHandler(IMessageRepository msgRepository)
        {
            _msgRepository = msgRepository;
        }

        public IResult GetMessages()
        {
            return TypedResults.Ok(_msgRepository.GetItems());
        }
    }
}
