using Microsoft.Data.Sqlite;
using System;
using Dapper;

namespace DocVaultLocal
{
    internal class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=docvault.db";
        public void InitializeDatabase()
        {
            string query = @"CREATE TABLE IF NOT EXISTS Documents (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Title TEXT NOT NULL,
                Tags TEXT,
                FileType TEXT,
                FilePath TEXT NOT NULL,
                DateAdded DATETIME NOT NULL,
                LastModified DATETIME NOT NULL
            );";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddDocument(string title, string tags, string fileType, string filePath)
        {
            string query = @"INSERT INTO Documents (Title, Tags, FileType, FilePath, DateAdded, LastModified)
                 VALUES (@Title, @Tags, @FileType, @FilePath, @DateAdded, @LastModified);";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue("@Title", title);
                    cmd.Parameters.AddWithValue("@Tags", tags);
                    cmd.Parameters.AddWithValue("@FileType", fileType);
                    cmd.Parameters.AddWithValue("@FilePath", filePath);
                    cmd.Parameters.AddWithValue("@DateAdded", DateTime.Now);
                    cmd.Parameters.AddWithValue("@LastModified", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Models.Document> GetAllDocuments()
        {
            string query = @"SELECT * FROM Documents ORDER BY DateAdded DESC;";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var result = connection.Query<Models.Document>(query).ToList();
                return result;
            }
        }

        public void DeleteDocument(int id)
        {
            string query = @"DELETE FROM Documents WHERE Id = @Id;";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = query;
                    cmd.Parameters.AddWithValue ("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}