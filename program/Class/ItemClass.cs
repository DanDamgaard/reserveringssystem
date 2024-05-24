using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Class
{
    public class ItemClass
    {
        public ItemClass(int id, string name, string brand, string type)
        {
            this.id = id;
            this.name = name;
            this.brand = brand;
            this.type = type;
        }

        public int id { get; set; }
        public string name { get; set; }
        public string brand { get; set; }
        public string type { get; set; }
    }
}
