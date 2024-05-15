using api.Model;

namespace api.Data
{
    public interface IItemData
    {
        Task DeleteItem(int id);
        Task<IEnumerable<string>> GetItemBrands();
        Task<ItemModel?> GetItemById(int id);
        Task<IEnumerable<ItemModel>> GetItems();
        Task<IEnumerable<string>> GetItemTypes();
        Task InsertItem(ItemModel item);
        Task UpdateItem(ItemModel item);
    }
}