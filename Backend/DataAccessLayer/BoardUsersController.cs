using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using log4net;


namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardUsersController
    {
        private const string ColumnTasksTableName = "BOARD_USERS";
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string connectionString;
        private readonly string tableName;
        internal BoardUsersController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\kanban.db")); // \\Backend\\bin\\Debug\\net6.0\\
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = ColumnTasksTableName;
        }

        internal Dictionary<int, LinkedList<string>> LoadBoardUsers()
        {
            log.Info("Got into Load of Board.");
            Dictionary<int, LinkedList<string>> results = new Dictionary<int, LinkedList<string>>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select distinct BID from {tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                    {
                        int x = (int)dataReader.GetInt32(0);
                        results.Add(x, LoadBoardUsers(x));
                    }
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

        private LinkedList<string> LoadBoardUsers(int bid)
        {
            LinkedList<string> results = new LinkedList<string>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select UEMAIL from {tableName} where BID = {bid};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    while (dataReader.Read())
                        results.AddFirst(dataReader.GetString(0));

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

        internal Tuple<int, string> ConvertReaderToObject(SQLiteDataReader reader)
        {
            return Tuple.Create((int)reader.GetValue(0), reader.GetString(1));
        }

        internal bool Insert(Tuple<int, string> t)
        {
            log.Info("Got into Insert of Board-Users.");
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTasksTableName} ({"BID"} ,{"UEMAIL"}) " + $"VALUES (@BIDVal,@UEMAILVal);";
                    SQLiteParameter BIDParam = new SQLiteParameter(@"BIDVal", t.Item1);
                    SQLiteParameter UEMAILParam = new SQLiteParameter(@"UEMAILVal", t.Item2);
                    command.Parameters.Add(BIDParam);
                    command.Parameters.Add(UEMAILParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("couldn't Insert the Board-Users");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        internal bool RemoveUser(Tuple<int, string> t)
        {
            log.Info("Got into Delete of Board-Users.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where BID = {t.Item1} and UEMAIL='{t.Item2}'"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("Error 404: User was not found");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        internal bool DeleteUser(string uEmail)
        {
            log.Info("Got into Delete of Board-Users.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where UEMAIL={uEmail}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("Error 404: User was not found");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        internal bool DeleteAll(int boardIds)
        {
            log.Info("Got into Delete of Board-Users.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where BID={boardIds}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {

                    log.Error("Error 404: Board was not found");
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
