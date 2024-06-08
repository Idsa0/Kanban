using log4net;
using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class ColumnBusiness
    {
        private const int MAXCOLUMNORDINAL = 2;
        private const int NOLIMITINTEGER = -1;

        private Dictionary<int, TaskBusiness> tasks;
        //private int userId;
        internal int BoardId { get; }
        //internal int UserId { get; }
        internal int Ordinal { get; }
        internal int Id { get; }

        internal int Limit { get; private set; }

        private TaskDTO _taskDTO;
        private ColumnDTO _columnDTO;
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        internal ColumnBusiness(int id, int boardId, int ordinal, int limit = NOLIMITINTEGER)
        {
            this.tasks = new Dictionary<int, TaskBusiness>();
            this.Id = id;
            //this.userId = userId;
            this.BoardId = boardId;
            this.Ordinal = ordinal;
            this.Limit = limit;
            _taskDTO = new TaskDTO();
            _columnDTO = new ColumnDTO();

        }

        private void DTOUpdate(TaskBusiness t)
        {
            _taskDTO.ChangeValues(t.Id, t.CreationTime, t.DueDate, t.Title, t.Description, t.Column, t.BoardId, t.AssigneeEmail);
        }

        private void DTOUpdate()
        {
            _columnDTO.ChangeValues(Id, Tasks, BoardId, Limit, Ordinal);
        }

        internal bool AddTask(int id, string title, string description, DateTime dueDate, string assignee = "")
        {
            if (IsFull())
            {
                log.Error("Column is full, task can not be added");
                throw new Exception("Column is full, task can not be added");
            }
            TaskBusiness t = new TaskBusiness(id, BoardId, title, description, dueDate, Ordinal, assignee);
            tasks.Add(id, t);
            DTOUpdate(t);
            _taskDTO.InsertMe();
            DTOUpdate();
            _columnDTO.InsertTask(this.Id, t.Id);
            return true;
        }

        internal bool UpdateTaskDueDate(string email, int taskId, DateTime dueDate)
        {
            TaskBusiness t = TaskExistsAndNotDone(taskId);
            if (!t.AssigneeEmail.Equals(email))
            {
                log.Error("Only the assignee can change the task's due date");
                throw new ArgumentException("Only the assignee can change the task's due date");
            }
            if (!t.SetDueDate(dueDate))
            {
                log.Error("Invalid due date");
                throw new ArgumentException("Invalid due date");
            }

            DTOUpdate(t);
            _taskDTO.DueDate = dueDate;
            return true;
        }

        internal bool UpdateTaskTitle(string email, int taskId, string title)
        {
            TaskBusiness t = TaskExistsAndNotDone(taskId);
            if (!t.AssigneeEmail.Equals(email))
            {
                log.Error("Only the assignee can change the task's due date");
                throw new ArgumentException("Only the assignee can change the task's due date");
            }
            if (!TaskExistsAndNotDone(taskId).SetTaskTitle(title))
            {
                log.Error("Invalid title");
                throw new ArgumentException("Invalid title");
            }

            DTOUpdate(t);
            _taskDTO.Title = title;
            return true;
        }

        internal bool UpdateTaskDescription(string email, int taskId, string description)
        {
            TaskBusiness t = TaskExistsAndNotDone(taskId);
            if (!t.AssigneeEmail.Equals(email))
            {
                log.Error("Only the assignee can change the task's description");
                throw new ArgumentException("Only the assignee can change the task's description");
            }
            if (!TaskExistsAndNotDone(taskId).SetTaskDescription(description))
            {
                log.Error("Invalid description");
                throw new ArgumentException("Invalid description");
            }
            DTOUpdate(t);
            _taskDTO.Description = description;
            return true;
        }

        internal TaskBusiness RemoveTask(int taskId)
        {
            TaskBusiness t = TaskExistsAndNotDone(taskId);
            if (t == null)
            {
                log.Error("Could not find undone task");
                throw new ArgumentException("Could not find undone task");
            }

            //TaskBusiness ret = Tasks[taskId];

            if (Tasks.Remove(taskId))
            {
                DTOUpdate(t);
                _taskDTO.DeleteMe();
                DTOUpdate();
                _columnDTO.DeleteTask(t.Id);
                return t;
            }
            log.Error("Could not remove task");
            throw new ArgumentException("Could not remove task");
        }

        internal bool LimitColumn(int limit)
        {
            //TODO
            // may be modified
            //if (limit < tasks.Count)
            //  throw new ArgumentException("column already has more tasks then allowed");

            if (limit < NOLIMITINTEGER || limit == 0)
            {
                log.Error($"Limit must be a positive value or {NOLIMITINTEGER} for no limit");
                throw new ArgumentException("Limit must be a positive value or -1 for no limit");
            }
            this.Limit = limit;
            DTOUpdate();
            _columnDTO.Limit = limit;
            return true;
        }

        private TaskBusiness TaskExistsAndNotDone(int taskId)
        {
            if (!Tasks.TryGetValue(taskId, out var task))
            {
                log.Error("Task not found");
                throw new ArgumentException("Task not found");
            }

            if (task.Column == MAXCOLUMNORDINAL)
            {
                log.Error("Task is done - cannot be changed");
                throw new ArgumentException("Task is done - cannot be changed");
            }

            return task;
        }

        internal bool IsFull()
        {
            return Limit != NOLIMITINTEGER && Tasks.Count >= Limit;
        }

        internal Dictionary<int, TaskBusiness> Tasks { get => tasks; }

        internal List<TaskToSend> SendTasks()
        {
            List<TaskToSend> d = new List<TaskToSend>();
            foreach (TaskBusiness task in Tasks.Values)
                d.Add(task.ToSend());
            return d;
        }

        internal bool AssignTask(int taskID, string email, string emailAssignee)
        {
            if (!Tasks.TryGetValue(taskID, out var task))
            {
                log.Error("Task does not exist");
                throw new ArgumentException("Task does not exist");
            }

            if (task == null)
            {
                log.Error("Task does not exist");
                throw new ArgumentException("Task does not exist");
            }
            if (!task.AssigneeEmail.Equals(email) && !task.AssigneeEmail.Equals(""))
            {
                log.Error("User is not permitted to assign the task");
                throw new InvalidOperationException("User is not permitted to assign the task");
            }

            task.AssigneeEmail = emailAssignee;
            DTOUpdate(task);
            _taskDTO.AssigneeEmail = emailAssignee;
            return true;
        }

        internal TaskBusiness GetTask(int taskId)
        {
            return Tasks[taskId];
        }

        internal void LoadData(LinkedList<int> taskList)
        {
            LinkedList<TaskDTO> tasks = _taskDTO.LoadTasks();
            foreach (TaskDTO t in tasks)
                if (taskList.Contains(t.Id))
                    this.tasks.Add(t.Id, DTOtoBusiness(t));
        }

        private TaskBusiness DTOtoBusiness(TaskDTO taskDTO)
        {
            TaskBusiness task = new TaskBusiness(taskDTO.Id, taskDTO.BoardId, taskDTO.Title, taskDTO.Description, taskDTO.DueDate, taskDTO.ColumnOrdinal, taskDTO.CreationTime);
            task.AssigneeEmail = taskDTO.AssigneeEmail;
            return task;
        }

        internal void DeleteData()
        {
            LinkedList<TaskDTO> tasks = _taskDTO.LoadTasks();
            foreach (TaskDTO t in tasks)
                t.DeleteMe();
            this.tasks = new Dictionary<int, TaskBusiness>();
        }

        internal bool UnassignTasks(string email)
        {
            foreach (TaskBusiness t in tasks.Values)
                if (t.AssigneeEmail == email)
                {
                    t.Unassign();
                    DTOUpdate(t);
                    _taskDTO.AssigneeEmail = "";
                }
            return true;
        }
    }
}
