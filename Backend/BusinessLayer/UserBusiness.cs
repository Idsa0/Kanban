using log4net;
using System;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserBusiness
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal int Id { get; }
        internal string Email { get; }
        internal string Password { get; }
        internal bool IsLoggedIn { get; set; }


        internal UserBusiness(int id, string email, string password)
        {
            this.Email = email;
            this.Password = password;
            this.Id = id;
            this.IsLoggedIn = true;
        }

        internal string Login(string password)
        {
            if (password != null && password.Equals(this.Password))
            {
                IsLoggedIn = true;
                log.Info("Success");
                return Email;
            }

            log.Error("Password is incorrect");
            throw new ArgumentException("Password is incorrect");
        }

        internal bool Logout()
        {
            if (IsLoggedIn)
            {
                IsLoggedIn = false;
                log.Info("Success");
                return true;
            }

            log.Error("User is already logged out");
            throw new ArgumentException("User is already logged out");
        }
    }
}
