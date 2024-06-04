using Newtonsoft.Json;
using program.Class;
using RestSharp;
using System.ComponentModel;
using System.Windows;

namespace program.Service
{
    public class Api
    {

        private static string apiIp = "https://dantdamgaard.dk/api";
        //private static string apiIp = "https://localhost:7082/api";

        private static RestClientOptions options = new RestClientOptions(apiIp)
        {
            ThrowOnAnyError = true,
            MaxTimeout = 5000,
        };

        private RestClient restClient = new RestClient(options);


        public Api()
        {
            Global.Login = new UserClass(0, "", "", 0, "", "", "");

        }

        public async Task<bool> UserLogin(UserClass user)
        {
            try
            {
                RestRequest request = new RestRequest("/User/Login", Method.Post);

                request.AddJsonBody(JsonConvert.SerializeObject(user));

                RestResponse res = await restClient.PostAsync(request);

                Global.Login = JsonConvert.DeserializeObject<UserClass>(res.Content!)!;

                restClient = new RestClient(options);

                Global.Login.token = "bearer " + Global.Login.token;

                restClient.AddDefaultHeader("Authorization", Global.Login.token);

                return true;

            }
            catch (Exception e)
            {
                Global.Logger.Error(e.Message);
                return false;
            }
        }

        public async Task<List<UserClass>> GetAllUsers()
        {
            List<UserClass> users = await restClient.GetJsonAsync<List<UserClass>>("/User");
            return users!;
        }

        public async Task CreateUser(UserClass user)
        {
            try
            {
                RestRequest request = new RestRequest("/User", Method.Post);
                request.AddJsonBody(JsonConvert.SerializeObject(user));
                RestResponse res = await restClient.PostAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke oprette ny bruger");
                Global.Logger.Error(e.Message);
            }

        }
        public async Task UpdateUser(UserClass user)
        {
            try
            {
                RestRequest request = new RestRequest("/User", Method.Put);
                request.AddJsonBody(JsonConvert.SerializeObject(user));
                await restClient.PutAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke rediger bruger");
                Global.Logger.Error(e.Message);
            }
        }

        public async Task DeleteUser(int Id)
        {
            try
            {
                RestResponse res = await restClient.DeleteAsync(new RestRequest("/User/" + Id, Method.Delete));
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke slette bruger");
                Global.Logger.Error(e.Message);
            }

        }

        public async Task<List<ItemClass>> GetAllItems()
        {
            List<ItemClass> items = await restClient.GetJsonAsync<List<ItemClass>>("/Item");
            return items!;
        }

        public async Task<List<string>> GetAllItemsTypes()
        {
            List<string> types = await restClient.GetJsonAsync<List<string>>("/Item/Types");
            return types!;
        }

        public async Task<List<string>> GetAllItemsBrands()
        {
            List<string> brands = await restClient.GetJsonAsync<List<string>>("/Item/Brands");
            return brands!;
        }

        public async Task CreateItem(ItemClass item)
        {
            try
            {
                RestRequest request = new RestRequest("/Item", Method.Post);
                request.AddJsonBody(JsonConvert.SerializeObject(item));
                RestResponse res = await restClient.PostAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke oprette ny vare");
                Global.Logger.Error(e.Message);
            }

        }

        public async Task UpdateItem(ItemClass item)
        {
            try
            {
                RestRequest request = new RestRequest("/Item", Method.Put);
                request.AddJsonBody(JsonConvert.SerializeObject(item));
                await restClient.PutAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke rediger vare");
                Global.Logger.Error(e.Message);
            }
        }

        public async Task DeleteItem(int Id)
        {
            try
            {
                RestResponse res = await restClient.DeleteAsync(new RestRequest("/Item/" + Id, Method.Delete));
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke slette Vare");
                Global.Logger.Error(e.Message);
            }

        }

        public async Task<List<DepartmentClass>> GetAllDepartments()
        {
            List<DepartmentClass> departments = await restClient.GetJsonAsync<List<DepartmentClass>>("/Department");
            return departments!;
        }

        public async Task CreateDepartmentItem(int vareId, int DepartmentId, int Count)
        {
            try
            {
                RestRequest request = new RestRequest("/DepartmentItem/" + $"{vareId}/" + $"{DepartmentId}/" + $"{Count}", Method.Post);
                RestResponse res = await restClient.PostAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke tildele afdeling varer");
                Global.Logger.Error(e.Message);
            }

        }

        public async Task<List<DepartmentItemClass>> GetAllDepartmentItemsById(int Id)
        {
            List<DepartmentItemClass> departmentItem = await restClient.GetJsonAsync<List<DepartmentItemClass>>("/DepartmentItem/ByDepartmentId/" + Id);
            return departmentItem!;
        }

        public async Task<List<DepartmentItemClass>> GetAllDepartmentItems()
        {
            List<DepartmentItemClass> departmentItem = await restClient.GetJsonAsync<List<DepartmentItemClass>>("/DepartmentItem");
            return departmentItem!;
        }

        public async Task UpdateDepartmentItem(DepartmentItemClass item)
        {
            try
            {
                RestRequest request = new RestRequest("/DepartmentItem", Method.Put);
                request.AddJsonBody(JsonConvert.SerializeObject(item));
                await restClient.PutAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke rediger vare");
                Global.Logger.Error(e.Message);
            }
        }

        public async Task DeleteDepartmentItem(int Id)
        {
            try
            {
                RestResponse res = await restClient.DeleteAsync(new RestRequest("/DepartmentItem/" + Id, Method.Delete));
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke slette Vare");
                Global.Logger.Error(e.Message);
            }

        }

        public async Task CreateDepartment(DepartmentClass department)
        {
            try
            {
                RestRequest request = new RestRequest("/Department", Method.Post);
                request.AddJsonBody(JsonConvert.SerializeObject(department));
                RestResponse res = await restClient.PostAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke oprette ny afdeling");
                Global.Logger.Error(e.Message);
            }

        }

        public async Task UpdateDepartment(DepartmentClass department)
        {
            try
            {
                RestRequest request = new RestRequest("/Department", Method.Put);
                request.AddJsonBody(JsonConvert.SerializeObject(department));
                await restClient.PutAsync(request);
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke rediger afdeling");
                Global.Logger.Error(e.Message);
            }
        }

        public async Task DeleteDepartment(int Id)
        {
            try
            {
                RestResponse res = await restClient.DeleteAsync(new RestRequest("/Department/" + Id, Method.Delete));
            }
            catch (Exception e)
            {
                MessageBox.Show("Kunne ikke slette afdeling");
                Global.Logger.Error(e.Message);
            }

        }
    }
}
