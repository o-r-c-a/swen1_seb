using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Interfaces
{
    public interface IUserService
    {
        void UpdateUserProfile(string username, UserProfileDTO userData);
        UserProfileDTO GetUserProfile(string username);
    }
}
