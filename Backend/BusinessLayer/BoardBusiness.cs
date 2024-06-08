using log4net;
using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.BusinessLayer
{


    internal class BoardBusiness
    {
        private LinkedList<string> users;
        private InputValidator iv = new InputValidator();
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal string Name { get; }

        internal int Id { get; }

        internal string OwnerEmail { get; set; }

        internal LinkedList<string> Users { get => users; }

        internal BoardBusiness(int id, string name, string ownerEmail)
        {
            log.Info("BoardBusiness constructor called");
            if (name == null)
            {
                log.Error("Board name cannot be null");
                throw new ArgumentNullException("Board name cannot be null");
            }

            if (name.Length == 0)
            {
                log.Error("Board name cannot be empty");
                throw new ArgumentException("Board name cannot be empty");
            }

            this.Id = id;
            this.Name = name;
            this.OwnerEmail = ownerEmail;
            this.users = new LinkedList<string>();
            this.users.AddLast(ownerEmail);
            log.Info("Board created successfully");
        }

        internal bool AddUser(string email)
        {
            if (users.Contains(email))
                return false;
            users.AddFirst(email);
            return true;
        }

        internal bool RemoveUser(string email)
        {
            if (!users.Contains(email))
                return false;
            return users.Remove(email);
        }

        internal bool TransferOwnership(string email)
        {
            if (!users.Contains(email))
                return false;
            OwnerEmail = email;
            return true;
        }

        internal bool HasUserJoined(string email)
        {
            // TODO - why contain the same mail several times
            return users.Contains(email);
        }

        internal void AddUsers(LinkedList<string> users)
        {
            foreach (string user in users)
                if (!this.users.Contains(user))
                    this.users.AddFirst(user);
            // this method should only be called by LoadData() in BoardFacade.
        }
    }
}
