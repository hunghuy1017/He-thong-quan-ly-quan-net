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
using CourseManagerment.Models;

namespace CourseManagerment
{
    /// <summary>
    /// Interaction logic for AddSchedule.xaml
    /// </summary>
    public partial class AddSchedule : Window
    {
        ApContext context = new ApContext();
        public CourseSchedule newSchedule {  get; private set; }
        private int? courseId;
        public AddSchedule(int? courseId)
        {
            this.courseId = courseId;
            InitializeComponent();
            LoadCbData();
        }

        private void LoadCbData()
        {
            cbAddCourse.ItemsSource = context.Courses.ToList();
            cbRoomId.ItemsSource = context.Rooms.ToList();

            if (courseId == null) return;

            cbAddCourse.SelectedValue = courseId;
            cbAddCourse.IsEnabled = false;
        }

        private void btnSaveAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                newSchedule = new CourseSchedule
                {
                    CourseId = this.courseId ?? (int)cbAddCourse.SelectedValue,
                    TeachingDate = dtPicker.SelectedDate??DateTime.Now,
                    Slot = int.Parse(txtSlot.Text),
                    RoomId = (int)cbRoomId.SelectedValue,
                    Description = txtDescription.Text,
                };
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: "+ex.Message);
            }
        }
    }
}
