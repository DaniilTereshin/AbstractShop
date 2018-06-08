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
    /// Логика взаимодействия для FormCreateZakaz.xaml
    /// </summary>
    public partial class FormCreateZakaz : Window
    {
        public FormCreateZakaz()
        {
            InitializeComponent();
            Loaded += FormCreateZakaz_Load;
            comboBoxCommodity.SelectionChanged += comboBoxCommodity_SelectedIndexChanged;
            comboBoxCommodity.SelectionChanged += new SelectionChangedEventHandler(comboBoxCommodity_SelectedIndexChanged);
        }

        private void FormCreateZakaz_Load(object sender, EventArgs e)
        {
            try
            {
                List<ZakazchikViewModel> listC = Task.Run(() => APIClient.GetRequestData<List<ZakazchikViewModel>>("api/Zakazchik/GetList")).Result;
                if (listC != null)
                {
                    comboBoxClient.DisplayMemberPath = "ZakazchikFIO";
                    comboBoxClient.SelectedValuePath = "Id";
                    comboBoxClient.ItemsSource = listC;
                    comboBoxCommodity.SelectedItem = null;
                }

                List<CommodityViewModel> listP = Task.Run(() => APIClient.GetRequestData<List<CommodityViewModel>>("api/Commodity/GetList")).Result;
                if (listP != null)
                {
                    comboBoxCommodity.DisplayMemberPath = "CommodityName";
                    comboBoxCommodity.SelectedValuePath = "Id";
                    comboBoxCommodity.ItemsSource = listP;
                    comboBoxCommodity.SelectedItem = null;
                }
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

        private void CalcSum()
        {
            if (comboBoxCommodity.SelectedItem != null && !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = ((CommodityViewModel)comboBoxCommodity.SelectedItem).Id;
                    CommodityViewModel product = Task.Run(() => APIClient.GetRequestData<CommodityViewModel>("api/Commodity/Get/" + id)).Result;
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * (int)product.Price).ToString();
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

        private void textBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void comboBoxCommodity_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxClient.SelectedItem == null)
            {
                MessageBox.Show("Выберите получателя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (comboBoxCommodity.SelectedItem == null)
            {
                MessageBox.Show("Выберите мебель", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            int ZakazchikId = Convert.ToInt32(comboBoxClient.SelectedValue);
            int CommodityId = Convert.ToInt32(comboBoxCommodity.SelectedValue);
            int count = Convert.ToInt32(textBoxCount.Text);
            int sum = Convert.ToInt32(textBoxSum.Text);
            Task task = Task.Run(() => APIClient.PostRequestData("api/Main/CreateZakaz", new ZakazBindingModel
            {
                ZakazchikId = ZakazchikId,
                CommodityId = CommodityId,
                Count = count,
                Sum = sum
            }));

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