﻿using System;
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
using AbstractShopService.ViewModels;
using Microsoft.Win32;
namespace WpfMotorZavod
{
    ///  <summary>
    /// Логика взаимодействия для FormMain.xaml
    ///  </summary>
    public partial class FormMain : Window
    {

        public FormMain()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                List<ZakazViewModel> list = Task.Run(() => APIClient.GetRequestData<List<ZakazViewModel>>("api/Main/GetList")).Result;
                if (list != null)
                {
                    dataGridViewMain.ItemsSource = list;
                    dataGridViewMain.Columns[0].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[3].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[5].Visibility = Visibility.Hidden;
                    dataGridViewMain.Columns[1].Width = DataGridLength.Auto;
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

        private void получателиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormZakazchiks();
            form.ShowDialog();
        }

        private void заготовкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormDetalis();
            form.ShowDialog();
        }

        private void мебельToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormCommoditys();
            form.ShowDialog();
        }

        private void базыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormStores();
            form.ShowDialog();
        }

        private void рабочиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormRabochis();
            form.ShowDialog();
        }

        private void пополнитьБазуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormPutOnStore();
            form.ShowDialog();
        }

        private void buttonCreateZakaz_Click(object sender, EventArgs e)
        {
            var form = new FormCreateZakaz();
            form.ShowDialog();
            LoadData();
        }

        private void buttonTakeZakazInWork_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                var form = new FormTakeZakazInWork();
                form.Id = ((ZakazViewModel)dataGridViewMain.SelectedItem).Id;
                form.ShowDialog();
                LoadData();
            }
        }

        private void buttonZakazReady_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((ZakazViewModel)dataGridViewMain.SelectedItem).Id;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/FinishZakaz", new ZakazBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заявки изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonPayZakaz_Click(object sender, EventArgs e)
        {
            if (dataGridViewMain.SelectedItem != null)
            {
                int id = ((ZakazViewModel)dataGridViewMain.SelectedItem).Id;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Main/PayZakaz", new ZakazBindingModel
                {
                    Id = id
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Статус заявки изменен. Обновите список", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void buttonRef_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void прайсМебелиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "doc|*.doc|docx|*.docx"
            };

            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveCommodityPrice", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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
        private void загруженностьБазToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "xls|*.xls|xlsx|*.xlsx"
            };
            if (sfd.ShowDialog() == true)
            {
                string fileName = sfd.FileName;
                Task task = Task.Run(() => APIClient.PostRequestData("api/Report/SaveStoresLoad", new ReportBindingModel
                {
                    FileName = fileName
                }));

                task.ContinueWith((prevTask) => MessageBox.Show("Выполнено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information),
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

        private void заказыПолучателейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var form = new FormZakazchikZakazs();
            form.ShowDialog();
        }
    }
}