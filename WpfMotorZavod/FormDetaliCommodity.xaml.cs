﻿using AbstractShopService.ViewModels;
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
    /// Логика взаимодействия для FormCommodityDetali.xaml
    /// </summary>
    public partial class FormCommodityDetali : Window
    {
        public CommodityDetaliViewModel Model { set { model = value; } get { return model; } }

        private CommodityDetaliViewModel model;

        public FormCommodityDetali()
        {
            InitializeComponent();
            Loaded += FormCommodityDetali_Load;
        }

        private void FormCommodityDetali_Load(object sender, EventArgs e)
        {
            try
            {
                var response = APIClient.GetRequest("api/Detali/GetList");
                if (response.Result.IsSuccessStatusCode)
                {
                    comboBoxDetali.DisplayMemberPath = "DetaliName";
                    comboBoxDetali.SelectedValuePath = "Id";
                    comboBoxDetali.ItemsSource = APIClient.GetElement<List<DetaliViewModel>>(response);
                    comboBoxDetali.SelectedItem = null;
                }
                else
                {
                    throw new Exception(APIClient.GetError(response));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (model != null)
            {
                comboBoxDetali.IsEnabled = false;
                comboBoxDetali.SelectedValue = model.DetaliId;
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
                MessageBox.Show("Выберите заготовку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
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