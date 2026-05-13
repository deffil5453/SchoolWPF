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

namespace SchoolWPF.ViewModel
{
    public class AddStudentViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext _context;
        private ObservableCollection<Classe> _classes;
        private ObservableCollection<string> _genders;
        private string _lastName = "";
        private string _firstName = "";
        private string _middleName = "";
        private string _selectedGender = "М";
        private DateTime _birthDate = DateTime.Now.AddYears(-14);
        private int _selectedClassId;

        public ObservableCollection<Classe> Classes
        {
            get { return _classes; }
            set { _classes = value; OnPropertyChanged("Classes"); }
        }

        public ObservableCollection<string> Genders
        {
            get { return _genders; }
            set { _genders = value; OnPropertyChanged("Genders"); }
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

        public string SelectedGender
        {
            get { return _selectedGender; }
            set { _selectedGender = value; OnPropertyChanged("SelectedGender"); }
        }

        public DateTime BirthDate
        {
            get { return _birthDate; }
            set { _birthDate = value; OnPropertyChanged("BirthDate"); }
        }

        public int SelectedClassId
        {
            get { return _selectedClassId; }
            set { _selectedClassId = value; OnPropertyChanged("SelectedClassId"); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddStudentViewModel()
        {
            _context = new WpfDbContext();

            // Список для выбора пола
            Genders = new ObservableCollection<string>();
            Genders.Add("М");
            Genders.Add("Ж");

            // Загружаем классы из БД для выпадающего списка
            Classes = new ObservableCollection<Classe>(_context.Classes.ToList());
            if (Classes.Count > 0)
            {
                SelectedClassId = Classes[0].Id;
            }

            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void OnSave(object parameter)
        {
            if (string.IsNullOrWhiteSpace(LastName) || string.IsNullOrWhiteSpace(FirstName) || string.IsNullOrWhiteSpace(MiddleName))
            {
                MessageBox.Show("Введите фамилию, имя ученика или отчество");
                return;
            }

            Student newStudent = new Student();
            newStudent.LastName = LastName.Trim();
            newStudent.FirstName = FirstName.Trim();
            newStudent.MiddleName = MiddleName.Trim();
            newStudent.Gender = SelectedGender;
            newStudent.BirthDate = BirthDate;
            newStudent.ClassId = SelectedClassId;

            _context.Students.Add(newStudent);
            _context.SaveChanges();

            // Закрываем окно с успешным результатом
            Window win = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is AddStudentWindow);
            if (win != null)
            {
                win.DialogResult = true;
            }
        }

        private void OnCancel(object parameter)
        {
            Window win = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is AddStudentWindow);
            if (win != null)
            {
                win.DialogResult = false;
            }
        }
    }
}