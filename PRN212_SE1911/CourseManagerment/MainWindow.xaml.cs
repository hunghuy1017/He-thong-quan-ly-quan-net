using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CourseManagerment.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseManagerment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ApContext context = new ApContext();
        public MainWindow()
        {
            InitializeComponent();
            LoadCourse();
            LoadData();
        }
        private void LoadData(int? courseId=null)
        {
            var schedule = new List<CourseSchedule>();
            if (courseId != null)
            {
                schedule = context.CourseSchedules.Where(sc => sc.CourseId == courseId).ToList();
                dgSchedule.ItemsSource = schedule;
            }
            else {
                schedule = context.CourseSchedules.ToList();
                dgSchedule.ItemsSource = schedule;
            }
           
        }
        private void LoadCourse()
        {
            var courses = context.Courses.Include(c => c.Subject).ToList();
            cbCourse.ItemsSource = courses;
            cbCourse.DisplayMemberPath = "CourseCode";
            cbCourse.SelectedValuePath = "CourseId";
        }
    private void cbCourse_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
            if (cbCourse.SelectedValue is int courseId)
            {
                LoadData(courseId);
            }
            //{
            //    var result = context.CourseSchedules.Where(sc => sc.CourseId == courseId).ToList();
            //    dgSchedule.ItemsSource = result;
            //}
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var addSchedule = new AddSchedule((int?)cbCourse.SelectedValue);
            if (addSchedule.ShowDialog() == true)
            {
                context.CourseSchedules.Add(addSchedule.newSchedule);
                context.SaveChanges();
                LoadData();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectSchdeule = dgSchedule.SelectedItem as CourseSchedule;
            if (selectSchdeule == null) {
                MessageBox.Show("Please select a course Schedule to edit!");
                return;
            }
            var editSchedule = new EditSchedule(selectSchdeule);
            if (editSchedule.ShowDialog() == true)
            {
                var updated = editSchedule.updateSchedule;
                var old = context.CourseSchedules.Find(updated.TeachingScheduleId);
                if (old != null)
                {
                    old.TeachingScheduleId = updated.TeachingScheduleId;
                    old.CourseId = updated.CourseId;
                    old.TeachingDate = updated.TeachingDate;
                    old.Slot = updated.Slot;
                    old.RoomId = updated.RoomId;
                    old.Description = updated.Description;
                    context.SaveChanges();
                    int? selectedCourse = cbCourse.SelectedValue as int?;
                    if (selectedCourse != null)
                    {
                        LoadData(selectedCourse);
                    }
                    else
                    {
                        LoadData();
                    }
                }
            }
        }
    }
}