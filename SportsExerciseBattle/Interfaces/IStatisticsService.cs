using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SportsExerciseBattle.Models;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Interfaces
{
    public interface IStatisticsService
    {
        List<UserScoreDTO> GetScoreboard();
        UserScoreDTO GetUserStats(string token);
    }
}
