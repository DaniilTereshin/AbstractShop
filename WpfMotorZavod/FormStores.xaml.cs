﻿using AbstractShopService.BindingModels;
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
    /// Логика взаимодействия для FormStores.xaml
    /// </summary>
    public partial class FormStores : Window
    {

        public FormStores()
        {
            InitializeComponent();
            Loaded += FormStores_Load;
        }

        private void FormStores_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                var response = APIClient.GetRequest("api/Store/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    List<StoreViewModel> list = APIClient.GetElement<List<StoreViewModel>>(response);
                    if (list != null)
                    {
                        dataGridViewStores.ItemsSource = list;
                        dataGridViewStores.Columns[0].Visibility = Visibility.Hidden;
                        dataGridViewStores.Columns[1].Width = DataGridLength.Auto;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = new FormStore();
            if (form.ShowDialog() == true)
                LoadData();
        }

        private void buttonUpd_Click(object sender, EventArgs e)
        {
            if (dataGridViewStores.SelectedItem != null)
            {
                var form = new FormStore();
                form.Id = ((StoreViewModel)dataGridViewStores.SelectedItem).Id;
                if (form.ShowDialog() == true)
                {
                    LoadData();
                }
            }
        }

        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridViewStores.SelectedItem != null)
            {
                if (MessageBox.Show("Удалить запись?", "Внимание",
                    MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    int id = ((StoreViewModel)dataGridViewStores.SelectedItem).Id;
                    try
                    {
                        var response = APIClient.PostRequest("api/Store/DelElement", new ZakazchikBindingModel { Id = id });
                        if (!response.Result.IsSuccessStatusCode)
                        {
                            throw new Exception(APIClient.GetError(response));
                        }
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
    }
}