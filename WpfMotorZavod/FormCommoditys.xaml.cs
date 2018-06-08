using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
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
using System.Windows.Shapes;
namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormCommoditys.xaml
    /// </summary>
    public partial class FormCommoditys : Window
    {

        public FormCommoditys()
        {
            InitializeComponent();
            Loaded += FormCommoditys_Load;
        }

        private void FormCommoditys_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Commodity/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<CommodityViewModel> list = APIClient.GetElement<List<CommodityViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewCommoditys.ItemsSource = list;
                        dataGridViewCommoditys.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewCommoditys.Columns[1].Width = DataGridLength.Auto;
                        dataGridViewCommoditys.Columns[3].Visibility = Visibility.Hidden;
                    }
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormCommodity();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommoditys.SelectedItem != null)
            {
                var form = new FormCommodity();
                form.Id = ((CommodityViewModel)dataGridViewCommoditys.SelectedItem).Id;
                if (form.ShowDialog() == true)
                    LoadData();
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommoditys.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {

                    int id = ((CommodityViewModel)dataGridViewCommoditys.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Commodity/DelElement", new ZakazchikBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    LoadData();
                }
            }
        }

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}