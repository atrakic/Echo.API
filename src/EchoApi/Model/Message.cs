namespace EchoApi.Model
{
    public class Message
    {
        public int Id { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public string? Name { get; set; }
    }
}
