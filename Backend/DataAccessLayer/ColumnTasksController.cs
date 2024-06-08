using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using log4net;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnTasksController
    {
        private const string ColumnTasksTableName = "COLUMN_TASKS";
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string connectionString;
        private readonly string tableName;
        internal ColumnTasksController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = ColumnTasksTableName;
        }

        internal Dictionary<int, LinkedList<int>> LoadColumnTasks()
        {
            log.Info("Got into Load of Column-Tasks.");

            Dictionary<int, LinkedList<int>> results = new Dictionary<int, LinkedList<int>>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select distinct CID from {tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int x = (int)dataReader.GetInt32(0);
                        results.Add(x, LoadColumnTasks(x));
                    }
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




        private LinkedList<int> LoadColumnTasks(int cid)
        {
            LinkedList<int> results = new LinkedList<int>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select TID from {tableName} where CID = {cid};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.AddFirst((int)dataReader.GetInt32(0));
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
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


        //internal Tuple<int, int> ConvertReaderToObject(SQLiteDataReader reader)
        //{
        //    return Tuple.Create((int)reader.GetValue(0), (int)reader.GetValue(1));
        //}

        internal bool Insert(Tuple<int, int> t)
        {
            log.Info("Got into Insert of Column-Tasks.");
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTasksTableName} ({"CID"} ,{"TID"}) " + $"VALUES (@CIDVal,@TIDVal);";
                    SQLiteParameter CIDParam = new SQLiteParameter(@"CIDVal", t.Item1);
                    SQLiteParameter TIDParam = new SQLiteParameter(@"TIDVal", t.Item2);
                    command.Parameters.Add(CIDParam);
                    command.Parameters.Add(TIDParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("couldn't Insert the Column-Tasks");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }


        internal bool DeleteTask(int taskId)
        {
            log.Info("Got into Delete of Column-Tasks.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where TID={taskId}"
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

        internal bool DeleteAll(int columnId)
        {
            log.Info("Got into Delete of Column-Tasks.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where CID={columnId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {

                    log.Error("Error 404: Column was not found");
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
