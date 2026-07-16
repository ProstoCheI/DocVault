using Dapper;
using Microsoft.Data.Sqlite;

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
                connection.Execute(query);
            }
        }

        public void AddDocument(string title, string tags, string fileType, string filePath)
        {
            string query = @"INSERT INTO Documents (Title, Tags, FileType, FilePath, DateAdded, LastModified)
                 VALUES (@Title, @Tags, @FileType, @FilePath, @DateAdded, @LastModified);";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(query, new { Title = title, Tags = tags, FileType = fileType, FilePath = filePath, DateAdded = DateTime.Now, LastModified = DateTime.Now });
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
                connection.Execute(query, new { Id = id});
            }
        }

        public void UpdateDocument(int id, string title, string tags)
        {
            string query = @"UPDATE Documents 
                            SET Title = @Title, Tags = @Tags, LastModified = @LastModified 
                            WHERE Id = @Id;";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                connection.Execute(query, new {Id = id, Title = title, Tags = tags, LastModified = DateTime.Now });
            }
        }

        public List<Models.Document> SearchDocuments(string keyword)
        {
            string query = @"SELECT * FROM Documents 
                            WHERE Title LIKE @Keyword OR Tags LIKE @Keyword 
                            ORDER BY DateAdded DESC;";
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var result = connection.Query<Models.Document>(query, new { Keyword = $"%{keyword}%" }).ToList();
                return result;
            }
        }
    }
}