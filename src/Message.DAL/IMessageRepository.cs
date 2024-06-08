using Message.Model;

namespace Message.DAL
{
    public interface IMessageRepository
    {
        MessageItem? GetItem(int id);

        IEnumerable<MessageItem> GetItems();

        void AddItem(MessageItem item);

        void UpdateItem(MessageItem item);

        void RemoveItem(MessageItem item);

        void SaveChanges();
    }
}
