using program.Class;
using program.Service;
using System;
using System.Collections;
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
    /// Interaction logic for UserPage.xaml
    /// </summary>
    public partial class UserPage : Page
    {
        private List<UserClass> users = new List<UserClass>();
        private List<DepartmentClass> departments = new List<DepartmentClass>();
        private UserClass SelectedUser;
        
        public UserPage()
        {
            InitializeComponent();
        }
        private async void Onload(object sender, RoutedEventArgs e)
        {
            try
            {
                await GetUsers();
            }
            catch (Exception ex)
            {
                Global.Logger.Error(ex.Message);
                MessageBox.Show("Kunne ikke hente brugere");
                NavigationService.Navigate(Service.Pages.getMainPage());
                return;
            }

        }

        private async Task GetUsers()
        {
            users.Clear();
            await GetDepartemts();
            users = await Global.Api.GetAllUsers();
            UserDataGrid.ItemsSource = users;

        }

        private async Task GetDepartemts()
        {
            DepartmentBox.Items.Clear();
            DepartmentBox.Items.Add("");
            departments = await Global.Api.GetAllDepartments();
            foreach (DepartmentClass department in departments)
            {
                DepartmentBox.Items.Add(department.city);
            }
        }

        

        private void UserDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            SelectedUser = (UserClass)UserDataGrid.SelectedItem;
            UsernameBox.Text = SelectedUser.username;
            RoleBox.Text = SelectedUser.userRole;
            DepartmentBox.Text = SelectedUser.departmentName;
            EditUserBtn.Visibility = Visibility.Visible;
            DeleteUserBtn.Visibility = Visibility.Visible;
            CreateUserBtn.Visibility = Visibility.Collapsed;
        }

        private async void CreateUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(UsernameBox.Text))
            {
                MessageBox.Show("Du skal skrive et bruger navn");
                return;
            }

            if(users.Any(item => item.username == UsernameBox.Text))
            {
                MessageBox.Show("Bruger navn er brugt");
                return;
            }

            if (String.IsNullOrEmpty(PasswordBox.Password))
            {
                MessageBox.Show("Du skal skrive en adgangskode");
                return;
            }
            string username = UsernameBox.Text.ToLower();

            if(RoleBox.Text == "")
            {
                MessageBox.Show("Du skal vælge en bruger rolle");
                return;
            }

            if(DepartmentBox.Text == "")
            {
                MessageBox.Show("Du skal vælge en afdeling");
                return;
            }
            int department = departments.Single(x => x.city == DepartmentBox.Text).id;

            UserClass newUser = new UserClass(0, username, PasswordBox.Password, department, RoleBox.Text, "", "");

            await Global.Api.CreateUser(newUser);
            await GetUsers();
            
            
        }

        private async void EditUserBtn_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(UsernameBox.Text))
            {
                MessageBox.Show("Du skal skrive et bruger navn");
                return;
            }

            if (users.Where(item => item.username == UsernameBox.Text).Count() < 1)
            {
                MessageBox.Show("Bruger navn er brugt");
                return;
            }

            string password = string.IsNullOrEmpty(PasswordBox.Password) ? Global.Login.password : PasswordBox.Password;
            
            string username = UsernameBox.Text.ToLower();

            if (RoleBox.Text == "")
            {
                MessageBox.Show("Du skal vælge en bruger rolle");
                return;
            }

            if (DepartmentBox.Text == "")
            {
                MessageBox.Show("Du skal vælge en afdeling");
                return;
            }
            int department = departments.Single(x => x.city == DepartmentBox.Text).id;

            UserClass newUser = new UserClass(SelectedUser.id, username, password, department, RoleBox.Text, "", "");

            await Global.Api.UpdateUser(newUser);

            await GetUsers();

            clearInput();

        }

        private async void DeleteUserBtn_Click(object sender, RoutedEventArgs e)
        {
            await Global.Api.DeleteUser(SelectedUser.id);

            await GetUsers();

            clearInput();
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Service.Pages.getMainPage());
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            clearInput();
        }

        private void clearInput()
        {
            EditUserBtn.Visibility = Visibility.Collapsed;
            DeleteUserBtn.Visibility = Visibility.Collapsed;
            CreateUserBtn.Visibility = Visibility.Visible;
            UsernameBox.Text = "";
            PasswordBox.Password = "";
            RoleBox.Text = "";
            DepartmentBox.Text = "";
        }

        private void BackBtn_MouseEnter(object sender, MouseEventArgs e)
        {
            object res = Application.Current.FindResource("backButtonGreyIcon");
            BackPic.ImageSource = (ImageSource)res;
        }

        private void BackBtn_MouseLeave(object sender, MouseEventArgs e)
        {
            object res = Application.Current.FindResource("backButtonIcon");
            BackPic.ImageSource = (ImageSource)res;
        }
    }
}
