using api.Model;

namespace api.Data
{
    public class DepartmentItemData : IDepartmentItemData
    {
        private readonly IDataAccess _db;

        public DepartmentItemData(IDataAccess db)
        {
            _db = db;
        }
        public Task<IEnumerable<DepartmentItemModel>> GetDepartmentItems() => _db.LoadData<DepartmentItemModel, dynamic>("Call GetDepartmentItemsProcedure;", new { });
        public Task<IEnumerable<DepartmentItemModel>> GetDepartmentItemsByDepartmentId(int id) => _db.LoadData<DepartmentItemModel, dynamic>("Call GetDepartmentItemsByDepartmentIdProcedure(@Id);", new { Id = id });
        public Task<IEnumerable<DepartmentItemModel>> GetDepartmentItemsById(int id) => _db.LoadData<DepartmentItemModel, dynamic>("Call GetDepartmentItemById(@Id);", new { Id = id });
        public Task UpdateDepartmentItem(DepartmentItemModel DepartmentItem) => _db.SaveData("Call UpdateDepartmentItemProcedure(@Id, @ItemNo, @Status, @CustomerName, @CustomerPhone, @StartDate, @EndDate)", new { Id = DepartmentItem.Id,ItemNo = DepartmentItem.ItemNo, Status = DepartmentItem.Status, CustomerName = DepartmentItem.CustomerName, CustomerPhone = DepartmentItem.CustomerPhone, StartDate = DepartmentItem.StartDate, EndDate = DepartmentItem.EndDate });
        public Task InsertDepartmentItem(int itemId, int departmentId, int count) => _db.SaveData("Call CreateDepartmentItemProcedure(@ItemId, @DepartmentId, @Count)", new { ItemId = itemId, DepartmentId = departmentId, Count = count });
        public Task DeleteDepartmentItem(int id) => _db.SaveData("Call DeleteDepartmentItemProcedure(@Id)", new { Id = id });
    }
}
