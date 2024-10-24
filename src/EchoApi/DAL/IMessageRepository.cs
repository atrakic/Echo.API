using EchoApi.Model;

namespace EchoApi.DAL
{
    public interface IMessageRepository
    {
        Messages? GetItem(int id);
        IEnumerable<Messages> GetItems();
        void AddItem(Messages item);
        void UpdateItem(Messages item);
        void RemoveItem(Messages item);
        void SaveChanges();
    }
}
