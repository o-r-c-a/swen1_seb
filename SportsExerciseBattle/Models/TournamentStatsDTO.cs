using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class TournamentStatsDTO
    {
        [JsonPropertyName("StartTime")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("Status")]
        public TournamentStatus Status { get; set; }
        [JsonPropertyName("Username")]
        public string Username { get; set; }
        [JsonPropertyName("TotalEntries")]
        public int TotalEntries { get; set; }
        [JsonPropertyName("TotalReps")]
        public int TotalReps { get; set; }
    }
}
