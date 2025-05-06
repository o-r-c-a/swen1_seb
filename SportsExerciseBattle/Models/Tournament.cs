using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Utilities;
using System.Text.Json.Serialization;

namespace SportsExerciseBattle.Models
{
    public enum TournamentStatus
    {
        RUNNING,
        ENDED
    }
    public class Tournament
    {
        [JsonPropertyName("ID")]
        public int ID { get; set; }
        [JsonPropertyName("StartTime")]
        public DateTime StartTime { get; set; }
        [JsonPropertyName("EndTime")]
        public DateTime EndTime { get; set; }
        [JsonPropertyName("Status")]
        public TournamentStatus Status { get; set; }
        //public List<Participant> Participants { get; set; } = [];

        //public List<User> GetWinners()
        //{
        //    if (Status != TournamentStatus.ENDED)
        //        return [];
            
        //    int maxPushUps = 0;
        //    List<User> winners = [];

        //    foreach (var user in Participants)
        //    {
        //        int totalPushUps = user.GetTotalCount();
        //        if (totalPushUps > maxPushUps)
        //        {
        //            maxPushUps = totalPushUps;
        //            winners.Clear();
        //            winners.Add(user.User);
        //        }
        //        else if (totalPushUps == maxPushUps)
        //        {
        //            winners.Add(user.User);
        //        }
        //    }
        //    return winners;
        //}

        //public void CalculateResults()
        //{
        //    if (Status == TournamentStatus.RUNNING)
        //        return;

        //    var winners = GetWinners();

        //    if (Participants.Count == 0 || winners.Count == 0)
        //        return;
        //    // Update ELO for winners
        //    if (winners.Count == 1)
        //    {
        //        winners[0].ELO += ConstantsSettings.ELOWinPoints;
        //    }
        //    else if (winners.Count > 1)
        //    {
        //        foreach (var winner in winners)
        //        {
        //            winner.ELO += ConstantsSettings.ELODrawPoints;
        //        }
        //    }
        //    // Deduct points from non-winners
        //    foreach (var participant in Participants)
        //    {
        //        if (!winners.Any(w => w.Username == participant.User.Username))
        //        {
        //            participant.User.ELO += ConstantsSettings.ELOLosePoints;
        //        }
        //    }
        //}

        //public void AddParticipant(User user, ExerciseEntry entry)
        //{
        //    if (!IsTournamentRunning())
        //        return;

        //    var existingParticipant = Participants.FirstOrDefault(p => p.User == user);
        //    if (existingParticipant != null)
        //    {
        //        existingParticipant.Entries.Add(entry);
        //    }
        //    else
        //    {
        //        Participant newParticipant = new()
        //        {
        //            User = user,
        //            Entries = [entry]
        //        };
        //        Participants.Add(newParticipant);
        //    }
        //}

        //public bool IsTournamentRunning()
        //{
        //    return Status == TournamentStatus.RUNNING && DateTime.UtcNow < EndTime;
        //}
        //public void EndTournament()
        //{
        //    Status = TournamentStatus.ENDED;
        //    EndTime = DateTime.UtcNow;
        //}
    }
}
