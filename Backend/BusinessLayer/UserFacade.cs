using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class UserFacade
    {
        private UserDTO _userDTO;
        private Dictionary<string, UserBusiness> users;
        private InputValidator iv;
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private int nextId;

        internal UserFacade()
        {
            users = new Dictionary<string, UserBusiness>();
            iv = new InputValidator();
            nextId = 0;
            _userDTO = new UserDTO();
        }


        internal Dictionary<string, UserBusiness> Users
        {
            get { return users; }
        }


        /// <summary>
        /// This method update the DTO so it will contain the user 	
        /// </summary>		 
        /// <param name="userBusiness">the user we want the DTO to contain</param>		  	 
        private void DTOUpdate(UserBusiness userBusiness) /// <summary>
        {
            _userDTO.ChangeValues(userBusiness.Id, userBusiness.Email, userBusiness.Password);
        }

        /// <summary>
        /// This method Register a new user	
        /// </summary>		 
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <returns>true if was succsesfull register</returns>		 
        /// <exception cref="ArgumentException">Thrown if emails is used or invalid or password invalid</exception>
        internal bool Register(string email, string password)
        {
            // TODO emails are not case sensitive, we didn't do anything about it

            log.Info("Got into Register.");
            if (UserExists(email))
            {
                log.Error("User already exists");
                throw new ArgumentException("User already exists");
            }

            if (!iv.ValidateEmail(email))
            {
                log.Error("Email is invalid");
                throw new ArgumentException("Email is invalid");
            }


            if (!iv.ValidatePassword(password))
            {
                log.Error("Password is invalid");
                throw new ArgumentException("Password is invalid");
            }

            UserBusiness u = new UserBusiness(nextId++, email, password);
            users.Add(email, u);
            DTOUpdate(u);
            _userDTO.InsertMe();
            log.Info("Successfully added User " + email);
            return true;
        }

        /// <summary>
        /// This method login a user	
        /// </summary>		 
        /// <param name="email">email of the user</param>
        /// <param name="password">password of the user</param>
        /// <returns>the email of the user</returns>		
        /// <exception cref="ArgumentException">Thrown if user does not exsist or password does not match </exception>
        internal string Login(string email, string password)
        {
            log.Info("Got into Login.");
            if (UserExists(email))
                return users[email].Login(password);
            log.Error("User does not exist");
            throw new ArgumentException("User does not exist");
        }

        /// <summary>
        /// This method checks if a user logged in
        /// </summary>		 
        /// <param name="email">email of the user</param>
        /// <returns>true if he is logged in or false if not</returns>
        /// <exception cref="ArgumentException">Thrown if user does not exsist </exception>

        internal bool IsLoggedIn(string email)
        {
            if (UserExists(email))
                return users[email].IsLoggedIn;
            log.Error("User does not exist");
            return false;
        }

        /// <summary>
        /// This method logout a user
        /// </summary>		 
        /// <param name="email">email of the user</param>
        /// <returns>true if he is logged out now</returns>
        /// <exception cref="ArgumentException">Thrown if user does not exsist or user was logged out before </exception>
        internal bool Logout(string email)
        {
            log.Info("Got into Logout.");
            if (UserExists(email))
                return users[email].Logout();
            log.Error("User does not exist");
            return false;

        }

        /// <summary>
        /// This method checks if a user exsist
        /// </summary>		 
        /// <param name="email">email of the user</param>
        /// <returns>true if he exsist false if not</returns>
        internal bool UserExists(string email)
        {
            return users.ContainsKey(email);
        }

        internal int GetUserId(string email)
        {
            return users[email].Id;
        }


        /// <summary>
        /// This method load the data of all the users from the DB in the dto
        /// </summary>		 
        internal void LoadData()
        {
            LinkedList<UserDTO> DTOusers = _userDTO.LoadUsers();
            foreach (UserDTO uDTO in DTOusers)
            {
                users.Add(uDTO.Email, DTOtoBusiness(uDTO));
                if (uDTO.Id > nextId)
                    nextId = uDTO.Id + 1;
            }
        }


        /// <summary>
        /// This method transform a UserDTO into a UserBusiness
        /// </summary>		 
        /// <param name="userDTO">the UserDTO</param>
        /// <returns>the userDTO as UserBusiness</returns>
        private UserBusiness DTOtoBusiness(UserDTO userDTO)
        {
            return new UserBusiness(userDTO.Id, userDTO.Email, userDTO.Password);
        }

        /// <summary>
        /// This method Delete the data of all the users from the DB, reset the id's and the current users dictonary
        /// </summary>		 
        internal void DeleteData()
        {
            LinkedList<UserDTO> DTOusers = _userDTO.LoadUsers();
            foreach (UserDTO uDTO in DTOusers)
                uDTO.DeleteMe();
            nextId = 0;
            users = new Dictionary<string, UserBusiness>();
        }
    }
}