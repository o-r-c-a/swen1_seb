using SportsExerciseBattle.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Models;
using System.Text.RegularExpressions;
using SportsExerciseBattle.Interfaces;

namespace SportsExerciseBattle.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        public Tournament? GetCurrentTournament()
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT id, start_time, end_time, status " +
                "FROM tournaments " +
                "WHERE status = 'RUNNING';",
                connection
            );
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                var tournament = new Tournament
                {
                    ID = reader.GetInt32(0),
                    StartTime = reader.GetDateTime(1),
                    EndTime = reader.GetDateTime(2),
                    Status = Enum.Parse<TournamentStatus>(reader.GetString(3))
                };
                return tournament;
            }
            return null;
        }
        public Tournament? GetTournamentById(int id)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT id, start_time, end_time, status " +
                "FROM tournaments " +
                "WHERE id = @id;",
                connection
            );
            DatabaseConnection.AddParameter(command, "@id", id);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new Tournament
                {
                    ID = reader.GetInt32(0),
                    StartTime = reader.GetDateTime(1),
                    EndTime = reader.GetDateTime(2),
                    Status = Enum.Parse<TournamentStatus>(reader.GetString(3))
                };
            }
            return null;
        }
        public List<UserScoreDTO> RetrieveParticipants(Tournament tournament)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT u.username, u.elo, " +
                "COALESCE(SUM(ee.count), 0) AS total_reps " +
                "FROM Users u LEFT JOIN ExerciseEntries ee ON u.username = ee.username " +
                "WHERE ee.tournament_id = @tournamentId " +
                "GROUP BY u.username, u.elo;",
                connection
            );
            DatabaseConnection.AddParameter(command, "@tournamentId", tournament.ID);
            using var reader = command.ExecuteReader();
            var entries = new List<UserScoreDTO>();
            while (reader.Read())
            {
                var entry = new UserScoreDTO
                {
                    Username = reader.GetString(0),
                    EloRating = reader.GetInt32(1),
                    TotalPushUps = reader.GetInt32(2),
                };
                entries.Add(entry);
            }
            return entries;
        }
        // maybe implement
        public List<TournamentStatsDTO>? GetCurrentTournamentOverview()
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT t.start_time, t.status, u.username AS user, " +
                "COUNT(e.id) AS entries_total, " +
                "COALESCE(SUM(e.count), 0) AS reps_total " +
                "FROM Tournaments t " +
                "JOIN TournamentParticipants tp ON t.id = tp.tournament_id " +
                "JOIN Users u ON tp.username = u.username " +
                "LEFT JOIN ExerciseEntries e ON e.username = u.username AND e.tournament_id = t.id " +
                "WHERE t.status = 'RUNNING' " +
                "GROUP BY t.start_time, t.status, u.username " +
                "ORDER BY reps_total DESC, user DESC;",
                connection
            );
            using var reader = command.ExecuteReader();
            var stats = new List<TournamentStatsDTO>();
            while (reader.Read())
            {
                var tourneyStat = new TournamentStatsDTO
                {
                    StartTime = reader.GetDateTime(0),
                    Status = Enum.Parse<TournamentStatus>(reader.GetString(1)),
                    Username = reader.GetString(2),
                    TotalEntries = reader.GetInt32(3),
                    TotalReps = reader.GetInt32(4)
                };
                stats.Add(tourneyStat);
            }
            return stats;
        }
        // GET /tournament
        public List<TournamentStatsDTO> GetTournamentsOverview()
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT t.start_time, t.status, u.username AS user, " +
                "COUNT(e.id) AS entries_total, " +
                "COALESCE(SUM(e.count), 0) AS reps_total " +
                "FROM Tournaments t " +
                "JOIN TournamentParticipants tp ON t.id = tp.tournament_id " +
                "JOIN Users u ON tp.username = u.username " +
                "LEFT JOIN ExerciseEntries e ON e.username = u.username AND e.tournament_id = t.id " +
                "GROUP BY t.start_time, t.status, u.username, u.name " +
                "ORDER BY ARRAY_POSITION(ARRAY['RUNNING', 'COMPLETED'], t.status), t.start_time DESC, reps_total DESC;",
                connection
            );
            using var reader = command.ExecuteReader();
            var stats = new List<TournamentStatsDTO>();
            while (reader.Read())
            {
                var tourneyStat = new TournamentStatsDTO
                {
                    StartTime = reader.GetDateTime(0),
                    Status = Enum.Parse<TournamentStatus>(reader.GetString(1)),
                    Username = reader.GetString(2),
                    TotalEntries = reader.GetInt32(3),
                    TotalReps = reader.GetInt32(4)
                };
                stats.Add(tourneyStat);
            }
            return stats;
        }
        public void AddTournament(Tournament tournament)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "INSERT INTO tournaments (start_time, end_time, status) " +
                "VALUES (@startTime, @endTime, @status);",
                connection
            );
            DatabaseConnection.AddParameter(command, "@startTime", tournament.StartTime);
            DatabaseConnection.AddParameter(command, "@endTime", tournament.EndTime);
            DatabaseConnection.AddParameter(command, "@status", tournament.Status.ToString());
            command.ExecuteNonQuery();
        }
        public void UpdateTournamentStatus(Tournament tournament)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "UPDATE tournaments " +
                "SET status = @status " +
                "WHERE id = @id;",
                connection
            );
            DatabaseConnection.AddParameter(command, "@status", tournament.Status.ToString());
            DatabaseConnection.AddParameter(command, "@id", tournament.ID);
            command.ExecuteNonQuery();
        }
    }
}
