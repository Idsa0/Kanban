using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class TaskController
    {
        private const string TaskTableName = "Tasks";
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string connectionString;
        private readonly string tableName;
        internal TaskController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = TaskTableName;
        }


        internal LinkedList<TaskDTO> LoadTasks()
        {
            log.Info("Got into Load of Tasks.");
            LinkedList<TaskDTO> results = new LinkedList<TaskDTO>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.AddFirst(ConvertReaderToObject(dataReader));
                }
                catch (Exception ex)
                {
                    log.Error(ex.ToString());
                }
                finally
                {
                    if (dataReader != null)
                        dataReader.Close();
                    command.Dispose();
                    connection.Close();
                }
            }
            return results;
        }

        internal bool Insert(TaskDTO Task)
        {
            log.Info("Got into Insert of Task.");
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {TaskTableName} ({TaskDTO.TaskIdName},{TaskDTO.TaskAssigneeEmailName}, "+
                        $"{TaskDTO.TaskColumnOrdinalName} ,{TaskDTO.TaskCreationTimeName}, {TaskDTO.TaskDueDateName}, {TaskDTO.TaskTitleName}, " +
                        $"{TaskDTO.TaskDescriptionName}, {TaskDTO.TaskBoardIdName} ) " +
                        $"VALUES (@idVal,@assigneeEmaillVal,@ordinallVal,@creationVal,@dueVal,@titleVal,@descriptionlVal," +
                        $"@boardIdVal);";
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", Task.Id);
                    SQLiteParameter creationParam = new SQLiteParameter(@"creationVal", Task.CreationTime);
                    SQLiteParameter dueParam = new SQLiteParameter(@"dueVal", Task.DueDate);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", Task.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionlVal", Task.Description);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"ordinallVal", Task.ColumnOrdinal);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", Task.BoardId);
                    SQLiteParameter assigneeEmailParam = new SQLiteParameter(@"assigneeEmaillVal", Task.AssigneeEmail);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(creationParam);
                    command.Parameters.Add(dueParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(assigneeEmailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("couldn't Insert the Task");
                    Console.WriteLine(ex.ToString());// TODO DELETE THIS
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        internal bool Delete(TaskDTO Task)
        {
            log.Info("Got into Delete of Task.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where id={Task.Id}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("Error 404: Task was not found");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }


        internal TaskDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new TaskDTO(
            (int)reader.GetInt32(0),
            GetDateTimeValue(reader.GetValue(3)),
            GetDateTimeValue(reader.GetValue(4)),
            reader.GetString(5),
            reader.GetString(6),
            (int)reader.GetInt32(2),
            (int)reader.GetInt32(7),
            reader.GetString(1));
        }

        // Helper method to handle DateTime conversion
        private DateTime GetDateTimeValue(object value)
        {
            DateTime dateTime;
            if (value != null && !string.IsNullOrEmpty(value.ToString()))
                if (DateTime.TryParse(value.ToString(), out dateTime))
                    return dateTime;
            log.Error("Corrapted day time data");
            return new DateTime();
        }

        internal bool Update(int Id, string valueName, DateTime value)
        {
            log.Info("Got into Update of Task.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{valueName}]= @value where id = {Id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@"value", value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("Cannot update task-DataTime");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        internal bool Update(int Id, string valueName, int value)
        {
            log.Info("Got into Update of Task.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{valueName}]= @value where id = {Id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@"value", value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("Cannot update task-int");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        internal bool Update(int Id, string valueName, string value)
        {
            log.Info("Got into Update of Task.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {tableName} set [{valueName}]= @value where id = {Id}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(@"value", value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    log.Error("Cannot update task-string");
                }

                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }
    }
}
