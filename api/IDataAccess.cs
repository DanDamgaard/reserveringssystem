
namespace api
{
    public interface IDataAccess
    {
        Task<IEnumerable<T>> LoadData<T, U>(string sql, U parameters);
        Task SaveData<T>(string sql, T parameters);
    }
}