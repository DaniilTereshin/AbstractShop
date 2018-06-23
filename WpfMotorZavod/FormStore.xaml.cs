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
    /// Логика взаимодействия для FormStore.xaml
    /// </summary>
    public partial class FormStore : Window
    {

        public int Id { set { id = value; } }

        private int? id;

        public FormStore()
        {
            InitializeComponent();
            Loaded += FormStore_Load;

        }

        private void FormStore_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {
                    var Store = Task.Run(() => APIClient.GetRequestData<StoreViewModel>("api/Store/Get/" + id.Value)).Result;
                    textBoxName.Text = Store.StoreName;
                    dataGridViewStore.ItemsSource = Store.StoreDetalis;
                    dataGridViewStore.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewStore.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewStore.Columns[2].Visibility = Visibility.Hidden;
                    dataGridViewStore.Columns[3].Width = DataGridLength.Auto;

                }
                catch (Exception ex)
                {
                    while (ex.InnerException != null)
                    {
                        ex = ex.InnerException;
                    }
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
            string name = textBoxName.Text;
            Task task;
            if (id.HasValue)
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Store/UpdElement", new StoreBindingModel
                {
                    Id = id.Value,
                    StoreName = name
                }));
            }
            else
            {
                task = Task.Run(() => APIClient.PostRequestData("api/Store/AddElement", new StoreBindingModel
                {
                    StoreName = name
                }));
            }

            task.ContinueWith((prevTask) => MessageBox.Show("Сохранение прошло успешно. Обновите список", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
                TaskContinuationOptions.OnlyOnRanToCompletion);
            task.ContinueWith((prevTask) =>
            {
                var ex = (Exception)prevTask.Exception;
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }, TaskContinuationOptions.OnlyOnFaulted);

            Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}