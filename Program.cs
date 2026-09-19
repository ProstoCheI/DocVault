using System;
using System.Threading;

namespace DocVaultLocal
{
    internal static class Program
    {
#if NET9_0_WINDOWS
        /// <summary>
        ///  The main entry point for the Windows Forms application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
#else
        /// <summary>
        ///  The main entry point for Docker Container execution.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("========================================");
            Console.WriteLine("    DocVault Service [Docker Engine]    ");
            Console.WriteLine("========================================");
            Console.WriteLine($"Environment: Linux Container (.NET 9.0)");
            Console.WriteLine($"Started at:  {DateTime.Now:yyyy-MM-dd HH:mm:ss}");

            try
            {
                var db = new DatabaseHelper();
                Console.WriteLine("\n[1/2] Initializing SQLite database in container...");
                db.InitializeDatabase();
                Console.WriteLine("      Database schema verified and ready.");

                Console.WriteLine("\n[2/2] Checking documents in database...");
                var docs = db.GetAllDocuments();
                Console.WriteLine($"      Total documents stored: {docs.Count}");

                Console.WriteLine("\n========================================");
                Console.WriteLine("  DocVault Container is HEALTHY & RUNNING ");
                Console.WriteLine("========================================");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[ERROR] Initialization failed: {ex.Message}");
                Console.ResetColor();
            }

            // Keep the container running for daemon / inspection
            while (true)
            {
                Thread.Sleep(30000);
                Console.WriteLine($"[Heartbeat] DocVault Container active - {DateTime.Now:HH:mm:ss}");
            }
        }
#endif
    }
}