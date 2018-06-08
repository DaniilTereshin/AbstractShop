using AbstractShopService.BindingModels;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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
    /// Логика взаимодействия для FormCommodity.xaml
    /// </summary>
    public partial class FormCommodity : Window
    {
        public int Id { set { id = value; } }

        private int? id;

        private List<CommodityDetaliViewModel> DetaliCommoditys;

        public FormCommodity()
        {
            InitializeComponent();
            Loaded += FormCommodity_Load;
        }

        private void FormCommodity_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var response = APIClient.GetRequest("api/Commodity/Get/" + id.Value);
                    if (response.Result.IsSuccessStatusCode)
                    {
                        var Commodity = APIClient.GetElement<CommodityViewModel>(response);
                        textBoxName.Text = Commodity.CommodityName;
                        textBoxPrice.Text = Commodity.Price.ToString();
                        DetaliCommoditys = Commodity.CommodityDetalis;
                        LoadData();
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
            else
                DetaliCommoditys = new List<CommodityDetaliViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (DetaliCommoditys != null)
                {
                    dataGridViewCommodity.ItemsSource = null;
                    dataGridViewCommodity.ItemsSource = DetaliCommoditys;
                    dataGridViewCommodity.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewCommodity.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewCommodity.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewCommodity.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormCommodityDetali();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.CommodityId = id.Value;
                    DetaliCommoditys.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommodity.SelectedItem != null)
            {
                var form = new FormCommodityDetali();
                form.Model = DetaliCommoditys[dataGridViewCommodity.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    DetaliCommoditys[dataGridViewCommodity.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommodity.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        DetaliCommoditys.RemoveAt(dataGridViewCommodity.SelectedIndex);
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

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (DetaliCommoditys == null || DetaliCommoditys.Count == 0)
            {
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<CommodityDetaliBindingModel> productComponentBM = new List<CommodityDetaliBindingModel>();
                for (int i = 0; i < DetaliCommoditys.Count; ++i)
                {
                    productComponentBM.Add(new CommodityDetaliBindingModel
                    {
                        Id = DetaliCommoditys[i].Id,
                        CommodityId = DetaliCommoditys[i].CommodityId,
                        DetaliId = DetaliCommoditys[i].DetaliId,
                        Count = DetaliCommoditys[i].Count
                    });
                }
                Task<HttpResponseMessage> response;
                if (id.HasValue)
                {
                    response = APIClient.PostRequest("api/Commodity/UpdElement", new CommodityBindingModel
                    {
                        Id = id.Value,
                        CommodityName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CommodityDetalis = productComponentBM
                    });
                }
                else
                {
                    response = APIClient.PostRequest("api/Commodity/AddElement", new CommodityBindingModel
                    {
                        CommodityName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CommodityDetalis = productComponentBM
                    });
                }
                if (response.Result.IsSuccessStatusCode)
                {
                    MessageBox.Show("Сохранение прошло успешно", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                    DialogResult = true;
                    Close();
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

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}