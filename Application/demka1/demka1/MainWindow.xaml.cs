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
using demka1.Pages;
using demka1.Models;
namespace demka1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PartnerProducts _order;
        public MainWindow()
        {
            _order = null;
            InitializeComponent();
            MainFrame.Navigate(new OrderList());
        }
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {
            if (e.Content is OrderList)
            {
                tbHeader.Text = "Список заказов";
                btnBack.Visibility = Visibility.Collapsed;
            }
            if (e.Content is OrderEdit)
            {
                tbHeader.Text = "Редактирование/Добавление заказа";
                btnBack.Visibility = Visibility.Visible;
            }
        }
        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.GoBack();
        }
    }
}
