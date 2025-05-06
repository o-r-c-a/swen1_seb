using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class ExerciseEntry
    {
        public int ID { get; set; }
        public required string Name { get; set; }
        public int Count { get; set; }
        public int DurationInSeconds { get; set; }
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
