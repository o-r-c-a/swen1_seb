using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Infrastructure;
using System.Text.RegularExpressions;

namespace SportsExerciseBattle.Repositories
{
    public class ExerciseEntryRepository : IExerciseEntryRepository
    {
        public List<ExerciseEntry>? GetExerciseEntriesByUsername(string username)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT name, count, duration_in_seconds " +
                "FROM ExerciseEntries " +
                "WHERE username = @username",
                connection
            );
            DatabaseConnection.AddParameter(command, "@username", username);
            using var reader = command.ExecuteReader();
            var entries = new List<ExerciseEntry>();
            while (reader.Read())
            {
                var entry = new ExerciseEntry
                {
                    Name = reader.GetString(0),
                    Count = reader.GetInt32(1),
                    DurationInSeconds = reader.GetInt32(2)
                };
                entries.Add(entry);
            }
            return entries;
        }
        //  retrieve single user statistics
        public UserScoreDTO? GetStats(string username)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT u.elo, " +
                "COALESCE(SUM(ee.count), 0) AS total_pushups " +
                "FROM Users u " +
                "LEFT JOIN ExerciseEntries ee ON u.username = ee.username " +
                "WHERE u.username = @username " +
                "GROUP BY u.elo; ",
                connection
            );
            DatabaseConnection.AddParameter(command, "@username", username);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserScoreDTO
                {
                    EloRating = reader.GetInt32(0),
                    TotalPushUps = reader.GetInt32(1)
                };
            }
            return null;
        }
        //  retrieve scores of all users
        public List<UserScoreDTO> GetScores()
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            // Get all exercise entries for the user from the database
            var command = new Npgsql.NpgsqlCommand(
                "SELECT u.username, u.elo, " +
                "COALESCE(SUM(ee.count), 0) AS total_pushups, " +
                "COALESCE(COUNT(ee.id), 0) AS total_entries " +
                "FROM Users u " +
                "LEFT JOIN ExerciseEntries ee ON u.username = ee.username " +
                "GROUP BY u.username, u.elo;",
                connection
            );
            using var reader = command.ExecuteReader();
            var entries = new List<UserScoreDTO>();
            while (reader.Read()) {
                var entry = new UserScoreDTO
                {
                    Username = reader.GetString(0),
                    EloRating = reader.GetInt32(1),
                    TotalPushUps = reader.GetInt32(2),
                    TotalEntries = reader.GetInt32(3),
                };
                entries.Add(entry);
            }
            return entries;
        }

        public void AddEntry(string username, ExerciseEntry entry, int tournamentID)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            // Insert a new exercise entry into the database
            var command = new Npgsql.NpgsqlCommand(
                "INSERT INTO exerciseentries (username, name, count, duration_in_seconds, timestamp, tournament_id) " +
                "VALUES (@username, @name, @count, @durationInSeconds, @timestamp, @tournamentID)",
                connection
            );
            DatabaseConnection.AddParameter(command, "@username", username);
            DatabaseConnection.AddParameter(command, "@name", entry.Name);
            DatabaseConnection.AddParameter(command, "@count", entry.Count);
            DatabaseConnection.AddParameter(command, "@durationInSeconds", entry.DurationInSeconds);
            DatabaseConnection.AddParameter(command, "@timestamp", entry.Timestamp);
            DatabaseConnection.AddParameter(command, "@tournamentID", tournamentID);
            command.ExecuteNonQuery();
        }
    }
}
