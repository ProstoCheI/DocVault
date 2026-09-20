using Dapper;
using Microsoft.Data.Sqlite;

namespace DocVaultLocal
{
    internal class DatabaseHelper
    {
        private const string ConnectionString = "Data Source=docvault.db";
        public async Task InitializeDatabaseAsync()
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
            await using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query);
            }
        }

        public async Task AddDocumentAsync(string title, string tags, string fileType, string filePath)
        {
            string query = @"INSERT INTO Documents (Title, Tags, FileType, FilePath, DateAdded, LastModified)
                 VALUES (@Title, @Tags, @FileType, @FilePath, @DateAdded, @LastModified);";
            await using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new { Title = title, Tags = tags, FileType = fileType, FilePath = filePath, DateAdded = DateTime.Now, LastModified = DateTime.Now });
            }
        }

        public async Task<List<Models.Document>> GetAllDocumentsAsync()
        {
            string query = @"SELECT * FROM Documents ORDER BY DateAdded DESC;";
            await using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<Models.Document>(query);
                return result.ToList();
            }
        }

        public async Task DeleteDocumentAsync(int id)
        {
            string query = @"DELETE FROM Documents WHERE Id = @Id;";
            await using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new { Id = id});
            }
        }

        public async Task UpdateDocumentAsync(int id, string title, string tags)
        {
            string query = @"UPDATE Documents 
                            SET Title = @Title, Tags = @Tags, LastModified = @LastModified 
                            WHERE Id = @Id;";
            await using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                await connection.ExecuteAsync(query, new {Id = id, Title = title, Tags = tags, LastModified = DateTime.Now });
            }
        }

        public async Task<List<Models.Document>> SearchDocumentsAsync(string keyword)
        {
            string query = @"SELECT * FROM Documents 
                            WHERE Title LIKE @Keyword OR Tags LIKE @Keyword 
                            ORDER BY DateAdded DESC;";
            await using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<Models.Document>(query, new { Keyword = $"%{keyword}%" });
                return result.ToList();
            }
        }
    }
}