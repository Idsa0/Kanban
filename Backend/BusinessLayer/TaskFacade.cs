using log4net;
using System;
using System.Collections.Generic;
using IntroSE.Kanban.Backend.DataAccessLayer;


namespace IntroSE.Kanban.Backend.BusinessLayer
{
    internal class TaskFacade
    {
        private const int MAXCOLUMNORDINAL = 2;
        private const int BACKLOGORDINAL = 0;
        private const int INPROGRESSORDINAL = 1;
        private const int DONEORDINAL = 2;

        private Dictionary<int, ColumnBusiness> columns;
        private InputValidator iv;
        private int nextTaskId;
        private int nextColumnId;

        private BoardFacade bf;
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ColumnDTO _columnDTO;

        internal TaskFacade(BoardFacade bf)
        {
            columns = new Dictionary<int, ColumnBusiness>();
            iv = new InputValidator();
            nextTaskId = 0;
            nextColumnId = 0;
            this.bf = bf;
            _columnDTO = new ColumnDTO();
        }

        private void DTOUpdate(ColumnBusiness c)
        {
            _columnDTO.ChangeValues(c.Id, c.Tasks, c.BoardId, c.Limit, c.Ordinal);
        }

        internal bool CreateBoard(string email, string boardName)
        {
            BoardBusiness b = bf.CreateBoard(email, boardName);
            ColumnBusiness c = null;
            for (int i = 0; i <= MAXCOLUMNORDINAL; i++)
            {
                c = new ColumnBusiness(nextColumnId, b.Id, i);
                if (!columns.TryAdd(nextColumnId++, c))
                {
                    // this block should be unreachable
                    log.Error("Cannot create column");
                    throw new ArgumentException("Cannot create column");
                }
                DTOUpdate(c);
                _columnDTO.InsertMe();
            }

            return true;
        }

        internal bool DeleteBoard(string email, string boardName)
        {
            BoardBusiness b = bf.GetBoard(email, boardName);
            foreach (ColumnBusiness column in columns.Values)
                if (column.BoardId == b.Id)
                {
                    if (!columns.Remove(column.Id))
                    {
                        // this block should be unreachable
                        log.Error("Could not delete columns");
                        throw new ArgumentException("Could not delete columns");
                    }
                    DTOUpdate(column);
                    _columnDTO.DeleteMe();
                }
            return bf.DeleteBoard(email, boardName);
        }

        internal List<int> GetUserOwnedBoards(string email)
        {
            return bf.GetUserOwnedBoards(email);
        }

        internal List<int> GetUserJoinedBoards(string email)
        {
            return bf.GetUserJoinedBoards(email);
        }

        internal bool JoinBoard(string email, int boardID)
        {
            return bf.JoinBoard(email, boardID);
        }

        internal bool LeaveBoard(string email, int boardID)
        {
            string name = bf.GetBoardById(boardID).Name;
            ColumnBusiness c;
            for (int i = 0; i <= MAXCOLUMNORDINAL; i++)
            {
                c = GetColumnBusiness(email, name, i);
                c.UnassignTasks(email);
            }

            return bf.LeaveBoard(email, boardID);
        }

        internal string GetBoardName(int boardId)
        {
            return bf.GetBoardName(boardId);
        }

        internal bool TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            return bf.TransferOwnership(currentOwnerEmail, newOwnerEmail, boardName);
        }

        internal List<TaskToSend> GetColumn(string email, string boardName, int columnOrdinal)
        {
            log.Info("GetColumn called");

            int boardId = bf.GetIdByEmailAndName(email, boardName);

            foreach (ColumnBusiness c in columns.Values)
                if (c.BoardId == boardId && c.Ordinal == columnOrdinal)
                    return c.SendTasks();

            log.Error("Column does not exist");
            throw new ArgumentException("Column does not exist");
        }

        internal ColumnBusiness GetColumnBusiness(string email, string boardName, int columnOrdinal)
        {
            log.Info("GetColumn called");

            int boardId = bf.GetIdByEmailAndNameForJoinedUser(email, boardName);

            foreach (ColumnBusiness c in columns.Values)
                if (c.BoardId == boardId && c.Ordinal == columnOrdinal)
                    return c;
            log.Error("Column does not exist");
            throw new ArgumentException("Column does not exist");
        }

        internal string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            log.Info("GetColumnName called");
            switch (columnOrdinal)
            {
                case BACKLOGORDINAL:
                    return "Backlog";
                case INPROGRESSORDINAL:
                    return "In Progress";
                case DONEORDINAL:
                    return "Done";
                default:
                    {
                        log.Error("Invalid column ordinal");
                        throw new ArgumentException("Invalid column ordinal");
                    }
            }
        }

        internal int GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            log.Info("GetColumnLimit called");
            foreach (ColumnBusiness c in columns.Values)
                if (c.Ordinal == columnOrdinal && c.BoardId == bf.GetIdByEmailAndName(email, boardName))
                    return c.Limit;
            log.Error("Column does not exist");
            throw new ArgumentException("Column does not exist");
        }

        internal bool LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            log.Info("LimitColumn called");
            ValidUserAndBoard(email, boardName);
            foreach (ColumnBusiness c in columns.Values)
                if (c.Ordinal == columnOrdinal && c.BoardId == bf.GetIdByEmailAndName(email, boardName))
                {
                    c.LimitColumn(limit);
                    DTOUpdate(c);
                    _columnDTO.Limit = limit;
                    return true;
                }
            log.Error("Column does not exist");
            throw new ArgumentException("Column does not exist");
        }

        internal List<TaskToSend> InProgressTasks(string email)
        {
            log.Info("InProgressTasks called");
            if (!bf.UserFacade.UserExists(email))
            {
                log.Error("User does not exist");
                throw new ArgumentException("User does not exist");
            }

            List<TaskToSend> inProgressTasks = new List<TaskToSend>();

            foreach (ColumnBusiness c in columns.Values)
                if (c.Ordinal == 1 && bf.GetBoardById(c.BoardId).HasUserJoined(email))
                    inProgressTasks.AddRange(c.SendTasks());
            return inProgressTasks;
        }

        internal bool AddTask(string email, string boardName, string title, string description, DateTime dueDate, int column = 0)
        {
            log.Info("AddTask called");
            iv.ValidateTaskTitle(title);
            iv.ValidateTaskDescription(description);
            iv.ValidateTaskDueDate(dueDate);
            ColumnBusiness c = GetColumnBusiness(email, boardName, 0);
            c.AddTask(nextTaskId, title, description, dueDate);
            //DTOUpdate(c);
            //_columnDTO.InsertTask(c.Id, nextTaskId);
            log.Info("Successfully added task " + nextTaskId++ + "for board " + boardName + ", user " + email);
            return true;
        }

        internal bool UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            log.Info("UpdateTaskDueDate called");
            iv.ValidateTaskDueDate(dueDate);
            ValidateUser(email);
            ColumnBusiness c = GetColumnBusiness(email, boardName, columnOrdinal);
            return c.UpdateTaskDueDate(email, taskId, dueDate);
        }

        internal bool UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            log.Info("UpdateTaskTitle called");
            iv.ValidateTaskTitle(title);
            ValidateUser(email);
            ColumnBusiness c = GetColumnBusiness(email, boardName, columnOrdinal);
            return c.UpdateTaskTitle(email, taskId, title);
        }

        internal bool UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            log.Info("UpdateTaskDescription called");
            iv.ValidateTaskDescription(description);
            ValidateUser(email);
            ColumnBusiness c = GetColumnBusiness(email, boardName, columnOrdinal);
            return c.UpdateTaskDescription(email, taskId, description);
        }

        internal bool AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            log.Info("AdvanceTask called");
            if (columnOrdinal >= MAXCOLUMNORDINAL || columnOrdinal < BACKLOGORDINAL)
            {
                log.Error("Illegal column");
                throw new ArgumentException("Illegal column");
            }
            ColumnBusiness next = GetColumnBusiness(email, boardName, columnOrdinal + 1);
            ColumnBusiness curr = GetColumnBusiness(email, boardName, columnOrdinal);
            TaskBusiness task = curr.GetTask(taskId);

            if (next.IsFull())
            {
                log.Error("Column is full, task cannot be advanced");
                throw new InvalidOperationException("Column is full, task cannot be advanced");
            }
            if (!task.AssigneeEmail.Equals(email))
            {
                log.Error("User is not assigned to the task");
                throw new InvalidOperationException("User is not assigned to the task");
            }
            if (curr.RemoveTask(taskId) == null)
            {
                log.Error("Failed to remove task");
                throw new ArgumentException("Failed to remove task");
            }
            return next.AddTask(task.Id, task.Title, task.Description, task.DueDate, task.AssigneeEmail);
        }

        internal bool AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            log.Info("AssignTask called");
            ValidUserAndBoard(email, boardName);
            if (!bf.UserFacade.UserExists(emailAssignee))
            {
                log.Error("Assignee does not exist");
                throw new ArgumentException("Assignee does not exist");
            }
            if (columnOrdinal >= MAXCOLUMNORDINAL || columnOrdinal < BACKLOGORDINAL)
                throw new ArgumentException("Illegal column");
            BoardBusiness b = bf.GetBoardForJoinedUser(email, boardName);
            if (!b.HasUserJoined(emailAssignee))
            {
                log.Error("Assingee has not joined the board");
                throw new InvalidOperationException("Assingee has not joined the board");
            }
            ColumnBusiness c = GetColumnBusiness(email, boardName, columnOrdinal);
            return c.AssignTask(taskID, email, emailAssignee);
        }

        //////////////////////
        /// help functions ///
        //////////////////////

        internal bool ValidateUser(string email)
        {
            if (!bf.UserFacade.UserExists(email))
            {
                log.Error("User does not exist");
                throw new ArgumentException("User does not exist");
            }
            return true;
        }

        internal bool ValidUserAndBoard(string email, string boardName)
        {
            if (!bf.UserFacade.UserExists(email))
            {
                log.Error("User does not exist");
                throw new ArgumentException("User does not exist");
            }
            if (!bf.UserFacade.Users[email].IsLoggedIn)
            {
                log.Error("User is not logged in");
                throw new InvalidOperationException("User is not logged in");
            }

            Tuple<string, string> key = Tuple.Create(email, boardName);

            if (!bf.Boards.TryGetValue(key, out BoardBusiness b))
            {
                log.Error("Board is not exist");
                throw new ArgumentException("Board does not exist");
            }

            return true;
        }

        internal void LoadData()
        {
            Tuple<LinkedList<ColumnDTO>, Dictionary<int, LinkedList<int>>> x = _columnDTO.LoadColumns();
            foreach (ColumnDTO c in x.Item1)
            {
                columns.Add(c.Id, DTOtoBusiness(c));
                if (columns.TryGetValue(c.Id, out ColumnBusiness col))
                {
                    if (x.Item2.TryGetValue(c.Id, out LinkedList<int> t))
                        col.LoadData(t);
                    if (c.Id > nextColumnId)
                        nextColumnId = c.Id + 1;
                }
            }
            bf.LoadData();
        }

        private ColumnBusiness DTOtoBusiness(ColumnDTO columnDTO)
        {
            ColumnBusiness c = new ColumnBusiness(columnDTO.Id, columnDTO.BoardId, columnDTO.Ordinal, columnDTO.Limit);
            //c.LoadData(new LinkedList<int>());
            return c;
        }

        internal void DeleteData()
        {
            Tuple<LinkedList<ColumnDTO>, Dictionary<int, LinkedList<int>>> cols = _columnDTO.LoadColumns();
            foreach (ColumnDTO c in cols.Item1)
                c.DeleteMe();
            foreach (ColumnBusiness c in columns.Values)
                c.DeleteData();
            nextTaskId = 0;
            nextColumnId = 0;
            columns = new Dictionary<int, ColumnBusiness>();
            bf.DeleteData();
        }
    }
}
