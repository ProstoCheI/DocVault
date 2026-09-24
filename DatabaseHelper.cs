using Dapper;
using Microsoft.Data.Sqlite;
using System.ComponentModel;
using System.Threading;

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

        public async Task<List<Models.Document>> GetDocumentsAsync(int limit = 30, int offset = 0, CancellationToken cancelToken = default)
        {
            CommandDefinition query = new CommandDefinition(
                commandText: @"SELECT * FROM Documents ORDER BY DateAdded DESC LIMIT @Limit OFFSET @Offset;",
                parameters: new { Limit = limit, Offset = offset },
                cancellationToken: cancelToken
            );
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

        public async Task<List<Models.Document>> SearchDocumentsAsync(string keyword, int limit = 30, int offset = 0, CancellationToken cancelToken = default)
        {
            CommandDefinition query = new CommandDefinition(
                commandText: @"SELECT * FROM Documents WHERE LOWER(Title) LIKE LOWER(@Keyword) OR LOWER(Tags) LIKE LOWER(@Keyword) ORDER BY DateAdded DESC LIMIT @Limit OFFSET @Offset;",
                parameters: new { Keyword = $"%{keyword}%", Limit = limit, Offset = offset },
                cancellationToken: cancelToken
            );
            await using (var connection = new SqliteConnection(ConnectionString))
            {
                await connection.OpenAsync();
                var result = await connection.QueryAsync<Models.Document>(query);
                return result.ToList();
            }
        }
    }
}