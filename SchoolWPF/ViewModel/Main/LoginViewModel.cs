using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using SchoolWPF.Model;
using SchoolWPF.Services;
using SchoolWPF.View;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolWPF.ViewModel.Main
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private AuthService _authService;
        private string _login;
        private string _password;
        private string _errorMessage;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged("Login");
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string ErrorMessage
        {
            get { return _errorMessage; }
            set
            {
                _errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        public ICommand LoginCommand { get; set; }
        public ICommand GoToRegisterCommand { get; set; }

        public LoginViewModel()
        {
            _authService = new AuthService();
            LoginCommand = new RelayCommand(OnLoginClicked);
            //GoToRegisterCommand = new RelayCommand(OnGoToRegisterClicked);
        }

        private void OnLoginClicked(object parameter)
        {
            PasswordBox passwordBox = parameter as PasswordBox;

            if (string.IsNullOrEmpty(Login) || passwordBox == null || string.IsNullOrEmpty(passwordBox.Password))
            {
                ErrorMessage = "Заполните логин и пароль";
                return;
            }
            Password = passwordBox.Password;



            User user = _authService.Login(Login, Password);
            if (user != null)
            {
                // ✅ Определяем, является ли пользователь админом
                bool isAdmin = false;
                if (user.RoleId == 1)
                {
                    isAdmin = true;
                }

                // ✅ Передаём флаг в MainViewModel
                //Application.Current.MainWindow.DataContext = new MainViewModel(user.Login, isAdmin);
                var mainWin = new MainWindow();
                mainWin.DataContext = new MainViewModel(user.Login, isAdmin);
                mainWin.Show();
                // ✅ Закрываем окно входа
                var win = Application.Current.Windows
                    .Cast<Window>()
                    .FirstOrDefault(w => w is LoginWindow);
                if (win != null)
                {
                    win.Close();
                }
            }
            else
            {
                ErrorMessage = "Неверный логин или пароль";
            }
        }

        //private void OnGoToRegisterClicked(object parameter)
        //{
        //    var regWin = new RegisterWindow();
        //    regWin.DataContext = new RegisterViewModel();
        //    regWin.Show();

        //    var loginWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(w => w is LoginWindow);
        //    if (loginWindow != null) loginWindow.Close();
        //}

    }
}
