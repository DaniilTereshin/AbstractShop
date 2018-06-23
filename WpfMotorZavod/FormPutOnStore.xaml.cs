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
    /// Логика взаимодействия для FormPutOnStore.xaml
    /// </summary>
    public partial class FormPutOnStore : Window
    {
        public FormPutOnStore()
        {
            InitializeComponent();
            Loaded += FormPutOnStore_Load;
        }

        private void FormPutOnStore_Load(object sender, EventArgs e)
        {
            try
            {
                List<DetaliViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<DetaliViewModel>>("api/Detali/GetList")).Result;
                if (listC != null)
                {
                    comboBoxDetali.DisplayMemberPath = "DetaliName";
                    comboBoxDetali.SelectedValuePath = "Id";
                    comboBoxDetali.ItemsSource = listC;
                    comboBoxDetali.SelectedItem = null;
                }
                List<StoreViewModel> listS = Task.Run(() => APIClient.GetRequestData<List<StoreViewModel>>("api/Store/GetList")).Result;
                if (listS != null)
                {
                    comboBoxStore.DisplayMemberPath = "StoreName";
                    comboBoxStore.SelectedValuePath = "Id";
                    comboBoxStore.ItemsSource = listS;
                    comboBoxStore.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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
                MessageBox.Show("Выберите заготовку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxStore.SelectedItem == null)
            {
                MessageBox.Show("Выберите базу", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            try
            {
                int DetaliId = Convert.ToInt32(comboBoxDetali.SelectedValue);
                int StoreId = Convert.ToInt32(comboBoxStore.SelectedValue);
                int count = Convert.ToInt32(textBoxCount.Text);
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PutDetaliOnStore", new StoreDetaliBindingModel
                {
                    DetaliId = DetaliId,
                    StoreId = StoreId,
                    Count = count
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("База пополнен", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information),
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
            catch (Exception ex)
            {
                while (ex.InnerException != null)
                {
                    ex = ex.InnerException;
                }
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