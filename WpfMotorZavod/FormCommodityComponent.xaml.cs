using AbstractShopService.Interfaces;
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
using Unity;
using Unity.Attributes;

namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormCommodityDetali.xaml
    /// </summary>
    public partial class FormCommodityDetali : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public CommodityDetaliViewModel Model { set { model = value; } get { return model; } }

        private readonly IDetaliService service;

        private CommodityDetaliViewModel model;

        public FormCommodityDetali(IDetaliService service)
        {
            InitializeComponent();
            Loaded += FormCommodityDetali_Load;
            this.service = service;
        }

        private void FormCommodityDetali_Load(object sender, EventArgs e)
        {
            List<DetaliViewModel> list = service.GetList();
            try
            {
                if (list != null)
                {
                    comboBoxDetali.DisplayMemberPath = "DetaliName";
                    comboBoxDetali.SelectedValuePath = "Id";
                    comboBoxDetali.ItemsSource = list;
                    comboBoxDetali.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxDetali.IsEnabled = false;
                foreach (DetaliViewModel item in list)
                {
                    if (item.DetaliName == model.DetaliName)
                    {
                        comboBoxDetali.SelectedItem = item;
                    }
                }
                textBoxCount.Text = model.Count.ToString();
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxDetali.SelectedItem == null)
            {
                MessageBox.Show("Выберите компонент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (model == null)
                {
                    model = new CommodityDetaliViewModel
                    {
                        DetaliId = Convert.ToInt32(comboBoxDetali.SelectedValue),
                        DetaliName = comboBoxDetali.Text,
                        Count = Convert.ToInt32(textBoxCount.Text)
                    };
                }
                else
                {
                    model.Count = Convert.ToInt32(textBoxCount.Text);
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
