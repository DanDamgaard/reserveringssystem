using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Class
{
    public class DepartmentClass
    {
        public DepartmentClass(int id, string city, string address, int itemCount)
        {
            this.id = id;
            this.city = city;
            this.address = address;
            this.itemCount = itemCount;
        }

        public int id { get; set; }
        public string city { get; set; }
        public string address { get; set; }
        public int itemCount { get; set; }

    }
}
