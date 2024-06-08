using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnDTO
    {
        private ColumnController controller;
        private ColumnTasksController tasks;
        internal const string ColumnBoardIdName = "BoardId";
        internal const string ColumnLimitName = "Limit";
        internal const string ColumnOrdinalName = "Ordinal";
        internal const string ColumnIdName = "ID";

        internal int Id { get; set; }

        private int boardId;
        internal int BoardId
        {
            get => boardId;
            set
            {
                boardId = value; controller.Update(Id, ColumnBoardIdName, value);
            }
        }

        private int limit;
        internal int Limit
        {
            get => limit;
            set
            {
                limit = value; controller.Update(Id, ColumnLimitName, value);
            }
        }
        private int ordinal;
        internal int Ordinal
        {
            get => ordinal;
            set
            {
                ordinal = value; controller.Update(Id, ColumnOrdinalName, value);
            }
        }

        private Dictionary<int, TaskBusiness> tasks_ids;
        internal Dictionary<int, TaskBusiness> Tasks_IDs
        {
            get => tasks_ids;
            set
            {
                tasks_ids = value;
            }
        }

        internal ColumnDTO()
        {
            tasks = new ColumnTasksController();
            controller = new ColumnController();
        }
        internal ColumnDTO(int ID, Dictionary<int, TaskBusiness> Tasks, int BoardId, int Limit, int Ordinal)
        {
            Id = ID;
            tasks = new ColumnTasksController();
            boardId = BoardId;
            limit = Limit;
            ordinal = Ordinal;
            controller = new ColumnController();
            tasks_ids = Tasks;
        }
        internal ColumnDTO(int ID, int BoardId, int Limit, int Ordinal)
        {
            Id = ID;
            tasks = new ColumnTasksController();
            boardId = BoardId;
            limit = Limit;
            ordinal = Ordinal;
            controller = new ColumnController();
            tasks_ids = new Dictionary<int, TaskBusiness>();
        }


        internal Tuple<LinkedList<ColumnDTO>, Dictionary<int, LinkedList<int>>> LoadColumns()
        {
            Dictionary<int, LinkedList<int>> tasks = this.tasks.LoadColumnTasks();
            LinkedList<ColumnDTO> DTOs = controller.LoadColumns();
            return Tuple.Create(DTOs, tasks);
        }

        internal bool InsertMe()
        {
            bool x = true;
            foreach (int t in tasks_ids.Keys)
                if (!tasks.Insert(Tuple.Create(Id, t)))
                    x = false;
            return x && controller.Insert(this);
        }

        internal bool InsertTask(int cid, int tid)
        {
            return tasks.Insert(Tuple.Create(cid, tid));
        }
        
        internal bool DeleteTask(int tid)
        {
            return tasks.DeleteTask(tid);
        }
        internal bool DeleteMe()
        {
            return tasks.DeleteAll(Id) & controller.Delete(this);
        }

        internal void ChangeValues(int ID, Dictionary<int, TaskBusiness> Tasks, int BoardId, int Limit, int Ordinal)
        {
            Id = ID;
            tasks_ids = Tasks;
            boardId = BoardId;
            limit = Limit;
            ordinal = Ordinal;
        }
    }
}
