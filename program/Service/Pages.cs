using program.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Service
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

        private static MainPage mainPage = null;
        public static MainPage getMainPage()
        {
            if (mainPage == null)
                mainPage = new MainPage();
            return mainPage;
        }

        private static UserPage userPage = null;
        public static UserPage getUserPage()
        {
            if (userPage == null)
                userPage = new UserPage();
            return userPage;
        }

        private static ItemPage itemPage = null;
        public static ItemPage getItemPage()
        {
            if (itemPage == null)
                itemPage = new ItemPage();
            return itemPage;
        }

        private static DepartmentPage departmentPage = null;
        public static DepartmentPage getDepartmentPage()
        {
            if (departmentPage == null)
                departmentPage = new DepartmentPage();
            return departmentPage;
        }
    }
}
