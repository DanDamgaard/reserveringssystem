using program.Class;
using program.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for ItemPage.xaml
    /// </summary>
    public partial class ItemPage : Page
    {
        private List<ItemClass> items = new List<ItemClass>();
        private List<string> types = new List<string>();
        private List<string> brands = new List<string>();
        List<DepartmentClass> departments = new List<DepartmentClass>();
        private ItemClass selectedItem;
        private DepartmentClass selectedDepartment;
        public ItemPage()
        {
            InitializeComponent();
            modebox.Text = "Vare";
        }
        private void Onload(object sender, RoutedEventArgs e)
        {
           startUp();
        }

        private async void startUp()
        {
            await GetItems();
            await GetTypes();
            await GetBrands();
        }

        private void NumberValidation(object sender, TextCompositionEventArgs e)
        {
            Regex re = new Regex("[^0-9]+");
            e.Handled = re.IsMatch(e.Text); //hvis der er et match, så sætter den det i textbox
        }

        private async Task GetItems()
        {
            items.Clear();
            items = await Global.Api.GetAllItems();
            ItemDataGrid.ItemsSource = items;

        }
        private async Task GetDepartments()
        {
            departments.Clear();
            Departmentbox.Items.Clear();
            departments = await Global.Api.GetAllDepartments();
            foreach(DepartmentClass d in departments)
            {
                Departmentbox.Items.Add(d.city);
            }
            Departmentbox.Items.Insert(0, "");
            ItemBrandProposalbox.ItemsSource = brands;
            ItemBrandSearchbox.ItemsSource = brands;

        }

        private async Task GetBrands()
        {
            brands.Clear();
            brands = await Global.Api.GetAllItemsBrands();
            brands.Insert(0,"");
            ItemBrandProposalbox.ItemsSource = brands;
            ItemBrandSearchbox.ItemsSource= brands;

        }

        private async Task GetTypes()
        {
            types.Clear();
            types = await Global.Api.GetAllItemsTypes();
            types.Insert(0, "");
            ItemTypeProposalbox.ItemsSource= types;
            ItemTypeSearchbox.ItemsSource= types;
        }

        private void ItemBrandProposalbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemBrandBox.Text = (sender as ComboBox)!.SelectedItem as string;
        }

        private void ItemTypeProposalbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ItemTypeBox.Text = (sender as ComboBox)!.SelectedItem as string;
        }

        private void clearItemBoxes()
        {
            ItemNameBox.Text = "";
            ItemBrandBox.Text = "";
            ItemBrandProposalbox.Text = "";
            ItemTypeBox.Text = "";
            ItemTypeProposalbox.Text = "";
            CreateItemBtn.Visibility = Visibility.Visible;
            EditItemBtn.Visibility = Visibility.Collapsed;
            DeleteItemBtn.Visibility = Visibility.Collapsed;
        }


        private async void CreateItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ItemNameBox.Text))
            {
                MessageBox.Show("Du skal skrive et vare navn");
                return;
            }

            if (items.Any(item => item.name == ItemNameBox.Text))
            {
                MessageBox.Show("Vare navn er brugt");
                return;
            }

            if (string.IsNullOrEmpty(ItemBrandBox.Text))
            {
                MessageBox.Show("Du skal skrive et vare mærke");
                return;
            }

            if (string.IsNullOrEmpty(ItemTypeBox.Text))
            {
                MessageBox.Show("Du skal skrive et vare type");
                return;
            }

            ItemClass newItem = new ItemClass(0, ItemNameBox.Text, ItemBrandBox.Text, ItemTypeBox.Text);

            await Global.Api.CreateItem(newItem);

            clearItemBoxes();
            startUp();

        }

        private async void EditItemBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ItemNameBox.Text))
            {
                MessageBox.Show("Du skal skrive et vare navn");
                return;
            }

            if (items.Where(x => x.name == ItemNameBox.Text).Count() < 1)
            {
                MessageBox.Show("Vare navn er brugt");
                return;
            }

            if (string.IsNullOrEmpty(ItemBrandBox.Text))
            {
                MessageBox.Show("Du skal skrive et vare mærke");
                return;
            }

            if (string.IsNullOrEmpty(ItemTypeBox.Text))
            {
                MessageBox.Show("Du skal skrive et vare type");
                return;
            }

            ItemClass editItem = new ItemClass(selectedItem.id, ItemNameBox.Text, ItemBrandBox.Text, ItemTypeBox.Text);

            await Global.Api.UpdateItem(editItem);
            clearItemBoxes();
            startUp();

        }

        private async void DeleteItemBtn_Click(object sender, RoutedEventArgs e)
        {
            await Global.Api.DeleteItem(selectedItem.id);
            clearItemBoxes();
            startUp();
        }

        private void ItemDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            selectedItem = (ItemClass)ItemDataGrid.SelectedItem;
            CreateDepartmentItemBtn.IsEnabled = true;
            CreateDepartmentItemBtn.Opacity = 1;
            ItemNameBox.Text = selectedItem.name;
            ItemBrandBox.Text = selectedItem.brand;
            ItemTypeBox.Text = selectedItem.type;
            CreateItemBtn.Visibility = Visibility.Collapsed;
            EditItemBtn.Visibility = Visibility.Visible;
            DeleteItemBtn.Visibility = Visibility.Visible;
        }

        private void CancelItemBtn_Click(object sender, RoutedEventArgs e)
        {
            clearItemBoxes();
        }

        private async void modebox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string text = ((sender as ComboBox).SelectedItem as ComboBoxItem)!.Content as string;
            if (text == "Afdelings vare")
            {
                await GetDepartments();
                DepartmentItemStack.Visibility = Visibility.Visible;
                ItemStack.Visibility = Visibility.Collapsed;
            }
            else
            {
                DepartmentItemStack.Visibility = Visibility.Collapsed;
                ItemStack.Visibility= Visibility.Visible;
            }
        }

        private void ItemDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(modebox.Text != "Afdelings vare") return;

            selectedItem = (ItemClass)ItemDataGrid.SelectedItem;

            DepartmentItemLabel.Content = selectedItem.name;
        }

        private void Departmentbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var text = (string)Departmentbox.SelectedValue;
            if (!string.IsNullOrEmpty(text))
            {
                selectedDepartment = departments.Single(x => x.city == (sender as ComboBox)!.SelectedItem as string);
                DepartmentItemCountLabel.Content = "Lager tal: " + selectedDepartment.itemCount;
            }

            if(text == "")
            {
                DepartmentItemCountLabel.Content = "Lager tal: 0";
            }

        }

        private void clearDepartmentItemBoxes()
        {
            Departmentbox.Text = "";
            DepartmentItemLabel.Content = "Vælg en vare";
            DepartmentItemCountLabel.Content = "Lager tal: 0";
            Countbox.Text = "";
            CreateDepartmentItemBtn.IsEnabled = false;
            CreateDepartmentItemBtn.Opacity = 0.5;
        }

        private async void CreateDepartmentItemBtn_Click(object sender, RoutedEventArgs e)
        {
            await Global.Api.CreateDepartmentItem(selectedItem.id, selectedDepartment.id, int.Parse(Countbox.Text));
            MessageBox.Show($"{Countbox.Text} {selectedItem.name} er blevet tildelt til {selectedDepartment.city}");
            await GetDepartments();
            clearDepartmentItemBoxes();
        }

        private void CancelDepartmentItemBtn_Click(object sender, RoutedEventArgs e)
        {
            clearDepartmentItemBoxes();
        }

        private void ItemNameSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var branText = (string)ItemBrandSearchbox.SelectedValue;
            var typeText = (string)ItemTypeSearchbox.SelectedValue;
            if (string.IsNullOrEmpty(ItemNameSearchBox.Text) && string.IsNullOrEmpty(branText) && string.IsNullOrEmpty(typeText))
            {
                ItemDataGrid.ItemsSource = items;
            }
            else
            {
                filter();
            }

        }

        private void filter()
        {
            var data = items.ToList();

            var branText = (string)ItemBrandSearchbox.SelectedValue;
            var typeText = (string)ItemTypeSearchbox.SelectedValue;



            if (!string.IsNullOrEmpty(ItemNameSearchBox.Text))
            {
                data = data.FindAll(x => x.name.ToLower().ToString().Contains(ItemNameSearchBox.Text.ToLower()));
            }

            if (!string.IsNullOrEmpty(branText))
            {
                data = data.FindAll(x => x.brand.ToLower().ToString().Equals(branText.ToLower()));
            }

            if (!string.IsNullOrEmpty(typeText))
            {
                data = data.FindAll(x => x.type.ToLower().ToString().Equals(typeText.ToLower()));
            }


            ItemDataGrid.ItemsSource = data;

        }

        private void ItemBrandSearchbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var branText = (string)ItemBrandSearchbox.SelectedValue;
            var typeText = (string)ItemTypeSearchbox.SelectedValue;
            if(string.IsNullOrEmpty(ItemNameSearchBox.Text) && string.IsNullOrEmpty(branText) && string.IsNullOrEmpty(typeText))
            {
                ItemDataGrid.ItemsSource = items;
            }
            else
            {
                filter();
            }
            
        }

        private void ItemTypeSearchbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var branText = (string)ItemBrandSearchbox.SelectedValue;
            var typeText = (string)ItemTypeSearchbox.SelectedValue;
            if (string.IsNullOrEmpty(ItemNameSearchBox.Text) && string.IsNullOrEmpty(branText) && string.IsNullOrEmpty(typeText))
            {
                ItemDataGrid.ItemsSource = items;
            }
            else
            {
                filter();
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(Service.Pages.getMainPage());
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
