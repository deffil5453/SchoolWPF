using Microsoft.EntityFrameworkCore;
using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows.Input;

namespace SchoolWPF.ViewModel
{
    public class ScheduleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext _context;
        private ObservableCollection<Classe> _classes;
        private Classe _selectedClass;
        private ObservableCollection<WeeklyScheduleRow> _scheduleRows;

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

        public ObservableCollection<WeeklyScheduleRow> ScheduleRows
        {
            get { return _scheduleRows; }
            set { _scheduleRows = value; OnPropertyChanged("ScheduleRows"); }
        }

        public ICommand LoadScheduleCommand { get; set; }

        public ScheduleViewModel()
        {
            _context = new WpfDbContext();
            Classes = new ObservableCollection<Classe>(_context.Classes.ToList());
            ScheduleRows = new ObservableCollection<WeeklyScheduleRow>();
            LoadScheduleCommand = new RelayCommand(LoadSchedule);
        }

        private void LoadSchedule(object parameter)
        {
            if (SelectedClass == null) return;

            ScheduleRows.Clear();

            var items = _context.ScheduleEntries
                .Include(s => s.Subject)
                .Include(s => s.Teacher)
                .Where(s => s.ClassId == SelectedClass.Id)
                .ToList();

            for (int lesson = 1; lesson <= 8; lesson++)
            {
                WeeklyScheduleRow row = new WeeklyScheduleRow();
                row.LessonNumber = lesson;

                row.Monday = GetCellText(items, lesson, 1);
                row.Tuesday = GetCellText(items, lesson, 2);
                row.Wednesday = GetCellText(items, lesson, 3);
                row.Thursday = GetCellText(items, lesson, 4);
                row.Friday = GetCellText(items, lesson, 5);
                row.Saturday = GetCellText(items, lesson, 6);

                if (row.Monday.Length > 0 || row.Tuesday.Length > 0 || row.Wednesday.Length > 0 || row.Thursday.Length > 0 || row.Friday.Length > 0 || row.Saturday.Length > 0)
                {
                    ScheduleRows.Add(row);
                }
            }
        }

        private string GetCellText(List<Schedule> items, int lessonNumber, int dayOfWeek)
        {
            foreach (var item in items)
            {
                if (item.LessonNumber == lessonNumber && item.DayOfWeek == dayOfWeek)
                {
                    string subject = item.Subject != null ? item.Subject.Name : "";
                    string teacher = item.Teacher != null ? item.Teacher.LastName : "";
                    if (subject.Length > 0 && teacher.Length > 0)
                    {
                        return subject + " (" + teacher + ")";
                    }
                    if (subject.Length > 0)
                    {
                        return subject;
                    }
                    return teacher;
                }
            }
            return "";
        }
    }
    public class WeeklyScheduleRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private int _lessonNumber;
        public int LessonNumber { get { return _lessonNumber; } set { _lessonNumber = value; OnPropertyChanged("LessonNumber"); } }

        private string _monday = "";
        public string Monday { get { return _monday; } set { _monday = value; OnPropertyChanged("Monday"); } }

        private string _tuesday = "";
        public string Tuesday { get { return _tuesday; } set { _tuesday = value; OnPropertyChanged("Tuesday"); } }

        private string _wednesday = "";
        public string Wednesday { get { return _wednesday; } set { _wednesday = value; OnPropertyChanged("Wednesday"); } }

        private string _thursday = "";
        public string Thursday { get { return _thursday; } set { _thursday = value; OnPropertyChanged("Thursday"); } }

        private string _friday = "";
        public string Friday { get { return _friday; } set { _friday = value; OnPropertyChanged("Friday"); } }

        private string _saturday = "";
        public string Saturday { get { return _saturday; } set { _saturday = value; OnPropertyChanged("Saturday"); } }
    }
}