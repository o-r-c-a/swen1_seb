using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Infrastructure
{
    public static class DatabaseInitializer
    {
        // This method is used to create the database and its tables if they do not exist.
        public static void InitializeDatabase()
        {
            using var conn = DatabaseConnection.GetConnection();
            conn.Open();

            using var command = new NpgsqlCommand();
            command.Connection = conn;

            string scriptPath = Path.Combine(Environment.CurrentDirectory, "SQL", "createTables.sql");
            string script = File.ReadAllText(scriptPath);

            // Execute the script
            command.CommandText = script;
            command.ExecuteNonQuery();
        }
    }
}
