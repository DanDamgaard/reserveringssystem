﻿using api.Model;

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
        public async Task<UserModel?> GetUserById(int id)
        {
            var result = await _db.LoadData<UserModel, dynamic>("Call GetUserByIdProcedure(@ID)", new { ID = id });

            return result.First();
        }

        public async Task<UserModel?> GetUserByName(string name)
        {
            var result = await _db.LoadData<UserModel, dynamic>("Call GetUserByNameProcedure(@NAME)", new { NAME = name });

            return result.First();
        }

        public Task UpdateUser(UserModel user) => _db.SaveData("Call UpdateUserProcedure(@UserId, @User, @Pass)", new { UserId = user.Id, User = user.Username, Pass = user.Password });
        public Task InsertUser(UserModel user) => _db.SaveData("Call CreateUserProcedure(@Username, @Password, @Department, @Type)", new { Username = user.Username, Password = user.Password, Department = user.Department, Type = user.Type });
        public Task DeleteUser(int id) => _db.SaveData("Call DeleteUserProcedure(@UserId)", new { UserId = id });
    }
}
