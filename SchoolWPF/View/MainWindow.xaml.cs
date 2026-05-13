using SchoolWPF.View;
using SchoolWPF.ViewModel.Main;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SchoolWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //var loginWindow = new LoginWindow();
            //loginWindow.DataContext = new LoginViewModel();
            //loginWindow.ShowDialog(); // Модальное окно: главное не будет активно, пока не войдут
        }
    }
}