using IntroSE.Kanban.Backend.BusinessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class TaskService
    {
        private TaskFacade tf;

        public TaskService() { }

        internal TaskFacade Tf { set { tf = value; } }

        /// <summary>
        /// This method updates the due date of a task
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="dueDate">The new due date of the column</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDueDate(string email, string boardName, int columnOrdinal, int taskId, DateTime dueDate)
        {
            Response res;
            try
            {
                tf.UpdateTaskDueDate(email, boardName, columnOrdinal, taskId, dueDate);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method updates task title.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="title">New title for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskTitle(string email, string boardName, int columnOrdinal, int taskId, string title)
        {
            Response res;
            try
            {
                tf.UpdateTaskTitle(email, boardName, columnOrdinal, taskId, title);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method updates the description of a task.
        /// </summary>
        /// <param name="email">Email of user. Must be logged in</param>
        /// <param name="boardName">The name of the board</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column</param>
        /// <param name="taskId">The task to be updated identified task ID</param>
        /// <param name="description">New description for the task</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string UpdateTaskDescription(string email, string boardName, int columnOrdinal, int taskId, string description)
        {
            Response res;
            try
            {
                tf.UpdateTaskDescription(email, boardName, columnOrdinal, taskId, description);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }

        }

        /// <summary>
        /// This method returns all in-progress tasks of a user.
        /// </summary>
        /// <param name="email">Email of the user. Must be logged in</param>
        /// <returns>A response with a list of the in-progress tasks of the user, unless an error occurs (see <see cref="GradingService"/>)</returns>
        public string InProgressTasks(string email)
        {
            Response res;
            try
            {
                res = new Response(tf.InProgressTasks(email));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method limits the number of tasks in a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column.</param>
        /// <param name="limit">The new limit value. A value of -1 indicates no limit.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>).</returns>
        public string LimitColumn(string email, string boardName, int columnOrdinal, int limit)
        {
            Response res;
            try
            {
                tf.LimitColumn(email, boardName, columnOrdinal, limit);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method gets the limit of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column.</param>
        /// <returns>A response with the column's limit, unless an error occurs (see <see cref="GradingService"/>).</returns>
        public string GetColumnLimit(string email, string boardName, int columnOrdinal)
        {
            Response res;
            try
            {
                res = new Response(tf.GetColumnLimit(email, boardName, columnOrdinal) as object);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method gets the name of a specific column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column.</param>
        /// <returns>A response with the column's name, unless an error occurs (see <see cref="GradingService"/>).</returns>
        public string GetColumnName(string email, string boardName, int columnOrdinal)
        {
            Response res;
            try
            {
                res = new Response(tf.GetColumnName(email, boardName, columnOrdinal) as object);
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method adds a new task.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="title">Title of the new task.</param>
        /// <param name="description">Description of the new task.</param>
        /// <param name="dueDate">The due date of the new task.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>).</returns>
        public string AddTask(string email, string boardName, string title, string description, DateTime dueDate)
        {
            Response res;
            try
            {
                tf.AddTask(email, boardName, title, description, dueDate);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method advances a task to the next column.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column.</param>
        /// <param name="taskId">The task to be updated identified task ID.</param>
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>).</returns>
        /// <exception cref="ArgumentException">Thrown if the task does not exist or if the column is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown if the column is full or if the user is not assigned to the task</exception>

        public string AdvanceTask(string email, string boardName, int columnOrdinal, int taskId)
        {
            Response res;
            try
            {
                tf.AdvanceTask(email, boardName, columnOrdinal, taskId);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>
        /// This method returns a column given it's name.
        /// </summary>
        /// <param name="email">The email address of the user, must be logged in.</param>
        /// <param name="boardName">The name of the board.</param>
        /// <param name="columnOrdinal">The column ID. The first column is identified by 0, the ID increases by 1 for each column.</param>
        /// <returns>A response with a list of the column's tasks, unless an error occurs (see <see cref="GradingService"/>).</returns>
        public string GetColumn(string email, string boardName, int columnOrdinal)
        {
            Response res;
            try
            {
                res = new Response(tf.GetColumn(email, boardName, columnOrdinal));
                return JsonSerializer.Serialize(res);
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        /// <summary>		 
        /// This method assigns a task to a user		 
        /// </summary>		 
        /// <param name="email">Email of the user. Must be logged in</param>		 
        /// <param name="boardName">The name of the board</param>		 
        /// <param name="columnOrdinal">The column number. The first column is 0, the number increases by 1 for each column</param>		 
        /// <param name="taskID">The task to be updated identified a task ID</param>        		 
        /// <param name="emailAssignee">Email of the asignee user</param>		 
        /// <returns>An empty response, unless an error occurs (see <see cref="GradingService"/>)</returns>		 
        /// <exception cref="ArgumentException">Thrown if one of the arguments is invalid</exception>
        /// <exception cref="InvalidOperationException">Thrown if the email is not the board owner or if the assignee is not part of the board</exception>
        public string AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            Response res;
            try
            {
                tf.AssignTask(email, boardName, columnOrdinal, taskID, emailAssignee);
                return JsonSerializer.Serialize(new Response());
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        ///<summary>This method loads all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b> When starting the system via the GradingService - do not load the data automatically, only through this method.
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        /// <returns>null, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="IOException">Thrown if there was a problem when accessing the data</exception>
        public string LoadData()
        {
            Response res;
            try
            {
                tf.LoadData();
                return null;
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }

        ///<summary>This method deletes all persisted data.
        ///<para>
        ///<b>IMPORTANT:</b>
        ///In some cases we will call LoadData when the program starts and in other cases we will call DeleteData. Make sure you support both options.
        ///</para>
        /// </summary>
        ///<returns>null, unless an error occurs (see <see cref="GradingService"/>)</returns>
        /// <exception cref="IOException">Thrown if there was a problem when accessing the data</exception>
        public string DeleteData()
        {
            Response res;
            try
            {
                tf.DeleteData();
                return null;
            }
            catch (Exception ex)
            {
                res = new Response(ex.Message);
                return JsonSerializer.Serialize(res);
            }
        }
    }
}
