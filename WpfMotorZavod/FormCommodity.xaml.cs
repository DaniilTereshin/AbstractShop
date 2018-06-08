using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
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
using Unity;
using Unity.Attributes;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormCommodity.xaml
    /// </summary>
    public partial class FormCommodity : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int ID { set { id = value; } }

        private readonly ICommodityService service;

        private int? id;

        private List<CommodityDetaliViewModel> productDetalis;

        public FormCommodity(ICommodityService service)
        {
            InitializeComponent();
            Loaded += FormCommodity_Load;
            this.service = service;
        }

        private void FormCommodity_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    CommodityViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.CommodityName;
                        textBoxPrice.Text = view.Price.ToString();
                        productDetalis = view.CommodityDetalis;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                productDetalis = new List<CommodityDetaliViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (productDetalis != null)
                {
                    dataGridViewProduct.ItemsSource = null;
                    dataGridViewProduct.ItemsSource = productDetalis;
                    dataGridViewProduct.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewProduct.Columns[3].Width = DataGridLength.Auto;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormCommodityDetali>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.CommodityId = id.Value;
                    productDetalis.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                var form = Container.Resolve<FormCommodityDetali>();
                form.Model = productDetalis[dataGridViewProduct.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    productDetalis[dataGridViewProduct.SelectedIndex] = form.Model;
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewProduct.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        productDetalis.RemoveAt(dataGridViewProduct.SelectedIndex);
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
            if (productDetalis == null || productDetalis.Count == 0)
            {
                MessageBox.Show("Заполните компоненты", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<CommodityDetaliBindingModel> productDetaliBM = new List<CommodityDetaliBindingModel>();
                for (int i = 0; i < productDetalis.Count; ++i)
                {
                    productDetaliBM.Add(new CommodityDetaliBindingModel
                    {
                        Id = productDetalis[i].Id,
                        CommodityId = productDetalis[i].CommodityId,
                        DetaliId = productDetalis[i].DetaliId,
                        Count = productDetalis[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new CommodityBindingModel
                    {
                        Id = id.Value,
                        CommodityName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CommodityDetalis = productDetaliBM
                    });
                }
                else
                {
                    service.AddElement(new CommodityBindingModel
                    {
                        CommodityName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CommodityDetalis = productDetaliBM
                    });
                }
                MessageBox.Show("Сохранение прошло успешно", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
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
