using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserDTO
    {
        private UserController controller;
        internal const string UserEmailName = "Email";
        internal const string UserpasswordName = "Password";
        internal const string UseridName = "ID";


        internal int Id { get; set; }

        private string email;
        internal string Email
        {
            get => email;
            set
            {
                email = value; ;
            }
        }

        private string password;
        internal string Password
        {
            get => password;
            set
            {
                password = value; controller.Update(Id, UserpasswordName, value);
            }
        }

        internal UserDTO()
        {
            Id = -1;
            email = null;
            password = null;
            controller = new UserController();
        }
        internal UserDTO(int ID, string Email, string Password)
        {
            Id = ID;
            email = Email;
            password = Password;
            controller = new UserController();
        }

        internal LinkedList<UserDTO> LoadUsers() { return controller.LoadUsers(); }

        internal bool InsertMe() { return controller.Insert(this); }
        internal bool DeleteMe() { return controller.Delete(this); }

        internal void ChangeValues(int ID, string Email, string Password)
        {
            Id = ID;
            email = Email;
            password = Password;
        }
    }
}
