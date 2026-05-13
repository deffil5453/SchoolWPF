using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Security.Claims;
using System.Text;
using System.Windows.Input;

namespace SchoolWPF.ViewModel
{
    public class StatisticsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext context;
        private ObservableCollection<Classe> classes;
        private Classe selectedClass;
        private DateTime selectedMonth;
        private ObservableCollection<AttendanceReportRow> reportRows;
        private ObservableCollection<string> _months;
        private ObservableCollection<int> _years;
        private string _selectedMonthName;
        private int _selectedYear;
        public ObservableCollection<string> Months
        {
            get { return _months; }
            set { _months = value; OnPropertyChanged("Months"); }
        }

        public ObservableCollection<int> Years
        {
            get { return _years; }
            set { _years = value; OnPropertyChanged("Years"); }
        }

        public string SelectedMonthName
        {
            get { return _selectedMonthName; }
            set { _selectedMonthName = value; OnPropertyChanged("SelectedMonthName"); }
        }

        public int SelectedYear
        {
            get { return _selectedYear; }
            set { _selectedYear = value; OnPropertyChanged("SelectedYear"); }
        }
        public ObservableCollection<Classe> Classes
        {
            get { return classes; }
            set { classes = value; OnPropertyChanged("Classes"); }
        }

        public Classe SelectedClass
        {
            get { return selectedClass; }
            set { selectedClass = value; OnPropertyChanged("SelectedClass"); }
        }

        public DateTime SelectedMonth
        {
            get { return selectedMonth; }
            set { selectedMonth = value; OnPropertyChanged("SelectedMonth"); }
        }

        public ObservableCollection<AttendanceReportRow> ReportRows
        {
            get { return reportRows; }
            set { reportRows = value; OnPropertyChanged("ReportRows"); }
        }

        public ICommand GenerateReportCommand { get; set; }

        public StatisticsViewModel()
        {
            context = new WpfDbContext();
            AddMonths();

            // ✅ Список лет (текущий + 2 предыдущих)
            Years = new ObservableCollection<int>();
            int currentYear = DateTime.Now.Year;
            for (int i = currentYear; i >= currentYear - 2; i--)
            {
                Years.Add(i);
            }

            // ✅ Устанавливаем текущий месяц и год по умолчанию
            SelectedMonthName = Months[DateTime.Now.Month - 1];
            SelectedYear = currentYear;

            classes = new ObservableCollection<Classe>(context.Classes.ToList());
            if (classes.Count > 0)
            {
                selectedClass = classes[0];
            }
            classes = new ObservableCollection<Classe>(context.Classes.ToList());
            if (classes.Count > 0)
            {
                selectedClass = classes[0];
            }
            selectedMonth = DateTime.Today;
            reportRows = new ObservableCollection<AttendanceReportRow>();
            GenerateReportCommand = new RelayCommand(GenerateReport);
        }

        private void AddMonths()
        {
            Months = new ObservableCollection<string>();
            Months.Add("Январь");
            Months.Add("Февраль");
            Months.Add("Март");
            Months.Add("Апрель");
            Months.Add("Май");
            Months.Add("Июнь");
            Months.Add("Июль");
            Months.Add("Август");
            Months.Add("Сентябрь");
            Months.Add("Октябрь");
            Months.Add("Ноябрь");
            Months.Add("Декабрь");
        }

        private void GenerateReport(object parameter)
        {
            if (selectedClass == null) return;

            reportRows.Clear();

            // ✅ Преобразуем название месяца в номер (1-12)
            int monthNumber = Months.IndexOf(SelectedMonthName) + 1;

            DateTime startDate = new DateTime(SelectedYear, monthNumber, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var students = context.Students
                .Where(s => s.ClassId == selectedClass.Id)
                .ToList();

            var attendances = context.Attendances
                .Where(a => a.ClassId == selectedClass.Id
                         && a.Date >= startDate
                         && a.Date <= endDate)
                .ToList();

            foreach (var student in students)
            {
                AttendanceReportRow row = new AttendanceReportRow();
                row.LastName = student.LastName;
                row.FirstName = student.FirstName;
                row.MiddleName = student.MiddleName;

                int totalLessons = 0;
                int absentCount = 0;
                int sickCount = 0;
                int excusedCount = 0;

                foreach (var record in attendances)
                {
                    if (record.StudentId == student.Id)
                    {
                        totalLessons++;
                        string status = record.Status;
                        if (status == "Н")
                        {
                            absentCount++;
                        }
                        else if (status == "Б")
                        {
                            sickCount++;
                        }
                        else if (status == "У")
                        {
                            excusedCount++;
                        }
                    }
                }

                row.TotalLessons = totalLessons;
                row.AbsentCount = absentCount;
                row.SickCount = sickCount;
                row.ExcusedCount = excusedCount;
                row.RecalculatePercent();

                reportRows.Add(row);
            }
        }
    }
    public class AttendanceReportRow : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        public string LastName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string MiddleName { get; set; } = "";
        public int TotalLessons { get; set; }
        public int AbsentCount { get; set; }
        public int SickCount { get; set; }
        public int ExcusedCount { get; set; }

        private double attendancePercent;
        public double AttendancePercent
        {
            get { return attendancePercent; }
            set { attendancePercent = value; OnPropertyChanged("AttendancePercent"); }
        }

        public void RecalculatePercent()
        {
            if (TotalLessons > 0)
            {
                int presentCount = TotalLessons - AbsentCount - SickCount - ExcusedCount;
                attendancePercent = ((double)presentCount / TotalLessons) * 100.0;
            }
            else
            {
                attendancePercent = 0.0;
            }
            OnPropertyChanged("AttendancePercent");
        }
    }
}
