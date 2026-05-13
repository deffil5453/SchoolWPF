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
    public class ClassesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext _context;
        private ObservableCollection<Classe> _classes;
        private Classe _selectedClass;

        public ObservableCollection<Classe> Classes
        {
            get { return _classes; }
            set { _classes = value; OnPropertyChanged("Classes"); }
        }

        public Classe SelectedClass
        {
            get { return _selectedClass; }
            set { _selectedClass = value; OnPropertyChanged("SelectedClass"); }
        }

        public ICommand LoadCommand { get; set; }
        public ICommand AddCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public ClassesViewModel()
        {
            _context = new WpfDbContext();
            LoadClasses(null);
            LoadCommand = new RelayCommand(LoadClasses);
            AddCommand = new RelayCommand(OnAdd);
            DeleteCommand = new RelayCommand(OnDelete);
        }

        private void LoadClasses(object parameter)
        {
            Classes = new ObservableCollection<Classe>(_context.Classes.ToList());
        }

        private void OnAdd(object parameter)
        {
            var addWindow = new AddClassWindow();
            addWindow.DataContext = new AddClassViewModel();
                
            bool? result = addWindow.ShowDialog();

            if (result == true)
            {
                LoadClasses(null);
            }
        }

        private void OnDelete(object parameter)
        {
            if (SelectedClass == null)
            {
                MessageBox.Show("Выберите класс для удаления");
                return;
            }

            _context.Classes.Remove(SelectedClass);
            _context.SaveChanges();
            LoadClasses(null);
            MessageBox.Show("Класс удалён");
        }
    }
}
