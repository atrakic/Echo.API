using EchoApi.Model;

namespace EchoApi.DAL
{
    public interface IMessageRepository
    {
        Message? GetItem(int id);
        IEnumerable<Message> GetItems();
        void AddItem(Message item);
        void UpdateItem(Message item);
        void RemoveItem(Message item);
        void SaveChanges();
    }
}
