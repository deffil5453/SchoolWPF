using Microsoft.EntityFrameworkCore;
using SchoolWPF.AppContext;
using SchoolWPF.Model;
using System;
using System.Collections.Generic;
using System.Security.RightsManagement;
using System.Text;

namespace SchoolWPF.Services
{
    public class AuthService
    {
        private WpfDbContext _dbCpntext;
        public AuthService()
        {
            _dbCpntext = new WpfDbContext();
        }
        public User Login(string login, string password)
        {
            // Убрали пробелы при сравнении через Trim()
            return _dbCpntext.Users.FirstOrDefault(u =>
                u.Login.Trim() == login.Trim() &&
                u.Password.Trim() == password.Trim() &&
                u.IsActive == true);
        }        
    }
}