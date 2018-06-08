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
    /// Логика взаимодействия для FormStore.xaml
    /// </summary>
    public partial class FormStore : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int Id { set { id = value; } }

        private readonly IStoreService service;

        private int? id;

        public FormStore(IStoreService service)
        {
            InitializeComponent();
            Loaded += FormStore_Load;
            this.service = service;
        }

        private void FormStore_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    StoreViewModel view = service.GetElement(id.Value);
                    if (view != null)
                    {
                        textBoxName.Text = view.StoreName;
                        dataGridViewStore.ItemsSource = view.StoreDetalis;
                        dataGridViewStore.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewStore.Columns[1].Visibility = Visibility.Hidden;
                        dataGridViewStore.Columns[2].Visibility = Visibility.Hidden;
                        dataGridViewStore.Columns[3].Width = DataGridLength.Auto;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                if (id.HasValue)
                {
                    service.UpdElement(new StoreBindingModel
                    {
                        Id = id.Value,
                        StoreName = textBoxName.Text
                    });
                }
                else
                {
                    service.AddElement(new StoreBindingModel
                    {
                        StoreName = textBoxName.Text
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
