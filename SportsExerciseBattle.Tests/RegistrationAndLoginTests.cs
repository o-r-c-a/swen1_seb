using NSubstitute;
using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Models;
using Microsoft.AspNetCore.Identity;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;

namespace SportsExerciseBattle.Tests
{
    public class Tests
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
        public void Register_UserDoesNotExist_ShouldRegisterSuccessfully()
        {
            // Arrange
            var username = "testuser";
            var password = "testpassword";

            // Act
            _userRepository.UserExists(username).Returns(false);
            _authenticationService.Register(username, password);

            // Assert
            _userRepository.Received(1).AddUser(Arg.Is<User>(u => u.Username == username && 
            !string.IsNullOrWhiteSpace(u.Password)));
        }
        [Test]
        public void Register_UsernameAlreadyExists_ShouldThrowUserAlreadyExistsException()
        {
            // Arrange
            var existingUsername = "existinguser";
            var password = "anypassword";

            _userRepository.UserExists(existingUsername).Returns(true);

            // Act & Assert
            Assert.Throws<UserAlreadyExistsException>(() => _authenticationService.Register(existingUsername, password));

            // Verify that AddUser was never called
            _userRepository.DidNotReceive().AddUser(Arg.Any<User>());
        }

        [Test]

        public void Login_UserExistsAndPasswordIsCorrect_ShouldLoginSuccessfully()
        {
            // Arrange
            var username = "testuser";
            var password = "correctpassword";
            var hasher = new PasswordHasher<User>();
            var user = new User(username, "");
            var hashedPassword = hasher.HashPassword(user, password);

            user.Password = hashedPassword;

            _userRepository.GetUserByUsername(username).Returns(user);

            // Act - this should not throw any exceptions
            _authenticationService.Login(username, password);

            // Assert
            // Verify that UpdateUserToken was called with the correct parameters
            _userRepository.Received(1).UpdateUserToken(
                Arg.Is<string>(u => u == username),
                Arg.Is<string>(t => t == $"{username}-sebToken")
            );
        }
    }
}
