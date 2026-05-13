using SchoolWPF.AppContext;
using SchoolWPF.Model;
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
    public class AttendanceViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext _context;
        private ObservableCollection<Classe> _classes;
        private Classe _selectedClass;
        private DateTime _selectedDate;
        private int _selectedLessonNumber;
        private ObservableCollection<AttendanceRow> _rows;
        private ObservableCollection<string> _statusOptions;
        private ObservableCollection<int> _lessonNumbers;

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

        public DateTime SelectedDate
        {
            get { return _selectedDate; }
            set { _selectedDate = value; OnPropertyChanged("SelectedDate"); }
        }

        public int SelectedLessonNumber
        {
            get { return _selectedLessonNumber; }
            set { _selectedLessonNumber = value; OnPropertyChanged("SelectedLessonNumber"); }
        }

        public ObservableCollection<AttendanceRow> Rows
        {
            get { return _rows; }
            set { _rows = value; OnPropertyChanged("Rows"); }
        }

        public ObservableCollection<string> StatusOptions
        {
            get { return _statusOptions; }
            set { _statusOptions = value; OnPropertyChanged("StatusOptions"); }
        }

        public ObservableCollection<int> LessonNumbers
        {
            get { return _lessonNumbers; }
            set { _lessonNumbers = value; OnPropertyChanged("LessonNumbers"); }
        }

        public ICommand ShowJournalCommand { get; set; }
        public ICommand SaveCommand { get; set; }

        public AttendanceViewModel()
        {
            _context = new WpfDbContext();

            StatusOptions = new ObservableCollection<string>();
            StatusOptions.Add("");
            StatusOptions.Add("Н");
            StatusOptions.Add("Б");
            StatusOptions.Add("У");

            LessonNumbers = new ObservableCollection<int>();
            for (int i = 1; i <= 8; i++)
            {
                LessonNumbers.Add(i);
            }

            Classes = new ObservableCollection<Classe>(_context.Classes.ToList());
            if (Classes.Count > 0)
            {
                SelectedClass = Classes[0];
            }

            SelectedDate = DateTime.Today;
            SelectedLessonNumber = 1;

            ShowJournalCommand = new RelayCommand(ShowJournal);
            SaveCommand = new RelayCommand(SaveJournal);

            ShowJournal(null);
        }

        private void ShowJournal(object parameter)
        {
            if (SelectedClass == null) return;

            Rows = new ObservableCollection<AttendanceRow>();

            var students = _context.Students
                .Where(s => s.ClassId == SelectedClass.Id)
                .ToList();

            var existingRecords = _context.Attendances
                .Where(a => a.ClassId == SelectedClass.Id
                         && a.Date.Date == SelectedDate.Date
                         && a.LessonNumber == SelectedLessonNumber)
                .ToList();

            foreach (var student in students)
            {
                var record = existingRecords.FirstOrDefault(r => r.StudentId == student.Id);
                var currentStatus = "";
                if (record != null)
                {
                    currentStatus = record.Status;
                }

                Rows.Add(new AttendanceRow
                {
                    StudentId = student.Id,
                    LastName = student.LastName,
                    FirstName = student.FirstName,
                    MiddleName = student.MiddleName,
                    Status = currentStatus
                });
            }
        }

        private void SaveJournal(object parameter)
        {
            if (SelectedClass == null || Rows.Count == 0)
            {
                MessageBox.Show("Выберите класс и дату");
                return;
            }

            var oldRecords = _context.Attendances
                .Where(a => a.ClassId == SelectedClass.Id
                         && a.Date.Date == SelectedDate.Date
                         && a.LessonNumber == SelectedLessonNumber)
                .ToList();
            _context.Attendances.RemoveRange(oldRecords);

            foreach (var row in Rows)
            {
                _context.Attendances.Add(new Attendance
                {
                    StudentId = row.StudentId,
                    ClassId = SelectedClass.Id,
                    Date = SelectedDate,
                    LessonNumber = SelectedLessonNumber,
                    Status = row.Status,
                    RecordedAt = DateTime.Now,
                    RecordedBy = "admin"
                });
            }

            _context.SaveChanges();
            MessageBox.Show("Отметки за " + SelectedLessonNumber + "-й урок сохранены!");
        }
    }

    public class AttendanceRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public int StudentId { get; set; }
        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";

        private string _status = "";
        public string Status
        {
            get { return _status; }
            set { _status = value; OnPropertyChanged("Status"); }
        }
    }
}