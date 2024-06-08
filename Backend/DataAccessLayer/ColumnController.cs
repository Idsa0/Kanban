using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using log4net;



namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class ColumnController
    {
        private const string ColumnTableName = "Columns";
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string connectionString;
        private readonly string tableName;
        internal ColumnController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = ColumnTableName;
        }


        internal LinkedList<ColumnDTO> LoadColumns()
        {
            log.Info("Got into Load of Column.");
            LinkedList<ColumnDTO> results = new LinkedList<ColumnDTO>();
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

        internal bool Insert(ColumnDTO Column)
        {
            log.Info("Got into Insert of Column.");
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {ColumnTableName} ({ColumnDTO.ColumnIdName}, {ColumnDTO.ColumnBoardIdName}, \"{ColumnDTO.ColumnLimitName}\", {ColumnDTO.ColumnOrdinalName}) " +
                    $"VALUES (@idVal,@boardIdVal,@limitVal,@ordinalVal);";
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", Column.Id);
                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", Column.BoardId);
                    SQLiteParameter limitParam = new SQLiteParameter(@"limitVal", Column.Limit);
                    SQLiteParameter ordinalParam = new SQLiteParameter(@"ordinalVal", Column.Ordinal);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(limitParam);
                    command.Parameters.Add(ordinalParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("couldn't Insert the Column");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }


        internal bool Delete(ColumnDTO Column)
        {
            log.Info("Got into Load of Column.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where id={Column.Id}"
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


        internal ColumnDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new ColumnDTO((int)reader.GetInt32(0)
                , (int)reader.GetInt32(1),
                (int)reader.GetInt32(2), (int)reader.GetInt32(3));
        }


        internal bool Update(int Id, string valueName, int value)
        {
            log.Info("Got into Update of Column.");
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
                    log.Error("Couldn't update the Column");
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