using log4net;
using System;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class InputValidator
    {
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private const int descriptionTitleMaxLength = 50;
        private const int descriptionTitleMinLength = 0;
        private const int descriptionContentMaxLength = 300;

        internal bool ValidateEmail(string email)
        {

            Regex re = new Regex("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
            if (re.IsMatch(email))
                return true;
            log.Error("Invalid user email");
            throw new ArgumentException("Invalid user email");
        }

        internal bool ValidatePassword(string password)
        {
            Regex re = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9]).{6,20}$");

            if (password != null && re.IsMatch(password))
                return true;
            log.Error("Invalid user password");
            throw new ArgumentException("Invalid user password");
        }

        internal bool ValidateBoardName(string name)
        {
            // TODO base this on the functional requirement for board names
            if (string.IsNullOrEmpty(name))
            {
                log.Error("Invalid board name");
                throw new ArgumentException("Invalid board name");
            }
            return true;
        }

        internal bool ValidateTaskTitle(string title)
        {
            if (title != null && title.Length > descriptionTitleMinLength && title.Length <= descriptionTitleMaxLength)
                return true;
            log.Error("Invalid task title");
            throw new ArgumentException("Invalid task title");
        }

        internal bool ValidateTaskDescription(string description)
        {
            // TODO base this on the functional requirement for task descriptions
            if (description != null && description.Length <= descriptionContentMaxLength)
                return true;
            log.Error("Invalid task description");
            throw new ArgumentException("Invalid task description");

        }

        internal bool ValidateTaskDueDate(DateTime dueDate)
        {
            // TODO base this on the functional requirement for due dates
            if (dueDate != null)
                return true;
            log.Error("Invalid task due date");
            throw new ArgumentException("Invalid task due date");
        }
    }
}