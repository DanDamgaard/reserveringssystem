using program.Page;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Class
{
    internal class Pages
    {
        private static LoginPage loginPage = null;
        public static LoginPage getLoginPage()
        {
            if (loginPage == null)
                loginPage = new LoginPage();
            return loginPage;
        }
    }
}
