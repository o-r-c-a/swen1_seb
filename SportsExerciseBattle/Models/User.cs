using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using SportsExerciseBattle.Utilities;

namespace SportsExerciseBattle.Models
{
    public class User
    {
        [JsonPropertyName("Username")]
        public string Username { get; set; }
        [JsonPropertyName("Password")]
        public string Password { get; set; }
        public string? AuthToken { get; set; }
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? Image { get; set; }
        public int ELO { get; set; } = ConstantsSettings.DefaultELO;
        public List<ExerciseEntry> Entries { get; set; } = [];

        public User()
        {
            Username = string.Empty;
            Password = string.Empty;
        }
        public User(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public int GetTotalPushUps()
        {
            return Entries?.Sum(entry => entry.Count) ?? 0;
        }

        public int GetTotalEntries()
        {
            return Entries?.Count ?? 0;
        }

        public bool AddExerciseEntry(ExerciseEntry entry)
        {
            if (entry == null || entry.Count <= 0)
                return false;

            if ( entry.Name != ConstantsSettings.DefaultExerciseName || entry.Name == null)
            {
                Console.WriteLine($"User '{Username}'. Invalid exercise entry name: {entry.Name}. Expected: {ConstantsSettings.DefaultExerciseName}");
                return false;
            }

            entry.Timestamp = entry.Timestamp != default ? entry.Timestamp : DateTime.UtcNow;
            Entries.Add(entry);
            Console.WriteLine($"User '{Username}'. Added entry: {entry.Name}, Count: {entry.Count}, Duration: {entry.DurationInSeconds} seconds, Timestamp: {entry.Timestamp}");
            return true;
        }

        public override string ToString()
        {
            return $"User: {Username}, ELO: {ELO}, Name: {Name}, Bio: {Bio}, IMG: {Image}";
        }
    }
}
