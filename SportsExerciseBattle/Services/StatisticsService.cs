using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Services
{
    public class StatisticsService : IStatisticsService
    {
        // Singleton instance
        private static StatisticsService? _instance;
        private readonly IUserRepository _userRepository;
        private readonly IExerciseEntryRepository _exerciseEntryRepository;

        public StatisticsService(IUserRepository userRepository, IExerciseEntryRepository exerciseEntryRepository)
        {
            _userRepository = userRepository;
            _exerciseEntryRepository = exerciseEntryRepository;
        }

        public static StatisticsService GetInstance(IUserRepository userRepository, IExerciseEntryRepository exerciseEntryRepository)
        {
            if (_instance == null)
            {
                _instance = new StatisticsService(userRepository, exerciseEntryRepository);
            }
            return _instance;
        }
        public static void ResetInstance() => _instance = null;
        public UserScoreDTO GetUserStats(string token)
        {
            // Extract username from token
            var user = _userRepository.GetUserByToken(token);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid token");
            }
            var userStats = _exerciseEntryRepository.GetStats(user.Username);
            if (userStats != null)
            {
                userStats.Username = user.Username;
                return userStats;
            }
            throw new Exception("Couldn't load user statistics!");
        }

        public List<UserScoreDTO> GetScoreboard()
        {
            var scores = _exerciseEntryRepository.GetScores();
            if (scores.Count < 1)
            {
                throw new Exception("No scores");
            }
            return [.. scores.OrderByDescending(s => s.TotalPushUps)];
        }
    }
}
