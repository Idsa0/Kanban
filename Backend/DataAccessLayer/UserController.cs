using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using log4net;



namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    internal class UserController
    {
        private const string UserTableName = "Users";
        private readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly string connectionString;
        private readonly string tableName;
        internal UserController()
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            //string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..\\..\\..\\..\\kanban.db"));
            this.connectionString = $"Data Source={path}; Version=3;";
            this.tableName = UserTableName;
        }


        internal LinkedList<UserDTO> LoadUsers()
        {
            log.Info("Got into Load of Users.");
            LinkedList<UserDTO> results = new LinkedList<UserDTO>();
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
                    Console.WriteLine(ex.ToString());
                    log.Error("Error 404: User was not found");
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


        internal UserDTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            return new UserDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
        }

        internal bool Insert(UserDTO User)
        {
            log.Info("Got into Insert of User.");
            using (var connection = new SQLiteConnection(connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {UserTableName} ({UserDTO.UseridName} ,{UserDTO.UserEmailName}, {UserDTO.UserpasswordName}) " +
                        $"VALUES (@idVal,@emailVal,@passwordVal);";
                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", User.Id);
                    SQLiteParameter emailParam = new SQLiteParameter(@"emailVal", User.Email);
                    SQLiteParameter passwordParam = new SQLiteParameter(@"passwordVal", User.Password);
                    command.Parameters.Add(idParam);
                    command.Parameters.Add(emailParam);
                    command.Parameters.Add(passwordParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                }
                catch
                {
                    log.Error("couldn't Insert the User");
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }


        internal bool Delete(UserDTO User)
        {
            log.Info("Got into Delete of User.");
            int res = -1;
            using (var connection = new SQLiteConnection(connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {tableName} where id={User.Id}"
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
        internal bool Update(int Id, string valueName, bool value)
        {
            log.Info("Got into Update of User.");
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
                    log.Error("Couldn't update the User");
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
            log.Info("Got into Update of User.");
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
                    log.Error("Couldn't update the User");
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
