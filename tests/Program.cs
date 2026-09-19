using DocVaultLocal;
using Microsoft.Data.Sqlite;

// This executable has its own output directory; never open an existing archive.
var databasePath = Path.Combine(AppContext.BaseDirectory, "docvault.db");
if (File.Exists(databasePath)) throw new InvalidOperationException("Test database already exists; refusing to modify it.");
var previousDirectory = Environment.CurrentDirectory;
var temporaryDirectory = Path.Combine(Path.GetTempPath(), "docvault-smoke-" + Guid.NewGuid());
Directory.CreateDirectory(temporaryDirectory);
try
{
    Environment.CurrentDirectory = temporaryDirectory;
    var db = new DatabaseHelper();
    db.InitializeDatabase();
    Check(File.Exists(databasePath), "database is beside the executable");
    Check(!File.Exists(Path.Combine(temporaryDirectory, "docvault.db")), "working directory does not select the archive");
    db.AddDocument("Отчёт — версия 1", "покупки", ".pdf", "test-copy.pdf");
    db.AddDocument("Отчёт — версия 2", "работа", ".pdf", "test-copy-2.pdf");
    Check(db.GetAllDocuments().Count == 2, "documents added");
    Check(db.SearchDocuments("покупки").Count == 1, "tag search");
    Check(db.SearchDocuments("' OR 1=1 --").Count == 0, "query parameter is literal input");
    var first = db.SearchDocuments("версия 1").Single();
    db.UpdateDocument(first.Id, "Исправленный отчёт", "архив");
    var updated = db.SearchDocuments("Исправленный").Single();
    Check(updated.DateAdded == first.DateAdded && updated.FilePath == first.FilePath, "editing preserves creation date and file path");
    Check(updated.Tags == "архив", "metadata updated");
    db.InitializeDatabase();
    Check(new DatabaseHelper().GetAllDocuments().Count == 2, "initialization preserves records");
    db.DeleteDocument(first.Id);
    Check(db.GetAllDocuments().Count == 1 && db.SearchDocuments("версия 2").Count == 1, "deletion preserves other records");
    Console.WriteLine("PASS: all archive smoke checks");
}
finally
{
    Environment.CurrentDirectory = previousDirectory;
    SqliteConnection.ClearAllPools();
    File.Delete(databasePath);
    Directory.Delete(temporaryDirectory);
}
static void Check(bool condition, string description)
{
    if (!condition) throw new Exception("FAIL: " + description);
    Console.WriteLine("PASS: " + description);
}
