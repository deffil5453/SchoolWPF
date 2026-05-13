using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.View;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SchoolWPF.ViewModel
{
    public class AddTeacherViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _lastName;
        private string _firstName;
        private string _middleName;
        private DateTime _birthDate;
        private string _phone;
        private string _email;

        public string LastName { get { return _lastName; } set { _lastName = value; OnPropertyChanged("LastName"); } }
        public string FirstName { get { return _firstName; } set { _firstName = value; OnPropertyChanged("FirstName"); } }
        public string MiddleName { get { return _middleName; } set { _middleName = value; OnPropertyChanged("MiddleName"); } }
        public DateTime BirthDate { get { return _birthDate; } set { _birthDate = value; OnPropertyChanged("BirthDate"); } }
        public string Phone { get { return _phone; } set { _phone = value; OnPropertyChanged("Phone"); } }
        public string Email { get { return _email; } set { _email = value; OnPropertyChanged("Email"); } }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddTeacherViewModel()
        {
            BirthDate = DateTime.Now.AddYears(-30);
            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void OnSave(object parameter)
        {
            if (string.IsNullOrEmpty(LastName) || string.IsNullOrEmpty(FirstName))
            {
                MessageBox.Show("Введите фамилию и имя учителя");
                return;
            }

            Teacher newTeacher = new Teacher();
            newTeacher.LastName = LastName.Trim();
            newTeacher.FirstName = FirstName.Trim();
            newTeacher.MiddleName = MiddleName.Trim();
            newTeacher.HireDate = BirthDate;
            newTeacher.Phone = Phone;
            newTeacher.Email = Email;
            newTeacher.HireDate = DateTime.Now;
            newTeacher.IsClassTeacher = false;

            WpfDbContext context = new WpfDbContext();
            context.Teachers.Add(newTeacher);
            context.SaveChanges();

            Window win = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is AddTeacherWindow);
            if (win != null)
            {
                win.DialogResult = true;
            }
        }

        private void OnCancel(object parameter)
        {
            Window win = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is AddTeacherWindow);
            if (win != null)
            {
                win.DialogResult = false;
            }
        }


    }
}
