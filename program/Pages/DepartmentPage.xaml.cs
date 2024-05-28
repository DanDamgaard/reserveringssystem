using program.Class;
using program.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace program.Pages
{
    /// <summary>
    /// Interaction logic for DepartmentPage.xaml
    /// </summary>
    public partial class DepartmentPage : Page
    {
        private List<DepartmentClass> departments = new List<DepartmentClass>();
        private List<DepartmentItemClass> items = new List<DepartmentItemClass>();
        private List<DepartmentItemClass> departmentItems = new List<DepartmentItemClass>();
        private DepartmentClass selectedDepartment;
        private DepartmentItemClass selectedItem;
        public DepartmentPage()
        {
            InitializeComponent();
        }

        private async void Onload(object sender, RoutedEventArgs e)
        {
            ItemStatusSearchbox.Items.Insert(0, "");
            if(Global.Login.userRole == "Bruger")
            {
                BackBtn.Visibility = Visibility.Collapsed;
                LogOutBtn.Visibility = Visibility.Visible;
            }
            ModeBox.Text = "Afdelings Vare";
            getDepartmentItems();
            
        }


        private async Task GetDepartmentsButtons()
        {
            departments.Clear();
            DepartmentStack.Children.Clear();
            departments = await Global.Api.GetAllDepartments();
            foreach (DepartmentClass d in departments)
            {
                Button btn = new Button();
                btn.Style = (Style)FindResource("ButtonStyle");
                btn.Content = d.city;
                btn.Width = 200;
                btn.Height = 50;
                Thickness margin = btn.Margin;
                margin.Bottom = 20;
                btn.Margin = margin;
                if (d.id == Global.Login.department)
                {
                    DepartmentStack.Children.Insert(0, btn);
                }
                else
                {
                    DepartmentStack.Children.Add(btn);
                }
                btn.Click += (s, e) =>
                {
                    foreach(Button b in DepartmentStack.Children)
                    {
                        if(b != btn)
                        {
                            b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF008D7A")!;
                            b.MouseEnter += (s, e) =>
                            {
                                b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF17CDB4")!;
                            };
                            b.MouseLeave += (s, e) =>
                            {
                                b.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF008D7A")!;
                            };
                        }
                    }
                    btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF17CDB4")!;
                    btn.MouseLeave += (s, e) =>
                    {
                        btn.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#FF17CDB4")!;
                    };
                    selectedDepartment = d;
                    getDepartmentItemById(d.id);

                };
            }
            DepartmentStack.Children[0].RaiseEvent(new RoutedEventArgs(ButtonBase.ClickEvent));
        }

        private async void getDepartmentItems()
        {
            items.Clear();
            items = await Global.Api.GetAllDepartmentItems();
        }

        private async void getDepartmentItemById(int id)
        {
            departmentItems.Clear();
            departmentItems = await Global.Api.GetAllDepartmentItemsById(id);
            DepartmentItemDataGrid.ItemsSource = departmentItems;
        }

        private async void EditItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if(ItemNoBox.Text == "")
            {
                MessageBox.Show("Du skal skrive et Vare nr");
            }

            if (departmentItems.Where(x => x.itemNo == ItemNoBox.Text).Count() < 1)
            {
                MessageBox.Show("Vare nr er brugt");
                return;
            }

            DepartmentItemClass item = new DepartmentItemClass(selectedItem.id, selectedItem.itemId, selectedItem.itemName,selectedItem.departmentId, ItemStatusbox.Text, CustomerNameBox.Text, CustomerPhoneBox.Text, null, null, "", "");
            if(StartDatoPikcer.Text != "")
            {
                item.startDate = (DateTime)StartDatoPikcer.Value;
            }
            if(EndDatoPikcer.Text != "")
            {
                item.endDate = (DateTime)EndDatoPikcer.Value;
            }

            await Global.Api.UpdateDepartmentItem(item);
            getDepartmentItemById(selectedDepartment.id);
            ItemFilter();
            cancelEditItem();

        }

        private async void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            await Global.Api.DeleteDepartmentItem(selectedItem.id);
            getDepartmentItemById(selectedDepartment.id);
            ItemFilter();
            cancelEditItem();
        }

        private void CancelItemBtn_Click(object sender, RoutedEventArgs e)
        {
            cancelEditItem();
        }

        private void cancelEditItem()
        {
            DepartmentBtnGrid.Visibility = Visibility.Visible;
            EditItemGrid.Visibility = Visibility.Collapsed;
            EditItemScroll.ScrollToTop();
        }

        private void cancelDepartment()
        {
            DepartmentCityBox.Text = string.Empty;
            DepartmentAddressBox.Text = string.Empty;
            CreateDepartmentBtn.Visibility = Visibility.Visible;
            EditDepartmentBtn.Visibility = Visibility.Collapsed;
            DeleteDepartmentBtn.Visibility = Visibility.Collapsed;
        }

        private void DepartmentItemDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DepartmentBtnGrid.Visibility = Visibility.Collapsed;
            EditItemGrid.Visibility = Visibility.Visible;
            selectedItem = (DepartmentItemClass)DepartmentItemDataGrid.SelectedItem;
            ItemNoBox.Text = selectedItem.itemNo;
            ItemStatusbox.Text = selectedItem.status;
            CustomerNameBox.Text = selectedItem.customerName;
            CustomerPhoneBox.Text = selectedItem.customerPhone;
            if(selectedItem.startDateString != "")
            {
                StartDatoPikcer.Value = DateTime.Parse(selectedItem.startDateString);
            }
            else
            {
                StartDatoPikcer.Text = string.Empty;
            }

            if (selectedItem.endDateString != "")
            {
                EndDatoPikcer.Value = DateTime.Parse(selectedItem.endDateString);
            }
            else
            {
                EndDatoPikcer.Text = string.Empty;
            }

        }

        private async Task GetDepartments()
        {
            items.Clear();
            departments.Clear();
            departments = await Global.Api.GetAllDepartments();
            items = await Global.Api.GetAllDepartmentItems();

            foreach (var d in departments)
            {
                d.itemCount = items.Where(x => x != null && x.departmentId == d.id).Count();
            }

            DepartmentDataGrid.ItemsSource = departments;
            DepartmentDataGrid.Items.Refresh();

        }

        private void DepartmentDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedDepartment = (DepartmentClass)DepartmentDataGrid.SelectedItem;
            DepartmentCityBox.Text = selectedDepartment.city;
            DepartmentAddressBox.Text = selectedDepartment.address;
            CreateDepartmentBtn.Visibility = Visibility.Collapsed;
            EditDepartmentBtn.Visibility = Visibility.Visible;
            DeleteDepartmentBtn.Visibility = Visibility.Visible;
        }

        private async void CreateDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentClass newDepartment = new DepartmentClass(0, DepartmentCityBox.Text, DepartmentAddressBox.Text, 0);
            await Global.Api.CreateDepartment(newDepartment);
            await GetDepartments();
            DepartmentFilter();
            cancelDepartment();
        }

        private async void EditDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            DepartmentClass editDepartment = new DepartmentClass(selectedDepartment.id, DepartmentCityBox.Text, DepartmentAddressBox.Text, 0);
            await Global.Api.UpdateDepartment(editDepartment);
            await GetDepartments();
            DepartmentFilter();
            cancelDepartment();
        }

        private async void DeleteDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            await Global.Api.DeleteDepartment(selectedDepartment.id);
            await GetDepartments();
            DepartmentFilter();
            cancelDepartment();
        }

        private void CancelDepartmentBtn_Click(object sender, RoutedEventArgs e)
        {
            cancelDepartment();
        }

        private async void ModeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = ((sender as ComboBox).SelectedItem as ComboBoxItem)!.Content as string;
            if (text == "Afdelings Vare")
            {
                cancelDepartment();
                DepartmentGrid.Visibility = Visibility.Collapsed;
                DepartmentDataGrid.Visibility = Visibility.Collapsed;
                SearchDepartmentStack.Visibility = Visibility.Collapsed;
                await GetDepartmentsButtons();
                SearchItemStack.Visibility = Visibility.Visible;
                DepartmentBtnGrid.Visibility = Visibility.Visible;
                DepartmentItemDataGrid.Visibility = Visibility.Visible;
            }
            else
            {
                cancelEditItem();
                SearchItemStack.Visibility = Visibility.Collapsed;
                DepartmentBtnGrid.Visibility = Visibility.Collapsed;
                DepartmentItemDataGrid.Visibility = Visibility.Collapsed;
                await GetDepartments();
                DepartmentGrid.Visibility = Visibility.Visible;
                DepartmentDataGrid.Visibility = Visibility.Visible;
                SearchDepartmentStack.Visibility = Visibility.Visible;
            }
        }

        private void ItemFilter()
        {
            var data = departmentItems.ToList();

            ComboBoxItem cbi = ItemStatusSearchbox.SelectedItem as ComboBoxItem;
            string status = "";
            if (cbi != null)
            {
                status = (string)cbi.Content;
            }

            if (!string.IsNullOrEmpty(ItemNoSearchBox.Text))
            {
                data = data.FindAll(x => x.itemNo.ToLower().ToString().Contains(ItemNoSearchBox.Text.ToLower()));
            }

            if (!string.IsNullOrEmpty(ItemNameSearchBox.Text))
            {
                data = data.FindAll(x => x.itemName.ToLower().ToString().Contains(ItemNameSearchBox.Text.ToLower()));
            }

            if (!string.IsNullOrEmpty(ItemCustomerSearchBox.Text))
            {
                data = data.FindAll(x => x.customerName.ToLower().ToString().Contains(ItemCustomerSearchBox.Text.ToLower()));
            }

            if (!string.IsNullOrEmpty(ItemPhoneSearchBox.Text))
            {
                data = data.FindAll(x => x.customerPhone.ToLower().ToString().Contains(ItemPhoneSearchBox.Text.ToLower()));
            }

            if (!string.IsNullOrEmpty(status))
            {
                data = data.FindAll(x => x.status.ToLower().ToString().Equals(status.ToLower()));
            }


            DepartmentItemDataGrid.ItemsSource = data;
        }

        private void DepartmentFilter()
        {
            var data = departments.ToList();

            if (!string.IsNullOrEmpty(DepartmentCitySearchBox.Text))
            {
                data = data.FindAll(x => x.city.ToLower().ToString().Contains(DepartmentCitySearchBox.Text.ToLower()));
            }

            if (!string.IsNullOrEmpty(DepartmentAddressSearchBox.Text))
            {
                data = data.FindAll(x => x.address.ToLower().ToString().Contains(DepartmentAddressSearchBox.Text.ToLower()));
            }

            DepartmentDataGrid.ItemsSource = data;
        }

        private void ItemNoSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ComboBoxItem cbi = ItemStatusSearchbox.SelectedItem as ComboBoxItem;
            string status = "";
            if (cbi != null)
            {
                status = (string)cbi.Content;
            }


            if (string.IsNullOrEmpty(ItemNoSearchBox.Text) && string.IsNullOrEmpty(ItemNameSearchBox.Text) && string.IsNullOrEmpty(ItemCustomerSearchBox.Text) && string.IsNullOrEmpty(ItemPhoneSearchBox.Text) && string.IsNullOrEmpty(status))
            {
                DepartmentItemDataGrid.ItemsSource = departmentItems;
            }
            else
            {
                ItemFilter();
            }

        }

        private void ItemStatusSearchbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem cbi = ItemStatusSearchbox.SelectedItem as ComboBoxItem;
            string status = "";
            if (cbi != null) 
            {
                status = (string)cbi.Content;
            }
            

            if (string.IsNullOrEmpty(ItemNoSearchBox.Text) && string.IsNullOrEmpty(ItemNameSearchBox.Text) && string.IsNullOrEmpty(ItemCustomerSearchBox.Text) && string.IsNullOrEmpty(ItemPhoneSearchBox.Text) && string.IsNullOrEmpty(status))
            {
                DepartmentItemDataGrid.ItemsSource = departmentItems;
            }
            else
            {
                ItemFilter();
            }
        }

        private void DepartmentCitySearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(DepartmentCitySearchBox.Text) && string.IsNullOrEmpty(DepartmentAddressSearchBox.Text))
            {
                DepartmentDataGrid.ItemsSource = departments;
            }
            else
            {
                DepartmentFilter();
            }
            
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Service.Pages.getMainPage());
        }

        private void LogOutBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Service.Pages.getLoginPage());
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
