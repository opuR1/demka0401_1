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
using System.Data.Entity;
using demka1.Models;

namespace demka1.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderList.xaml
    /// </summary>
    public partial class OrderList : Page
    {
        private List<PartnerProducts> _allOrders = new List<PartnerProducts>();
        public OrderList()
        {
            InitializeComponent();
            LoadData();

        }
        private void LoadData()
        {
            using (var db = gb_de1Entities.GetContext())
            {
                _allOrders = db.PartnerProducts.Include(o => o.Partners).Include(o => o.Products).Include(o => o.Partners.Cities)
                    .Include(o => o.Partners.Regions).Include(o => o.Partners.PartnershipTypes).ToList();
                    
                lbOrders.ItemsSource = _allOrders;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderEdit(lbOrders.SelectedItem as PartnerProducts));
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderEdit(null));
        }
    }
}
