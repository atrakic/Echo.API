using System.ComponentModel.DataAnnotations.Schema;

namespace Message.Model
{
    public class MessageItem
    {
        public int Id { get; private set; }
        public string? Name { get; set; }
    }
}
