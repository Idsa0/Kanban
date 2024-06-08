using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardDTO
    {
        private BoardController controller;
        private BoardUsersController bu_controller;
        internal const string BoardName = "Name";
        internal const string BoardOwnerEmailName = "OwnerEmail";
        internal const string BoardUsersName = "Users";
        internal const string BoardIdName = "ID";

        internal int Id { get; set; }

        private string name;
        internal string Name
        {
            get => name;
            set
            {
                name = value; controller.Update(Id, BoardName, value);
            }
        }

        private string ownerEmail;
        internal string OwnerEmail
        {
            get => ownerEmail;
            set
            {
                ownerEmail = value; controller.Update(Id, BoardOwnerEmailName, value);
            }
        }

        private LinkedList<string> users;
        internal LinkedList<string> Users
        {
            get => users;
            set
            {
                users = value; controller.Update(Id, BoardUsersName, value);
            }
        }

        internal BoardDTO()
        {
            bu_controller = new BoardUsersController();
            controller = new BoardController();
        }
        internal BoardDTO(int ID, string Name, string OwnerEmail, LinkedList<string> Users)
        {
            Id = ID;
            name = Name;
            ownerEmail = OwnerEmail;
            users = Users;
            controller = new BoardController();
            bu_controller = new BoardUsersController();
        }
        internal BoardDTO(int ID, string Name, string OwnerEmail)
        {
            Id = ID;
            name = Name;
            ownerEmail = OwnerEmail;
            users = new LinkedList<string>();
            controller = new BoardController();
            bu_controller = new BoardUsersController();
        }


        internal Tuple<LinkedList<BoardDTO>, Dictionary<int, LinkedList<string>>> LoadBoards()
        {
            Dictionary<int, LinkedList<string>> u = bu_controller.LoadBoardUsers();
            LinkedList<BoardDTO> DTOs = controller.LoadBoards();
            return Tuple.Create(DTOs, u);
        }

        internal bool InsertMe()
        {
            bool x = true;
            foreach (var u in users)
                if (!bu_controller.Insert(Tuple.Create(Id, u)))
                    x = false;

            return x && controller.Insert(this);
        }
        internal bool DeleteMe() { return bu_controller.DeleteAll(Id) & controller.Delete(this); }

        internal bool Join(string mail)
        {
            return bu_controller.Insert(Tuple.Create(Id, mail));
        }
        internal bool Leave(string mail)
        {
            return bu_controller.RemoveUser(Tuple.Create(Id, mail));
        }

        internal void ChangeValues(int ID, string Name, string OwnerEmail, LinkedList<string> Users)
        {
            Id = ID;
            name = Name;
            ownerEmail = OwnerEmail;
            users = Users;
        }
    }
}