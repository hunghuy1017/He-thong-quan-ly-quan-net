using System.Windows;
using CourseManagerment.Models;

namespace CourseManagerment
{
    /// <summary>
    /// Interaction logic for EditSchedule.xaml
    /// </summary>
    public partial class EditSchedule : Window
    {
        ApContext context = new ApContext();
        CourseSchedule oldSchedule;
        public CourseSchedule updateSchedule { get;private set; }
        public EditSchedule(CourseSchedule oldSchedule)
        {
            InitializeComponent();
            LoadCBData();
            this.oldSchedule = oldSchedule;
            LoadFormData();
        }

        private void LoadFormData()
        {
            txtTeachingScheduleId.Text = oldSchedule.TeachingScheduleId.ToString();
            cbEditCourse.SelectedValue = oldSchedule.CourseId;
            dtPicker.SelectedDate = oldSchedule.TeachingDate;
            txtSlot.Text = oldSchedule.Slot.ToString();
            cbRoomId.SelectedValue = oldSchedule.RoomId;
            txtDescription.Text = oldSchedule.Description;
        }

        private void LoadCBData()
        {
            cbEditCourse.ItemsSource = context.Courses.ToList();
            cbRoomId.ItemsSource = context.Rooms.ToList();
        }

        private void btnSaveEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                updateSchedule = new CourseSchedule
                {
                    TeachingScheduleId = oldSchedule.TeachingScheduleId,
                    CourseId = (int)cbEditCourse.SelectedValue,
                    TeachingDate = dtPicker.SelectedDate,
                    Slot = int.Parse(txtSlot.Text),
                    RoomId = (int)cbRoomId.SelectedValue,
                    Description = txtDescription.Text,
                };
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }
    }
}
