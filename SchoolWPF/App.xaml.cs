using SchoolWPF.View;
using SchoolWPF.ViewModel.Main;
using System.Configuration;
using System.Data;
using System.Windows;

namespace SchoolWPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            //ShutdownMode = ShutdownMode.OnExplicitShutdown;

            var loginWin = new LoginWindow();
            loginWin.DataContext = new LoginViewModel();
            loginWin.Show();
        }
    }
}