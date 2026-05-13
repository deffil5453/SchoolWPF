using SchoolWPF.AppContext;
using SchoolWPF.Model;
using SchoolWPF.ViewModel.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SchoolWPF.ViewModel.Admin
{
    public class AdminScheduleViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private WpfDbContext _context;
        private bool _isAdmin;
        private ObservableCollection<Classe> _classes;
        private Classe _selectedClass;
        private ObservableCollection<Subject> _subjects;
        private Subject _selectedSubject;
        private ObservableCollection<Teacher> _teachers;
        private Teacher _selectedTeacher;
        private ObservableCollection<string> _days;
        private string _selectedDay;
        private ObservableCollection<int> _lessons;
        private int _selectedLesson;
        private ObservableCollection<Schedule> _classSchedule;
        private Schedule _selectedSchedule;


        public bool IsAdmin { get { return _isAdmin; } set { _isAdmin = value; OnPropertyChanged("IsAdmin"); } }
        public ObservableCollection<Classe> Classes { get { return _classes; } set { _classes = value; OnPropertyChanged("Classes"); } }
        public Classe SelectedClass { get { return _selectedClass; } set { _selectedClass = value; OnPropertyChanged("SelectedClass"); LoadSchedule(null); } }
        public ObservableCollection<Subject> Subjects { get { return _subjects; } set { _subjects = value; OnPropertyChanged("Subjects"); } }
        public Subject SelectedSubject { get { return _selectedSubject; } set { _selectedSubject = value; OnPropertyChanged("SelectedSubject"); } }
        public ObservableCollection<Teacher> Teachers { get { return _teachers; } set { _teachers = value; OnPropertyChanged("Teachers"); } }
        public Teacher SelectedTeacher { get { return _selectedTeacher; } set { _selectedTeacher = value; OnPropertyChanged("SelectedTeacher"); } }
        public ObservableCollection<string> Days { get { return _days; } set { _days = value; OnPropertyChanged("Days"); } }
        public string SelectedDay { get { return _selectedDay; } set { _selectedDay = value; OnPropertyChanged("SelectedDay"); } }
        public ObservableCollection<int> Lessons { get { return _lessons; } set { _lessons = value; OnPropertyChanged("Lessons"); } }
        public int SelectedLesson { get { return _selectedLesson; } set { _selectedLesson = value; OnPropertyChanged("SelectedLesson"); } }
        public ObservableCollection<Schedule> ClassSchedule { get { return _classSchedule; } set { _classSchedule = value; OnPropertyChanged("ClassSchedule"); } }
        public Schedule SelectedSchedule { get { return _selectedSchedule; } set { _selectedSchedule = value; OnPropertyChanged("SelectedSchedule"); } }

        public ICommand AddEntryCommand { get; set; }
        public ICommand DeleteEntryCommand { get; set; }
        public ICommand LoadScheduleCommand { get; set; }
        public ICommand EditEntryCommand { get; set; }
        private bool _isEditMode;
        private Schedule _editingSchedule;
        public AdminScheduleViewModel(bool isAdmin)
        {
            _isAdmin = isAdmin;
            _context = new WpfDbContext();
            Classes = new ObservableCollection<Classe>(_context.Classes.ToList());
            Subjects = new ObservableCollection<Subject>(_context.Subjects.ToList());
            Teachers = new ObservableCollection<Teacher>(_context.Teachers.ToList());

            Days = new ObservableCollection<string>();
            Days.Add("Понедельник");
            Days.Add("Вторник");
            Days.Add("Среда");
            Days.Add("Четверг");
            Days.Add("Пятница");
            Days.Add("Суббота");

            Lessons = new ObservableCollection<int>();
            for (int i = 1; i <= 8; i++)
            {
                Lessons.Add(i);
            }
            EditEntryCommand = new RelayCommand(OnEditEntry);
            ClassSchedule = new ObservableCollection<Schedule>();
            AddEntryCommand = new RelayCommand(OnAddEntry);
            DeleteEntryCommand = new RelayCommand(OnDeleteEntry);
            LoadScheduleCommand = new RelayCommand(LoadSchedule);

            if (Classes.Count > 0)
            {
                SelectedClass = Classes[0];
            }
            if (Days.Count > 0)
            {
                SelectedDay = Days[0];
            }
            SelectedLesson = 1;
        }
        private void OnEditEntry(object parameter)
        {
            if (_isAdmin == false)
            {
                MessageBox.Show("Доступно только для администратора");
                return;
            }
            if (SelectedSchedule == null)
            {
                MessageBox.Show("Выберите урок в таблице для редактирования");
                return;
            }

            // ✅ Сохраняем данные выбранной записи ПЕРЕД изменениями
            int classId = SelectedSchedule.ClassId;
            int subjectId = SelectedSchedule.SubjectId;
            int teacherId = SelectedSchedule.TeacherId;
            int dayValue = SelectedSchedule.DayOfWeek;
            int lessonNum = SelectedSchedule.LessonNumber;

            _isEditMode = true;
            _editingSchedule = SelectedSchedule;

            // Теперь безопасно заполняем форму
            SelectedClass = Classes.FirstOrDefault(c => c.Id == classId);
            SelectedSubject = Subjects.FirstOrDefault(s => s.Id == subjectId);
            SelectedTeacher = Teachers.FirstOrDefault(t => t.Id == teacherId);

            string[] days = { "", "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота" };
            if (dayValue >= 1 && dayValue <= 6)
                SelectedDay = days[dayValue];

            SelectedLesson = lessonNum;
        }
        private void LoadSchedule(object parameter)
        {
            if (SelectedClass == null) return;
            ClassSchedule.Clear();
            var schedule = _context.ScheduleEntries
                .Where(s => s.ClassId == SelectedClass.Id)
                .ToList();
            foreach (var item in schedule)
            {
                ClassSchedule.Add(item);
            }
        }

        private void OnAddEntry(object parameter)
        {
            if (_isAdmin == false)
            {
                MessageBox.Show("Доступно только для администратора");
                return;
            }
            if (SelectedClass == null || SelectedSubject == null || SelectedTeacher == null)
            {
                MessageBox.Show("Выберите класс, предмет и учителя");
                return;
            }

            int dayValue = 0;
            if (SelectedDay == "Понедельник") dayValue = 1;
            else if (SelectedDay == "Вторник") dayValue = 2;
            else if (SelectedDay == "Среда") dayValue = 3;
            else if (SelectedDay == "Четверг") dayValue = 4;
            else if (SelectedDay == "Пятница") dayValue = 5;
            else if (SelectedDay == "Суббота") dayValue = 6;

            if (_isEditMode && _editingSchedule != null)
            {
                // ✅ РЕДАКТИРОВАНИЕ: обновляем существующую запись
                _editingSchedule.ClassId = SelectedClass.Id;
                _editingSchedule.SubjectId = SelectedSubject.Id;
                _editingSchedule.TeacherId = SelectedTeacher.Id;
                _editingSchedule.DayOfWeek = dayValue;
                _editingSchedule.LessonNumber = SelectedLesson;

                _context.SaveChanges();
                _isEditMode = false;
                _editingSchedule = null;
                LoadSchedule(null);
                MessageBox.Show("Урок обновлён");
            }
            else
            {
                // ✅ ДОБАВЛЕНИЕ: создаём новую запись
                var newEntry = new Schedule();
                newEntry.ClassId = SelectedClass.Id;
                newEntry.SubjectId = SelectedSubject.Id;
                newEntry.TeacherId = SelectedTeacher.Id;
                newEntry.DayOfWeek = dayValue;
                newEntry.LessonNumber = SelectedLesson;
                newEntry.StartTime = TimeSpan.FromHours(8);
                newEntry.EndTime = TimeSpan.FromHours(9);
                newEntry.AcademicYearId = 1;

                _context.ScheduleEntries.Add(newEntry);
                _context.SaveChanges();
                LoadSchedule(null);
                MessageBox.Show("Урок добавлен в расписание");
            }
        }

        private void OnDeleteEntry(object parameter)
        {
            if (_isAdmin == false) return;
            if (SelectedSchedule == null)
            {
                MessageBox.Show("Выберите урок для удаления");
                return;
            }

            _context.ScheduleEntries.Remove(SelectedSchedule);
            _context.SaveChanges();
            LoadSchedule(null);
            MessageBox.Show("Урок удалён из расписания");
        }

    }
}
