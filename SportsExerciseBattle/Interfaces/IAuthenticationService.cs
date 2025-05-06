using SportsExerciseBattle.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Interfaces
{
    public interface IAuthenticationService
    {
        void Register(string username, string password);
        void Login(string username, string password);
        public void AuthenticateAction(string token, string? username = null);
        string GenerateToken(User user);
        string? GetUserToken(string username);
    }
}
