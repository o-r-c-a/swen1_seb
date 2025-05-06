using NSubstitute;
using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;

namespace SportsExerciseBattle.Tests
{
    public class TokenValidationTests
    {
        private AuthenticationService _authenticationService;
        private IUserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            AuthenticationService.ResetInstance();
            _userRepository = Substitute.For<IUserRepository>();
            _authenticationService = AuthenticationService.GetInstance(_userRepository);
        }


        [Test]
        public void EnsureAuthenticated_PathUsernameMismatch_ShouldThrowUnauthorizedException()
        {
            var authToken = "user-sebToken";
            var pathUsername = "otherUser";

            var user = new User(pathUsername, "password") { AuthToken = "otherUser-sebToken" };
            _userRepository.GetUserByUsername(pathUsername).Returns(user);

            Assert.Throws<UnauthorizedAccessException>(() => _authenticationService.AuthenticateAction(authToken, pathUsername));
        }
    }
}
