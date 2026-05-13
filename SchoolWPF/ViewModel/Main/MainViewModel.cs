using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.View;
using SchoolWPF.ViewModel.Admin;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace SchoolWPF.ViewModel.Main
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private string _userName;
        private bool _isAdmin;
        private object _currentView;

        public string UserName
        {
            get { return _userName; }
            set { _userName = value; OnPropertyChanged("UserName"); }
        }

        public bool IsAdmin
        {
            get { return _isAdmin; }
            set { _isAdmin = value; OnPropertyChanged("IsAdmin"); }
        }

        public object CurrentView
        {
            get { return _currentView; }
            set { _currentView = value; OnPropertyChanged("CurrentView"); }
        }
        
        public ICommand ShowStudentsCommand { get; set; }
        public ICommand ShowAttendanceCommand { get; set; }
        public ICommand ShowStatisticsCommand { get; set; }
        public ICommand ShowScheduleCommand { get; set; }
        public ICommand ShowAdminScheduleCommand { get; set; }
        public ICommand ShowUsersCommand { get; set; }
        public ICommand ShowTeachersCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand ShowClassesCommand { get; set; }
        public MainViewModel(string loggedInUser, bool isAdmin)
        {
            _userName = loggedInUser;
            _isAdmin = isAdmin;

            var context = new WpfDbContext();
            

            ShowStudentsCommand = new RelayCommand(OnShowStudents);
            ShowAttendanceCommand = new RelayCommand(OnShowAttendance);
            ShowStatisticsCommand = new RelayCommand(OnShowStatistics);
            ShowScheduleCommand = new RelayCommand(OnShowSchedule);
            ShowAdminScheduleCommand = new RelayCommand(OnShowAdminSchedule);
            ShowUsersCommand = new RelayCommand(OnShowUsers);
            ShowTeachersCommand = new RelayCommand(OnShowTeachers);
            LogoutCommand = new RelayCommand(OnLogout);
            ShowClassesCommand = new RelayCommand(OnShowClasses);
            OnShowStudents(null);
        }

        private void OnShowStudents(object parameter) { CurrentView = new StudentsViewModel(); }
        private void OnShowAttendance(object parameter) { CurrentView = new AttendanceViewModel(); }
        private void OnShowStatistics(object parameter) { CurrentView = new StatisticsViewModel(); }
        private void OnShowSchedule(object parameter) { CurrentView = new ScheduleViewModel(); }
        private void OnShowAdminSchedule(object parameter) { CurrentView = new AdminScheduleViewModel(_isAdmin); }
        private void OnShowUsers(object parameter) { CurrentView = new UsersViewModel(_isAdmin); }
        private void OnShowTeachers(object parameter) { CurrentView = new TeachersViewModel(); }
        private void OnShowClasses(object parameter) { CurrentView = new ClassesViewModel(); }
        private void OnLogout(object parameter)
        {
            System.Windows.Application.Current.MainWindow.DataContext = new LoginViewModel();
            // 1. Создаём новое окно входа
            var loginWin = new LoginWindow();
            loginWin.DataContext = new LoginViewModel();
            loginWin.Show();

            // 2. Закрываем текущее главное окно
            var mainWin = System.Windows.Application.Current.Windows
                .Cast<System.Windows.Window>()
                .FirstOrDefault(w => w is MainWindow);

            if (mainWin != null)
            {
                mainWin.Close();
            }
        }
    }
}