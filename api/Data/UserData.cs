using api.Model;

namespace api.Data
{
    public class UserData : IUserData
    {
        private readonly IDataAccess _db;

        public UserData(IDataAccess db)
        {
            _db = db;
        }
        public Task<IEnumerable<UserModel>> GetUsers() => _db.LoadData<UserModel, dynamic>("Call GetAllUsersProcedure;", new { });
        public Task InsertUser(UserModel user) => _db.SaveData("Call CreateUserProcedure(@Username, @Password, @Department, @Type)", new { Username = user.Username, Password = user.Password, Department = user.Department, Type = user.Type });
    }
}
