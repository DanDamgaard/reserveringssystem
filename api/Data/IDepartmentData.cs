using api.Model;

namespace api.Data
{
    public interface IDepartmentData
    {
        Task DeleteDepartment(int id);
        Task<IEnumerable<DepartmentModel>> GetDepartment();
        Task<DepartmentModel?> GetDepartmentById(int id);
        Task InsertDepartment(DepartmentModel Department);
        Task UpdateDepartment(DepartmentModel Department);
    }
}