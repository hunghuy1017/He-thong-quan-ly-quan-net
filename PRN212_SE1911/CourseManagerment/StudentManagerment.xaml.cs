using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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
    /// Interaction logic for StudentManagerment.xaml
    /// </summary>
    public partial class StudentManagerment : Window
    {
        
            public StudentManagerment()
            {
                InitializeComponent();
            }

            public void LoadStudent()
            {
                using (var newContext = new ApContext())
                {
                    var students = newContext.Students.ToList();
                    dvgDisplay.ItemsSource = students;
                }
            }

            private void Window_Loaded(object sender, RoutedEventArgs e)
            {
                LoadStudent();
            }

            private void btnAdd(object sender, RoutedEventArgs e)
            {
                using (var newContext = new ApContext())
                {
                    Student student = new Student
                    {
                        Roll = txtRool.Text,
                        FirstName = txtFirstName.Text,
                        LastName = txtLastName.Text,
                        MidName = txtMidName.Text
                    };
                Console.WriteLine("StudentId before add: " + student.StudentId);
                newContext.Students.Add(student);
                    newContext.SaveChanges();
                }
                   ResetFields();
                  LoadStudent();
            }
        

        private void btnEdit(object sender, RoutedEventArgs e)
        {
            
        }
        private void btnDelete(object sender, RoutedEventArgs e)
        {
           
            if(dvgDisplay.SelectedItem != null)
            {
                var selectedstudent = dvgDisplay.SelectedItem as Student;
                using (var newcontext= new ApContext())
                {
                    var student = newcontext.Students.Find(selectedstudent.StudentId);
                    if(student != null)
                    {
                        newcontext.Students.Remove(student);
                        newcontext.SaveChanges();
                        LoadStudent();
                        MessageBox.Show("Student deleted successfully.");

                    }
                    else
                    {
                        MessageBox.Show("Student not found.");
                    }   
            
                }   
            }
            else
            {
                MessageBox.Show("Please select a student to delete.");
            }
        }
        private void ResetFields()
        {
            txtStudentID.Text = "";
            txtRool.Text = "";
            txtFirstName.Text = "";
            txtMidName.Text = "";
            txtLastName.Text = "";
        }
        private void btnReset(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }
    }
}
