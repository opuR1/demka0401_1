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
using System.Collections.ObjectModel;

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
        private List<PartnerProducts> _delProducts = new List<PartnerProducts>();
        private ObservableCollection<PartnerProducts> _partnerProductsList;
        public OrderEdit(PartnerProducts order)
        {
            InitializeComponent();
            _order = order;
            LoadCMB();
            //Проверка на то что нам нужно сделать(редактировать или добавить)
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

            InitPartnerProducts();
        }
        //Инициализация списка продукции которую заказал партнер
        private void InitPartnerProducts()
        {
            if(!IsNew && _partner != null)
            {
                var list = db.PartnerProducts.Where(pp => pp.PartnerId == _partner.Id).ToList();

                _partnerProductsList = new ObservableCollection<PartnerProducts>(list);
            }

            dgPartnerProducts.ItemsSource = _partnerProductsList;
        }
        //Загрузка всех данных для комбобоксов
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

            dgColProducts.ItemsSource = db.Products.ToList();

        }
        //Загрузка данных партнера
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
        //Сохранение в бд
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
                    if (parsedRating >= 0)
                    {
                        _partner.Rating = parsedRating;
                    }
                    else
                    {
                        throw new Exception("Рейтинг не может быть отрицательным!");
                        
                    }
                }
                else
                {
                    throw new Exception("Рейтинг должен быть целым числом!");
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
                db.SaveChanges();// Сохранили чтобы у нового партнера появился id

                foreach (var deletedItem in _delProducts)
                {
                    var itemInDB = db.PartnerProducts.Find(deletedItem.Id);

                    if (itemInDB != null)
                    {
                        db.PartnerProducts.Remove(itemInDB);
                    }
                }
                //Обрабатываем заказы из DG и сохраняем их в PartnerProducts
                foreach (var item in _partnerProductsList)
                {
                    if (item.ProductId == 0) continue;

                    item.PartnerId = _partner.Id;
                    if(item.Id == 0)
                    {
                        db.PartnerProducts.Add(item);
                    }
                    else
                    {
                        db.Entry(item).State = EntityState.Modified;
                    }

                }

                db.SaveChanges();
                NavigationService.Navigate(new OrderList());
            }
            catch(Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        //Если нажали Delete для удаления строки в DG, то она удалится из БД, а не только из DG
        private void dgPartnerProducts_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                var selectedItem = dgPartnerProducts.SelectedItem as PartnerProducts;

                if (selectedItem != null)
                {
                    if (selectedItem.Id > 0)
                    {
                        _delProducts.Add(selectedItem);
                    }
                }
            }
        }
    }
}
