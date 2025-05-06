using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class Participant
    {
        public required User User { get; set; }
        public List<ExerciseEntry> Entries { get; set; } = [];

        public int GetTotalCount()
        {
            return Entries?.Sum(e => e.Count) ?? 0;
        }
    }
}
