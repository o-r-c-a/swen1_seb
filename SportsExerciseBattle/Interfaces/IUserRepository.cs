using SportsExerciseBattle.Models;

namespace SportsExerciseBattle.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User? GetUserByUsername(string username);
        User? GetUserByToken(string token);
        void UpdateUserElo(string username, int elo);
        void UpdateUserProfile(string username, UserProfileDTO userProfile);
        void UpdateUserToken(string username, string token);
        bool UserExists(string username);
        bool TokenExists(string token);
        UserProfileDTO GetUserProfile(string username);
        Dictionary<string, int> GetAllUsersElo();
    }
}
