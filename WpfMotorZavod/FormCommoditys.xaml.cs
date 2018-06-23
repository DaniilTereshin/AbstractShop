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
                List<CommodityViewModel> list = Task.Run(() => APIClient.GetRequestData<List<CommodityViewModel>>("api/Commodity/GetList")).Result;
                if (list != null)
                {
                    dataGridViewCommoditys.ItemsSource = list;
                    dataGridViewCommoditys.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewCommoditys.Columns[1].Width = DataGridLength.Auto;
                    dataGridViewCommoditys.Columns[3].Visibility = Visibility.Hidden;
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

                    Task task = Task.Run(() => APIClient.PostRequestData("api/Commodity/DelElement", new ZakazchikBindingModel { Id = id }));

                    task.ContinueWith((prevTask) => MessageBox.Show("Запись удалена. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
                }
            }
        }
        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }
    }
}