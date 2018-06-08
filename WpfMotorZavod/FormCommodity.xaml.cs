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
using AbstractShopService.BindingModels;
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
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

        public int Id { set { id = value; } }

        private readonly ICommodityService service;

        private int? id;

        private List<CommodityDetaliViewModel> CommodityDetalis;

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
                        CommodityDetalis = view.CommodityDetalis;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                CommodityDetalis = new List<CommodityDetaliViewModel>();
        }

        private void LoadData()
        {
            try
            {
                if (CommodityDetalis != null)
                {
                    dataGridViewCommodity.ItemsSource = null;
                    dataGridViewCommodity.ItemsSource = CommodityDetalis;
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
            var form = Container.Resolve<FormCommodityDetali>();
            if (form.ShowDialog() == true)
            {
                if (form.Model != null)
                {
                    if (id.HasValue)
                        form.Model.CommodityId = id.Value;
                    CommodityDetalis.Add(form.Model);
                }
                LoadData();
            }
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewCommodity.SelectedItem != null)
            {
                var form = Container.Resolve<FormCommodityDetali>();
                form.Model = CommodityDetalis[dataGridViewCommodity.SelectedIndex];
                if (form.ShowDialog() == true)
                {
                    CommodityDetalis[dataGridViewCommodity.SelectedIndex] = form.Model;
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
                        CommodityDetalis.RemoveAt(dataGridViewCommodity.SelectedIndex);
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
            if (CommodityDetalis == null || CommodityDetalis.Count == 0)
            {
                MessageBox.Show("Заполните заготовки", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                List<CommodityDetaliBindingModel> productComponentBM = new List<CommodityDetaliBindingModel>();
                for (int i = 0; i < CommodityDetalis.Count; ++i)
                {
                    productComponentBM.Add(new CommodityDetaliBindingModel
                    {
                        Id = CommodityDetalis[i].Id,
                        CommodityId = CommodityDetalis[i].CommodityId,
                        DetaliId = CommodityDetalis[i].DetaliId,
                        Count = CommodityDetalis[i].Count
                    });
                }
                if (id.HasValue)
                {
                    service.UpdElement(new CommodityBindingModel
                    {
                        Id = id.Value,
                        CommodityName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CommodityDetalis = productComponentBM
                    });
                }
                else
                {
                    service.AddElement(new CommodityBindingModel
                    {
                        CommodityName = textBoxName.Text,
                        Price = Convert.ToInt32(textBoxPrice.Text),
                        CommodityDetalis = productComponentBM
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
