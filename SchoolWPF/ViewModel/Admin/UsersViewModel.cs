using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SchoolWPF.ViewModel.Admin
{
    public class UsersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext _context;
        private bool _isAdmin;
        private ObservableCollection<User> _users;
        private User _selectedUser;
        private ObservableCollection<Role> _roles;
        private Role _selectedRole;
        private ObservableCollection<Teacher> _teachers;
        private Teacher _selectedTeacher;
        private string _newLogin;

        public bool IsAdmin { get { return _isAdmin; } set { _isAdmin = value; OnPropertyChanged("IsAdmin"); } }
        public ObservableCollection<User> Users { get { return _users; } set { _users = value; OnPropertyChanged("Users"); } }
        public User SelectedUser { get { return _selectedUser; } set { _selectedUser = value; OnPropertyChanged("SelectedUser"); } }
        public ObservableCollection<Role> Roles { get { return _roles; } set { _roles = value; OnPropertyChanged("Roles"); } }
        public Role SelectedRole { get { return _selectedRole; } set { _selectedRole = value; OnPropertyChanged("SelectedRole"); } }
        public ObservableCollection<Teacher> Teachers { get { return _teachers; } set { _teachers = value; OnPropertyChanged("Teachers"); } }
        public Teacher SelectedTeacher { get { return _selectedTeacher; } set { _selectedTeacher = value; OnPropertyChanged("SelectedTeacher"); } }
        public string NewLogin { get { return _newLogin; } set { _newLogin = value; OnPropertyChanged("NewLogin"); } }

        public ICommand LoadUsersCommand { get; set; }
        public ICommand CreateUserCommand { get; set; }
        public ICommand DeleteUserCommand { get; set; }

        public UsersViewModel(bool isAdmin)
        {
            _isAdmin = isAdmin;
            _context = new WpfDbContext();
            Roles = new ObservableCollection<Role>(_context.Roles.ToList());
            Teachers = new ObservableCollection<Teacher>(_context.Teachers.ToList());
            LoadUsers(null);
            LoadUsersCommand = new RelayCommand(LoadUsers);
            CreateUserCommand = new RelayCommand(CreateUser);
            DeleteUserCommand = new RelayCommand(DeleteUser);
        }

        private void LoadUsers(object parameter)
        {
            Users = new ObservableCollection<User>(_context.Users.ToList());
        }

        private void CreateUser(object parameter)
        {
            // ✅ Получаем PasswordBox из параметра команды
            PasswordBox passwordBox = parameter as PasswordBox;

            if (_isAdmin == false)
            {
                MessageBox.Show("Доступно только для администратора");
                return;
            }
            if (string.IsNullOrEmpty(NewLogin) || passwordBox == null || string.IsNullOrEmpty(passwordBox.Password))
            {
                MessageBox.Show("Введите логин и пароль");
                return;
            }
            if (_context.Users.Any(u => u.Login == NewLogin))
            {
                MessageBox.Show("Пользователь с таким логином уже существует");
                return;
            }

            User newUser = new User();
            newUser.Login = NewLogin;
            newUser.Password = passwordBox.Password; // ✅ Читаем пароль из PasswordBox
            newUser.RoleId = SelectedRole.Id;
            newUser.IsActive = true;
            newUser.CreatedAt = System.DateTime.Now;
            if (SelectedTeacher != null)
            {
                newUser.TeacherId = SelectedTeacher.Id;
            }

            _context.Users.Add(newUser);
            _context.SaveChanges();
            LoadUsers(null);
            NewLogin = "";
            passwordBox.Password = ""; // ✅ Очищаем поле после создания
            MessageBox.Show("Пользователь создан");
        }

        private void DeleteUser(object parameter)
        {
            if (_isAdmin == false) return;
            if (SelectedUser == null)
            {
                MessageBox.Show("Выберите пользователя для удаления");
                return;
            }

            _context.Users.Remove(SelectedUser);
            _context.SaveChanges();
            LoadUsers(null);
            MessageBox.Show("Пользователь удалён");
        }
    }
}