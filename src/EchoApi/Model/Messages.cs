namespace EchoApi.Model
{
    public class Messages
    {
        public int Id { get; private set; }
        public long UpdatedAt { get; private set; } = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        public required string Message { get; set; }
    }
}
