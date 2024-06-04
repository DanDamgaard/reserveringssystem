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
    /// Interaction logic for LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
            Global.Login = new UserClass(0, "", "", 0, "", "", "");
        }

        private async void LoginBtn_Click(object sender, RoutedEventArgs e)
        {
            loadingGrid.Visibility = Visibility.Visible;
            UserClass user = new UserClass(0, UserNameBox.Text.ToLower(), passwordBox.Password, 0, "Bruger", "","");
            if (await Global.Api.UserLogin(user))
            {
                if (Global.Login.userRole == "Admin")
                {
                    UserNameBox.Text = "";
                    NavigationService.Navigate(Service.Pages.getMainPage());
                }
                else
                {
                    UserNameBox.Text = "";
                    NavigationService.Navigate(Service.Pages.getDepartmentPage());
                }
                loadingGrid.Visibility = Visibility.Collapsed;
            }
            else
            {
                loadingGrid.Visibility = Visibility.Collapsed;
                MessageBox.Show("Forkert login");
            }
        }

        private void passwordBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                LoginBtn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }
        }
    }
}
