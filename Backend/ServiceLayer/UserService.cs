using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class UserService
    {
        private UserFacade uf;
        public UserService() { }

        internal UserFacade Uf { set { uf = value; } }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Register(string email, string password)
        {
            Response res;
            try
            {
                uf.Register(email, password);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method registers a new user to the system.
        /// </summary>
        /// <param name="email">The user email address, used as the username for logging the system.</param>
        /// <param name="password">The user password.</param>
        /// <returns>A response with the user's email, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Login(string email, string password)
        {
            Response res;
            try
            {
                res = new Response(uf.Login(email, password) as object);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method logs out a logged in user. 
        /// </summary>
        /// <param name="email">The email of the user to log out</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string Logout(string email)
        {
            Response res;
            try
            {
                uf.Logout(email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method checks if a user is logged in.
        /// </summary>
        /// <param name="email">The user email address, must be  a registered</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string IsLoggedIn(string email)
        {
            Response res;
            try
            {
                uf.IsLoggedIn(email);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>null, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="IOException">Thrown if there was a problem when accessing the data</exception>
        public string LoadData()
        {
            Response res;
            try
            {
                uf.LoadData();
                return null;
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b>
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>null, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="IOException">Thrown if there was a problem when accessing the data</exception>
        public string DeleteData()
        {
            Response res;
            try
            {
                uf.DeleteData();
                return null;
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }
    }
}
