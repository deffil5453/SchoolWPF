using Microsoft.EntityFrameworkCore;
using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.View;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace SchoolWPF.ViewModel
{
    public class StudentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private WpfDbContext _dbcontext;
        private ObservableCollection<Student> _students;
        private Student _selectedStudent;

        public ObservableCollection<Student> Students
        {
            get { return _students; }
            set { _students = value; OnPropertyChanged("Students"); }
        }

        public Student SelectedStudent
        {
            get { return _selectedStudent; }
            set { _selectedStudent = value; OnPropertyChanged("SelectedStudent"); }
        }

        public ICommand LoadStudentsCommand { get; set; }
        public ICommand AddStudentCommand { get; set; }
        public ICommand DeleteStudentCommand { get; set; }

        public StudentsViewModel()
        {
            _dbcontext = new WpfDbContext();
            LoadStudentsCommand = new RelayCommand(LoadStudents);
            AddStudentCommand = new RelayCommand(AddStudent);
            DeleteStudentCommand = new RelayCommand(DeleteStudent);

            LoadStudents(null);
        }

        private void LoadStudents(object parameter)
        {
            Students = new ObservableCollection<Student>(
                _dbcontext.Students
                    .Include(s => s.Class)
                    .ToList()
            );
        }

        private void AddStudent(object parameter)
        {
            var addWindow = new AddStudentWindow();
            addWindow.DataContext = new AddStudentViewModel();

            // ShowDialog блокирует интерфейс, пока окно открыто
            bool? result = addWindow.ShowDialog();

            // Если результат true (нажата кнопка Сохранить), обновляем список
            if (result == true)
            {
                LoadStudents(null);
            }
        }

        private void DeleteStudent(object parameter)
        {
            if (SelectedStudent != null)
            {
                _dbcontext.Students.Remove(SelectedStudent);
                _dbcontext.SaveChanges();
                LoadStudents(null);
            }
        }
    }
}