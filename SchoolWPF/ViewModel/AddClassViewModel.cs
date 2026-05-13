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
    public class AddClassViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private string _newClassName;
        public string NewClassName
        {
            get { return _newClassName; }
            set { _newClassName = value; OnPropertyChanged("NewClassName"); }
        }

        public ICommand SaveCommand { get; set; }
        public ICommand CancelCommand { get; set; }

        public AddClassViewModel()
        {
            SaveCommand = new RelayCommand(OnSave);
            CancelCommand = new RelayCommand(OnCancel);
        }

        private void OnSave(object parameter)
        {
            try
            {
                if (string.IsNullOrEmpty(NewClassName))
                {
                    MessageBox.Show("Введите название класса");
                    return;
                }

                WpfDbContext context = new WpfDbContext();

                Classe newClass = new Classe();
                newClass.ClassName = NewClassName.Trim();

                context.Classes.Add(newClass);
                context.SaveChanges();

                Window win = null;
                foreach (Window w in Application.Current.Windows)
                {
                    if (w is AddClassWindow)
                    {
                        win = w;
                        break;
                    }
                }
                if (win != null)
                {
                    win.DialogResult = true;
                }
            }
            catch (Microsoft.EntityFrameworkCore.DbUpdateException ex)
            {
                // ✅ Показываем внутреннюю ошибку
                string innerMessage = ex.InnerException != null ? ex.InnerException.Message : "";
                MessageBox.Show("Ошибка сохранения:\n" + innerMessage);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        private void OnCancel(object parameter)
        {
            Window win = null;
            foreach (Window w in Application.Current.Windows)
            {
                if (w is AddClassWindow)
                {
                    win = w;
                    break;
                }
            }
            if (win != null)
            {
                win.DialogResult = false;
            }
        }
    }
}
