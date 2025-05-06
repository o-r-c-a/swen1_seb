using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Interfaces
{
    public interface ITournamentRepository
    {
        Tournament? GetCurrentTournament();
        public Tournament? GetTournamentById(int id);
        void AddTournament(Tournament tournament);
        void UpdateTournamentStatus(Tournament tournament);

        List<TournamentStatsDTO>? GetCurrentTournamentOverview();
        List<TournamentStatsDTO> GetTournamentsOverview();
        List<UserScoreDTO> RetrieveParticipants(Tournament tournament);
    }
}
