using api.Model;

namespace api.Data
{
    public class ItemData : IItemData
    {
        private readonly IDataAccess _db;

        public ItemData(IDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<ItemModel>> GetItems() => _db.LoadData<ItemModel, dynamic>("Call GetAllItemsProcedure;", new { });
        public Task<IEnumerable<string>> GetItemTypes() => _db.LoadData<string, dynamic>("Call GetAllItemTypesProcedure;", new { });
        public Task<IEnumerable<string>> GetItemBrands() => _db.LoadData<string, dynamic>("Call GetAllItemBrandsProcedure;", new { });
        public async Task<ItemModel?> GetItemById(int id)
        {
            var result = await _db.LoadData<ItemModel, dynamic>("Call GetUserByIdProcedure(@ID)", new { ID = id });

            return result.First();
        }
        public Task InsertItem(ItemModel item) => _db.SaveData("Call CreateItemProcedure(@Name, @Brand, @Type)", new { Name = item.Name, Brand = item.Brand, Type = item.Type });
        public Task UpdateItem(ItemModel item) => _db.SaveData("Call UpdateItemProcedure(@ItemId, @ItemName, @ItemBrand, @ItemType)", new { ItemId = item.Id, ItemName = item.Name, ItemBrand = item.Brand, ItemType = item.Type });
        public Task DeleteItem(int id) => _db.SaveData("Call DeleteItemProcedure(@ItemId)", new { ItemId = id });
    }
}
