using api.Model;

namespace api.Data
{
    public class DepartmentData : IDepartmentData
    {
        private readonly IDataAccess _db;

        public DepartmentData(IDataAccess db)
        {
            _db = db;
        }

        public Task<IEnumerable<DepartmentModel>> GetDepartment() => _db.LoadData<DepartmentModel, dynamic>("Call GetAllDepartmentsProcedure;", new { });
        public async Task<DepartmentModel?> GetDepartmentById(int id)
        {
            var result = await _db.LoadData<DepartmentModel, dynamic>("Call GetDepartmentByIdProcedure(@ID)", new { ID = id });

            return result.First();
        }
        public Task InsertDepartment(DepartmentModel Department) => _db.SaveData("Call CreateDepartmentProcedure(@City, @Address)", new { City = Department.City, Address = Department.Address });
        public Task UpdateDepartment(DepartmentModel Department) => _db.SaveData("Call UpdateDepartmentProcedure(@Id, @City, @Address)", new { Id = Department.Id, City = Department.City, Address = Department.Address });
        public Task DeleteDepartment(int id) => _db.SaveData("Call DeleteDepartmentProcedure(@Id)", new { Id = id });
    }
}
