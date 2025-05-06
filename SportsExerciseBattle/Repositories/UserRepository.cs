using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Infrastructure;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Utilities;

namespace SportsExerciseBattle.Repositories
{
    public class UserRepository : IUserRepository
    {
        // This class is responsible for managing user data in the database.
        public void AddUser(User user)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();

            var command = new Npgsql.NpgsqlCommand(
                "INSERT INTO users (username, password, elo) " +
                "VALUES (@username, @password, @elo)",
                connection
            );
            DatabaseConnection.AddParameter(command, "@username", user.Username);
            DatabaseConnection.AddParameter(command, "@password", user.Password);
            DatabaseConnection.AddParameter(command, "@elo", user.ELO);
            command.ExecuteNonQuery();
        }

        public User? GetUserByUsername(string username)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();

            var command = new Npgsql.NpgsqlCommand(
                "SELECT * FROM users " +
                "WHERE username = @username",
                connection
            );
            DatabaseConnection.AddParameter(command, "@username", username);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Username = reader.GetString(0),
                    Password = reader.GetString(1),
                    AuthToken = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Bio = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Image = reader.IsDBNull(5) ? null : reader.GetString(5),
                    ELO = reader.IsDBNull(6) ? ConstantsSettings.DefaultELO : reader.GetInt32(6)
                };
            }
            return null;
        }

        public User? GetUserByToken(string token)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();

            var command = new Npgsql.NpgsqlCommand(
                "SELECT * FROM users WHERE authtoken = @token",
                connection
            );
            DatabaseConnection.AddParameter(command, "@token", token);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new User
                {
                    Username = reader.GetString(0),
                    Password = reader.GetString(1),
                    AuthToken = reader.IsDBNull(2) ? null : reader.GetString(2),
                    Name = reader.IsDBNull(3) ? null : reader.GetString(3),
                    Bio = reader.IsDBNull(4) ? null : reader.GetString(4),
                    Image = reader.IsDBNull(5) ? null : reader.GetString(5),
                    ELO = reader.IsDBNull(6) ? 0 : reader.GetInt32(6)
                };
            }
            return null;
        }
        public void UpdateUserToken(string username, string authtoken)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            // Update the user's auth_token in the database
            var command = new Npgsql.NpgsqlCommand(
                "UPDATE users " +
                "SET authtoken = @authtoken " +
                "WHERE username = @username",
                connection
            );
            DatabaseConnection.AddParameter(command, "@authtoken", authtoken);
            DatabaseConnection.AddParameter(command, "@username", username);
            command.ExecuteNonQuery();
        }

        public void UpdateUserProfile(string username, UserProfileDTO user)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            // Update the user's profile in the database
            var command = new Npgsql.NpgsqlCommand(
                "UPDATE users " +
                "SET name = @name, bio = @bio, image = @image " +
                "WHERE username = @username",
                connection
            );
            DatabaseConnection.AddParameter(command, "@name", user.Name);
            DatabaseConnection.AddParameter(command, "@bio", user.Bio);
            DatabaseConnection.AddParameter(command, "@image", user.Image);
            DatabaseConnection.AddParameter(command, "@username", username);
            command.ExecuteNonQuery();
        }

        public UserProfileDTO GetUserProfile(string username)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            // Get the user's profile from the database
            var command = new Npgsql.NpgsqlCommand(
                "SELECT name, bio, image FROM users " +
                "WHERE username = @username",
                connection
            );
            DatabaseConnection.AddParameter(command, "@username", username);
            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                return new UserProfileDTO
                {
                    Name = reader.IsDBNull(0) ? "" : reader.GetString(0),
                    Bio = reader.IsDBNull(1) ? "" : reader.GetString(1),
                    Image = reader.IsDBNull(2) ? "" : reader.GetString(2)
                };
            }
            throw new Exception($"Database error.");
        }
        public void UpdateUserElo(string username, int elo)
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            // Update the user's ELO in the database
            var command = new Npgsql.NpgsqlCommand(
                "UPDATE users " +
                "SET elo = @elo " +
                "WHERE username = @username",
                connection
            );
            DatabaseConnection.AddParameter(command, "@elo", elo);
            DatabaseConnection.AddParameter(command, "@username", username);
            command.ExecuteNonQuery();
        }
        public bool UserExists(string username)
        {
            return GetUserByUsername(username) != null;
        }

        public bool TokenExists(string token)
        {
            return GetUserByToken(token) != null;
        }

        public Dictionary<string, int> GetAllUsersElo()
        {
            using var connection = DatabaseConnection.GetConnection();
            connection.Open();
            var command = new Npgsql.NpgsqlCommand(
                "SELECT * FROM users",
                connection
            );
            using var reader = command.ExecuteReader();
            Dictionary<string, int> elos = [];
            
            while (reader.Read())
                elos.Add(reader.GetString(0), reader.IsDBNull(6) ? 0 : reader.GetInt32(6));
            
            return elos;
        }
    }
}
