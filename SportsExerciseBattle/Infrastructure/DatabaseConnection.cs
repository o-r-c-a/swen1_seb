using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Infrastructure
{
    public static class DatabaseConnection
    {
        // This method is used to create a connection to the PostgreSQL database.
        public static NpgsqlConnection GetConnection()
        {
            return new NpgsqlConnection(
                "Server=localhost;" +
                "Username=postgres;" +
                "Password=postgres;" +
                "Database=sebdb"
                );
        }
        // This method is used to create a command for executing SQL queries against the PostgreSQL database.
        public static void AddParameter(NpgsqlCommand command, string parameterName, object? value)
        {
            var parameter = command.CreateParameter();
            parameter.ParameterName = parameterName;
            parameter.Value = value ?? DBNull.Value;

            if (value != null)
            {
                parameter.NpgsqlDbType = GetDbType(value);
            }

            command.Parameters.Add(parameter);
        }

        private static NpgsqlDbType GetDbType(object value)
        {
            return value switch
            {
                int => NpgsqlDbType.Integer,
                long => NpgsqlDbType.Bigint,
                string => NpgsqlDbType.Text,
                DateTime => NpgsqlDbType.TimestampTz,
                bool => NpgsqlDbType.Boolean,
                Guid => NpgsqlDbType.Uuid,
                float => NpgsqlDbType.Real,
                double => NpgsqlDbType.Double,
                _ => throw new InvalidOperationException($"Unsupported parameter type: {value.GetType()}")
            };
        }
    }
}
