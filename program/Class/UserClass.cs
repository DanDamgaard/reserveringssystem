using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Class
{
    public class UserClass
    {
        public UserClass(int id, string username, string password, int department, string userRole, string token, string departmentName)
        {
            this.id = id;
            this.username = username;
            this.password = password;
            this.department = department;
            this.userRole = userRole;
            this.token = token;
            this.departmentName = departmentName;
        }

        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int department { get; set; }
        public string userRole { get; set; }
        public string token { get; set; } = string.Empty;
        public string departmentName { get; set; } = string.Empty;
    }
}
