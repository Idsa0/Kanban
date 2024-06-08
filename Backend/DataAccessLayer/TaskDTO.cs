using System;
using System.Collections.Generic;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskDTO
    {
        private TaskController controller;
        internal const string TaskCreationTimeName = "CreationTime";
        internal const string TaskDueDateName = "DueDate";
        internal const string TaskTitleName = "Title";
        internal const string TaskDescriptionName = "Description";
        internal const string TaskColumnOrdinalName = "ColumnOrdinal";
        internal const string TaskBoardIdName = "BoardID";
        internal const string TaskAssigneeEmailName = "AssigneeEmail";
        internal const string TaskIdName = "ID";


        internal int Id { get; set; }

        private DateTime creationTime;
        internal DateTime CreationTime
        {
            get => creationTime;
            set
            {
                creationTime = value; controller.Update(Id, TaskCreationTimeName, value);
            }
        }

        private DateTime dueDate;
        internal DateTime DueDate
        {
            get => dueDate;
            set
            {
                dueDate = value; controller.Update(Id, TaskDueDateName, value);
            }
        }

        private string title;
        internal string Title
        {
            get => title;
            set
            {
                title = value; controller.Update(Id, TaskTitleName, value);
            }
        }

        private string description;
        internal string Description
        {
            get => description;
            set
            {
                description = value; controller.Update(Id, TaskDescriptionName, value);
            }
        }

        private int columnOrdinal;
        internal int ColumnOrdinal
        {
            get => columnOrdinal;
            set
            {
                columnOrdinal = value; controller.Update(Id, TaskColumnOrdinalName, value);
            }
        }

        private int boardId;
        internal int BoardId
        {
            get => boardId;
            set
            {
                boardId = value; controller.Update(Id, TaskBoardIdName, value);
            }
        }

        private string assigneeEmail;
        internal string AssigneeEmail
        {
            get => assigneeEmail;
            set
            {
                assigneeEmail = value; controller.Update(Id, TaskAssigneeEmailName, value);
            }
        }
        internal TaskDTO()
        {
            controller = new TaskController();
        }

        internal TaskDTO(int ID, DateTime CreationTime, DateTime DueDate, string Title
            , string Description, int ColumnOrdinal, int BoardId, string AssigneeEmail)
        {
            Id = ID;
            creationTime = CreationTime;
            dueDate = DueDate;
            title = Title;
            description = Description;
            columnOrdinal = ColumnOrdinal;
            boardId = BoardId;
            assigneeEmail = AssigneeEmail;
            controller = new TaskController();
        }
        internal LinkedList<TaskDTO> LoadTasks() { return controller.LoadTasks(); }

        internal bool InsertMe() { return controller.Insert(this); }
        internal bool DeleteMe() { return controller.Delete(this); }

        internal void ChangeValues(int ID, DateTime CreationTime, DateTime DueDate, string Title,
            string Description, int ColumnOrdinal, int BoardId, string AssigneeEmail)
        {
            this.Id = ID;
            this.creationTime = CreationTime;
            this.dueDate = DueDate;
            this.title = Title;
            this.description = Description;
            this.columnOrdinal = ColumnOrdinal;
            this.boardId = BoardId;
            this.assigneeEmail = AssigneeEmail;
        }
    }
}
