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
using AbstractShopService.Interfaces;
using AbstractShopService.ViewModels;
using Unity;
using Unity.Attributes;
namespace WpfMotorZavod
{
    /// <summary>
    /// Логика взаимодействия для FormPutOnStore.xaml
    /// </summary>
    public partial class FormPutOnStore : Window
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        private readonly IStoreService serviceB;

        private readonly IDetaliService serviceZ;

        private readonly IMainService serviceG;

        public FormPutOnStore(IStoreService serviceB, IDetaliService serviceZ, IMainService serviceG)
        {
            InitializeComponent();
            Loaded += FormPutOnStore_Load;
            this.serviceB = serviceB;
            this.serviceZ = serviceZ;
            this.serviceG = serviceG;
        }

        private void FormPutOnStore_Load(object sender, EventArgs e)
        {
            try
            {
                List<DetaliViewModel> listZ = serviceZ.GetList();
                if (listZ != null)
                {
                    comboBoxDetali.DisplayMemberPath = "DetaliName";
                    comboBoxDetali.SelectedValuePath = "Id";
                    comboBoxDetali.ItemsSource = listZ;
                    comboBoxDetali.SelectedItem = null;
                }
                List<StoreViewModel> listB = serviceB.GetList();
                if (listB != null)
                {
                    comboBoxStore.DisplayMemberPath = "StoreName";
                    comboBoxStore.SelectedValuePath = "Id";
                    comboBoxStore.ItemsSource = listB;
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
                serviceG.PutDetaliOnStore(new StoreDetaliBindingModel
                {
                    DetaliId = Convert.ToInt32(comboBoxDetali.SelectedValue),
                    StoreId = Convert.ToInt32(comboBoxStore.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Информация",
                    MessageBoxButton.OK, MessageBoxImage.Information);
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
