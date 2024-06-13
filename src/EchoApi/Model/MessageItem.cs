namespace EchoApi.Model
{
    public class MessageItem
    {
        public int Id { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? Name { get; set; }
    }
}
