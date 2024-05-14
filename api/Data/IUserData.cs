using api.Model;

namespace api.Data
{
    public interface IUserData
    {
        Task<IEnumerable<UserModel>> GetUsers();
        Task InsertUser(UserModel user);
    }
}