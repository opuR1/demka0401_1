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
        private List<PartnerOrdersVM> _partnerOrders = new List<PartnerOrdersVM>();
        public OrderList()
        {
            InitializeComponent();
            LoadData();

        }
        private void LoadData()
        {
            using (var db = gb_de1Entities.GetContext())
            {
                //_allOrders = db.PartnerProducts.Include(o => o.Partners).Include(o => o.Products).Include(o => o.Partners.Cities)
                //    .Include(o => o.Partners.Regions).Include(o => o.Partners.PartnershipTypes).ToList();
                    
                //lbOrders.ItemsSource = _allOrders;

                //Список всех заказов
                var rawOrders = db.PartnerProducts.Include(o => o.Partners).Include(o => o.Products).Include(o => o.Partners.Cities)
                    .Include(o => o.Partners.Regions).Include(o => o.Partners.PartnershipTypes).ToList();
                //Группируем заказы по партнерам, а после перебираем и трансформируем каждую группу в один объект
                _partnerOrders = rawOrders.GroupBy(o => o.Partners).Select(group => new PartnerOrdersVM
                {
                    Partner = group.Key,
                    Orders = group.ToList(),
                    TotalPrice = group.Sum(o => o.TotalPrice),
                }
                ).ToList();

                lbOrders.ItemsSource = _partnerOrders;
            }                       
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OrderEdit(null));
        }

        private void lbOrders_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var selectedItem = lbOrders.SelectedItem as PartnerOrdersVM;
            if (selectedItem != null)
            {
                NavigationService.Navigate(new OrderEdit(selectedItem.Orders.FirstOrDefault()));
            }
        }
    }
}
