using program.Class;
using program.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace program.Pages
{
    /// <summary>
    /// Interaction logic for MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void UserBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Service.Pages.getUserPage());
        }

        private void DepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Service.Pages.getDepartmentPage());
        }

        private void ItemBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Service.Pages.getItemPage());
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            Global.Login = new UserClass(0, "", "", 0, "", "", "");
            NavigationService.Navigate(Service.Pages.getLoginPage());
        }
    }
}
