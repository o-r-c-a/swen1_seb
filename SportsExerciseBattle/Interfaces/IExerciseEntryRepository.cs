using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Interfaces
{
    public interface IExerciseEntryRepository
    {
        List<ExerciseEntry>? GetExerciseEntriesByUsername(string username);
        List<UserScoreDTO> GetScores();
        UserScoreDTO? GetStats(string username);
        void AddEntry(string username, ExerciseEntry entry, int tournamentID);
    }
}
