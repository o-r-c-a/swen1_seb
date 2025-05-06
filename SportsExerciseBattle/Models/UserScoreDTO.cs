using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models
{
    public class UserScoreDTO
    {
        public string? Username { get; set; }
        public int EloRating { get; set; }
        public int? TotalPushUps { get; set; }
        public int? TotalEntries { get; set; }
    }
}
