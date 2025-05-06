using Microsoft.AspNetCore.Identity;
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
    public class UserService : IUserService
    {
        private static UserService? _instance;
        private readonly IUserRepository _userRepository;
        private readonly IExerciseEntryRepository _exerciseEntries;

        public UserService(IUserRepository userRepository, IExerciseEntryRepository exerciseEntryRepository)
        {
            _userRepository = userRepository;
            _exerciseEntries = exerciseEntryRepository;
        }
        public static UserService GetInstance(IUserRepository userRepository, IExerciseEntryRepository exerciseEntryRepository)
        {
            if (_instance == null)
            {
                _instance = new UserService(userRepository, exerciseEntryRepository);
            }
            return _instance;
        }
        public static void ResetInstance() => _instance = null;

        public void UpdateUserProfile(string username, UserProfileDTO userData)
        {
            if (userData == null)
            {
                throw new ArgumentNullException($"{username}: User profile cannot be null");
            }
            _userRepository.UpdateUserProfile(username, userData);
        }
        public UserProfileDTO GetUserProfile(string username)
        {
            var userProfile = _userRepository.GetUserProfile(username);
            if (userProfile == null)
            {
                throw new Exception($"{username}: User profile not found");
            }
            return userProfile;
        }

        public List<ExerciseEntry>? GetUserEntries(string token)
        {
            var user = _userRepository.GetUserByToken(token);
            if (user == null)
            {
                throw new UnauthorizedException("Invalid token");
            }
            var userEntries = _exerciseEntries.GetExerciseEntriesByUsername(user.Username);
            return userEntries;
        }
    }
}
