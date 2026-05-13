using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.View;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SchoolWPF.ViewModel.Admin
{
    public class TeachersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext _context;
        private ObservableCollection<Teacher> _teachers;
        private Teacher _selectedTeacher;
        private string _lastName;
        private string _firstName;
        private string _middleName;
        private DateTime _birthDate;
        private string _phone;
        private string _email;

        public ObservableCollection<Teacher> Teachers
        {
            get { return _teachers; }
            set { _teachers = value; OnPropertyChanged("Teachers"); }
        }

        public Teacher SelectedTeacher
        {
            get { return _selectedTeacher; }
            set { _selectedTeacher = value; OnPropertyChanged("SelectedTeacher"); }
        }

        public string LastName
        {
            get { return _lastName; }
            set { _lastName = value; OnPropertyChanged("LastName"); }
        }

        public string FirstName
        {
            get { return _firstName; }
            set { _firstName = value; OnPropertyChanged("FirstName"); }
        }

        public string MiddleName
        {
            get { return _middleName; }
            set { _middleName = value; OnPropertyChanged("MiddleName"); }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; OnPropertyChanged("BirthDate"); }
        }

        public string Phone
        {
            get { return _phone; }
            set { _phone = value; OnPropertyChanged("Phone"); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged("Email"); }
        }

        public ICommand LoadTeachersCommand { get; set; }
        public ICommand AddTeacherCommand { get; set; }
        public ICommand DeleteTeacherCommand { get; set; }

        public TeachersViewModel()
        {
            _context = new WpfDbContext();
            LoadTeachers(null);
            LoadTeachersCommand = new RelayCommand(LoadTeachers);
            AddTeacherCommand = new RelayCommand(AddTeacher);
            DeleteTeacherCommand = new RelayCommand(DeleteTeacher);
            BirthDate = DateTime.Now.AddYears(30);
        }

        private void LoadTeachers(object parameter)
        {
            Teachers = new ObservableCollection<Teacher>(_context.Teachers.ToList());
        }

        private void AddTeacher(object parameter)
        {
            var addWindow = new AddTeacherWindow();
            addWindow.DataContext = new AddTeacherViewModel();

            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                LoadTeachers(null);
            }
        }

        private void DeleteTeacher(object parameter)
        {
            if (SelectedTeacher == null)
            {
                MessageBox.Show("Выберите учителя для удаления");
                return;
            }

            var linkedUser = _context.Users.FirstOrDefault(u => u.TeacherId == SelectedTeacher.Id);
            if (linkedUser != null)
            {
                MessageBox.Show("Нельзя удалить: учитель привязан к учётной записи. Сначала удалите пользователя.");
                return;
            }

            _context.Teachers.Remove(SelectedTeacher);
            _context.SaveChanges();
            LoadTeachers(null);
            MessageBox.Show("Учитель удалён");
        }
    }
}