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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfApp1.Models;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for AddComputerDialog.xaml
    /// </summary>
    public partial class AddComputerDialog : Window
    {
        public AddComputerDialog()
        {
            InitializeComponent();
            LoadComputerTypes();
            cbStatus.SelectedIndex = 0;
        }

        private void LoadComputerTypes()
        {
            using var context = new NetManagementContext();
            cbType.ItemsSource = context.ComputerTypes.ToList();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            string name = txtName.Text.Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("Tên máy tính không được để trống.");
                return;
            }

            using var context = new NetManagementContext();
            var isDuplicate = context.Computers.Any(c => c.ComputerName.ToLower() == name.ToLower());
            if (isDuplicate)
            {
                MessageBox.Show("Tên máy tính đã tồn tại.");
                return;
            }

            var selectedTypeId = cbType.SelectedValue as int?;
            if (selectedTypeId == null)
            {
                MessageBox.Show("Vui lòng chọn loại máy.");
                return;
            }

            var status = (cbStatus.SelectedItem as ComboBoxItem)?.Content?.ToString();

            var comp = new Computer
            {
                ComputerName = name,
                TypeId = selectedTypeId.Value,
                Status = status
            };

            context.Computers.Add(comp);
            context.SaveChanges();
            this.DialogResult = true;
        }


        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
