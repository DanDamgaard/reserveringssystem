using program.Class;
using program.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Service
{
    public class Global
    {
        public static Api Api { get; set; } = new Api();

        public static UserClass Login { get; set; } = new UserClass(0, "", "", 0, "", "", "");

        public static Logger Logger { get; set; } = new Logger();


    }
}
