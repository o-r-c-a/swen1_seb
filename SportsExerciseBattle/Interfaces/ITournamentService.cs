using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Interfaces
{
    public interface ITournamentService
    {
        Tournament? GetRunningTournament();
        void AddExerciseEntry(string username, ExerciseEntry entry);
    }
}
