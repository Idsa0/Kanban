using System;
using log4net;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class TaskBusiness
    {
        private int boardId;
        //private int boardOwnerId;
        private InputValidator iv = new InputValidator();

        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal int Id { get; }

        internal int BoardId { get => boardId; }

        internal DateTime CreationTime { get; }

        internal DateTime DueDate { get; set; }

        internal string Title { get; set; }

        internal string Description { get; set; }

        internal int Column { get; }

        internal string AssigneeEmail { get; set; }

        internal TaskBusiness(int id, int boardId, string title, string description, DateTime dueDate, int ordinal, string assignee = "") //, int boardOwnerId
        {
            this.Id = id;
            this.boardId = boardId;
            //this.boardOwnerId = boardOwnerId;
            this.AssigneeEmail = assignee;
            this.Title = title;
            this.DueDate = dueDate;
            this.Description = description;
            CreationTime = DateTime.Now;
            this.Column = ordinal;
        }

        internal TaskBusiness(int id, int boardId, string title, string description, DateTime dueDate,
            int ordinal, DateTime creationTime)
        {
            this.CreationTime = creationTime;

            this.Id = id;
            this.boardId = boardId;
            //this.boardOwnerId = boardOwnerId;
            this.AssigneeEmail = "";
            this.Title = title;
            this.DueDate = dueDate;
            this.Description = description;
            CreationTime = DateTime.Now;
            this.Column = ordinal;
        }

        internal bool SetDueDate(DateTime dueDate)
        {
            log.Info("Got into SetDueDate.");
            if (!iv.ValidateTaskDueDate(dueDate))
            {
                log.Error("Invalid task due date");
                throw new ArgumentException("Invalid task due date");
            }

            log.Info("Success");
            this.DueDate = dueDate;
            return true;
        }

        internal bool SetTaskTitle(string title)
        {
            log.Info("Got into SetTaskTitle.");
            if (!iv.ValidateTaskTitle(title))
            {
                log.Error("invalid TaskDueDate");
                throw new ArgumentException("Invalid task title");
            }

            log.Info("Success");
            this.Title = title;
            return true;
        }

        internal bool SetTaskDescription(string description)
        {
            log.Info("Got into SetTaskDescription.");
            if (!iv.ValidateTaskDescription(description))
            {
                log.Error("invalid description");
                throw new ArgumentException("Invalid task description");
            }

            log.Info("Success");
            this.Description = description;
            return true;
        }

        internal bool Unassign()
        {
            if (this.AssigneeEmail.Equals(""))
                return false;
            this.AssigneeEmail = "";
            return true;
        }

        internal TaskToSend ToSend()
        {
            return new TaskToSend(this.Id, this.CreationTime, this.DueDate, this.Title, this.Description);
        }
    }
}
