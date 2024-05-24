using api.Model;

namespace api.Data
{
    public interface IDepartmentItemData
    {
        Task DeleteDepartmentItem(int id);
        Task<IEnumerable<DepartmentItemModel>> GetDepartmentItems();
        Task<IEnumerable<DepartmentItemModel>> GetDepartmentItemsByDepartmentId(int id);
        Task<IEnumerable<DepartmentItemModel>> GetDepartmentItemsById(int id);
        Task InsertDepartmentItem(int itemId, int departmentId, int count);
        Task UpdateDepartmentItem(DepartmentItemModel DepartmentItem);
    }
}