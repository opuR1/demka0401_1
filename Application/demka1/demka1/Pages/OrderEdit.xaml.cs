using demka1.Models;
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

namespace demka1.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrderEdit.xaml
    /// </summary>
    public partial class OrderEdit : Page
    {
        private PartnerProducts _order;
        private Partners _partner;
        private bool IsNew;
        private gb_de1Entities db = gb_de1Entities.GetContext();
        public OrderEdit(PartnerProducts order)
        {
            InitializeComponent();
            _order = order;
            LoadCMB();
            IsNew = _order == null;
            if (IsNew)
            {
                _partner = new Partners();
            }
            else
            {
                _partner = db.Partners.FirstOrDefault(p => p.Id == _order.PartnerId);
                LoadOrder();
            }
        }
        private void LoadCMB()
        {
            var partnershipTypesList = db.PartnershipTypes.ToList();
            var RegionsList = db.Regions.ToList();
            var CitiesList = db.Cities.ToList();

            cmbPartnerTypes.ItemsSource = partnershipTypesList;
            cmbPartnerTypes.DisplayMemberPath = "TypeName";
            cmbPartnerTypes.SelectedValuePath = "Id";

            cmbRegion.ItemsSource = RegionsList;
            cmbRegion.DisplayMemberPath = "RegionName";
            cmbRegion.SelectedValuePath = "Id";

            cmbCity.ItemsSource = CitiesList;
            cmbCity.DisplayMemberPath = "CityName";
            cmbCity.SelectedValuePath = "Id";

        }
        private void LoadOrder()
        {
            tbPartnerName.Text = _partner.Name;
            tbDirectorLN.Text = _partner.DirectorLN;
            tbDirectorFN.Text = _partner.DirectorFN;
            tbDirectorMN.Text = _partner.DirectorMN;
            tbIndex.Text = _partner.RegisterAddressIndex;
            tbStreet.Text = _partner.RegisterAddressStreet;
            tbHouse.Text = _partner.RegisterAddressHouse;
            tbRating.Text = _partner.Rating.ToString();
            tbPhone.Text = _partner.Phone;
            tbEmail.Text = _partner.Email;

            cmbPartnerTypes.SelectedValue = _partner.PartnershipId.ToString();
            cmbRegion.SelectedValue = _partner.RegionId.ToString();
            cmbCity.SelectedValue = _partner.CityId.ToString();
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbPartnerTypes.SelectedValue != null)
                {
                    _partner.PartnershipId = Convert.ToInt32(cmbPartnerTypes.SelectedValue);
                }
                _partner.Name = tbPartnerName.Text;
                _partner.DirectorLN = tbDirectorLN.Text;
                _partner.DirectorFN = tbDirectorFN.Text;
                _partner.DirectorMN = tbDirectorMN.Text;
                _partner.RegisterAddressIndex = tbIndex.Text;
                
                if (cmbRegion.SelectedValue != null)
                {
                    _partner.RegionId = Convert.ToInt32(cmbRegion.SelectedValue);
               
                }
                if (cmbCity.SelectedValue != null)
                {
                    _partner.CityId = Convert.ToInt32(cmbCity.SelectedValue);
                }
                _partner.RegisterAddressStreet = tbStreet.Text;
                _partner.RegisterAddressHouse = tbHouse.Text;
                if (int.TryParse(tbRating.Text, out int parsedRating))
                {
                    _partner.Rating = parsedRating;
                }

                _partner.Phone = tbPhone.Text;
                _partner.Email = tbEmail.Text;

                if (IsNew)
                {
                    db.Partners.Add(_partner);
                    MessageBox.Show("Партнер успешно добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    db.Entry(_partner).State = EntityState.Modified;
                    MessageBox.Show("Партнер успешно обновлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                db.SaveChanges();
                NavigationService.Navigate(new OrderList());
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}, Детали: {ex.InnerException?.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
