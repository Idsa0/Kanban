using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using log4net;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class BoardController
    {
        private const string BoardTableName = "Boards";
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string connectionString;
        private readonly string tableName;
        internal BoardController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\kanban.db")); // \\Backend\\bin\\Debug\\net6.0\\
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = BoardTableName;
        }


        internal LinkedList<BoardDTO> LoadBoards()
        {
            log.Info("Got into Load of Board.");
            LinkedList<BoardDTO> results = new LinkedList<BoardDTO>();
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

        internal bool Insert(BoardDTO Board)
        {
            log.Info("Got into Insert of Board.");
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {BoardTableName} ({BoardDTO.BoardIdName} ,{BoardDTO.BoardName}, {BoardDTO.BoardOwnerEmailName}) " +
                    $"VALUES (@idVal,@nameVal,@emailVal);";
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", Board.Id);
                    SQLiteParameter nameParam = new SQLiteParameter(@"nameVal", Board.Name);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", Board.OwnerEmail);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(nameParam);
                    command.Parameters.Add(emailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("couldn't Insert the Board");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }


        internal bool Delete(BoardDTO Board)
        {
            log.Info("Got into Delete of Board.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where id={Board.Id}"
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


        internal BoardDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new BoardDTO((int)reader.GetInt32(0)
                , reader.GetString(1), reader.GetString(2));
        }

        internal bool Update(int Id, string valueName, string value)
        {
            log.Info("Got into Update of Board.");
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
                    command.Parameters.Add(new SQLiteParameter(valueName, value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("Couldn't update the Board");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
            }
            return res > 0;
        }

        internal bool Update(int Id, string valueName, LinkedList<string> value)
        {
            log.Info("Got into Update of Board.");
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
                    log.Error("Couldn't update the Board");
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
            log.Info("Got into Update of Board.");
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
                    log.Error("Couldn't update the Board");
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
