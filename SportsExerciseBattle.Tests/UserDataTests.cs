using NSubstitute;
using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace SportsExerciseBattle.Tests
{
    public class UserDataTests
    {
        private StatisticsService _statisticsService;
        private IUserRepository _userRepository;
        private IExerciseEntryRepository _exerciseEntryRepository;
        private ITournamentRepository _tournamentRepository;

        [SetUp]
        public void Setup()
        {
            StatisticsService.ResetInstance();
            _userRepository = Substitute.For<IUserRepository>();
            _exerciseEntryRepository = Substitute.For<IExerciseEntryRepository>();
            _tournamentRepository = Substitute.For<ITournamentRepository>();
            _statisticsService = StatisticsService.GetInstance(_userRepository, _exerciseEntryRepository);
        }

        [Test]
        public void GetUserStats_ValidToken_ReturnsUserStats()
        {
            // Arrange
            var testUser = new User { Username = "testuser", AuthToken = "testuser-sebToken" };
            var expectedStats = new UserScoreDTO { EloRating = 1000, TotalPushUps = 100 };

            _userRepository.GetUserByToken("testuser-sebToken").Returns(testUser);
            _exerciseEntryRepository.GetStats("testuser").Returns(expectedStats);

            // Act
            var result = _statisticsService.GetUserStats("testuser-sebToken");

            // Assert
            Assert.That(testUser.Username, Is.EqualTo(result.Username));
            Assert.That(1000, Is.EqualTo(result.EloRating));
            Assert.That(100, Is.EqualTo(result.TotalPushUps));
        }
        [Test]
        public void GetScoreboard_ReturnsOrderedScores()
        {
            // Arrange
            var scores = new List<UserScoreDTO>
            {
                new UserScoreDTO { Username = "user1", TotalPushUps = 50 },
                new UserScoreDTO { Username = "user2", TotalPushUps = 100 },
                new UserScoreDTO { Username = "user3", TotalPushUps = 75 }
            };

            _exerciseEntryRepository.GetScores().Returns(scores);

            var statisticsService = new StatisticsService(_userRepository, _exerciseEntryRepository);

            // Act
            var result = statisticsService.GetScoreboard();

            // Assert
            Assert.That(3, Is.EqualTo(result.Count));
            Assert.That("user2", Is.EqualTo(result[0].Username)); // Highest score first
            Assert.That("user3", Is.EqualTo(result[1].Username));
            Assert.That("user1", Is.EqualTo(result[2].Username));
        }
        [Test]
        public void GetUserProfile_ExistingUser_ReturnsProfile()
        {
            // Arrange
            var expectedProfile = new UserProfileDTO
            {
                Name = "Test User",
                Bio = "Test Bio",
                Image = "test.jpg"
            };

            _userRepository.GetUserProfile("testuser").Returns(expectedProfile);

            var userService = new UserService(_userRepository, _exerciseEntryRepository);

            // Act
            var result = userService.GetUserProfile("testuser");

            // Assert
            Assert.That("Test User", Is.EqualTo(result.Name));
            Assert.That("Test Bio", Is.EqualTo(result.Bio));
            Assert.That("test.jpg", Is.EqualTo(result.Image));
        }
        [Test]
        public void UpdateUserProfile_ValidData_CallsRepository()
        {
            // Arrange
            var profileData = new UserProfileDTO
            {
                Name = "Updated Name",
                Bio = "Updated Bio",
                Image = "updated.jpg"
            };

            var userService = new UserService(_userRepository, _exerciseEntryRepository);

            // Act
            userService.UpdateUserProfile("testuser", profileData);

            // Assert
            _userRepository.Received(1).UpdateUserProfile("testuser", profileData);
        }
        [Test]
        public void GetUserEntries_ValidToken_ReturnsEntries()
        {
            // Arrange
            var testUser = new User { Username = "testuser", AuthToken = "testuser-sebToken" };
            var expectedEntries = new List<ExerciseEntry>
            {
                new ExerciseEntry { Name = "Push-ups", Count = 20, DurationInSeconds = 60 },
                new ExerciseEntry { Name = "Sit-ups", Count = 15, DurationInSeconds = 45 }
            };

            _userRepository.GetUserByToken("testuser-sebToken").Returns(testUser);
            _exerciseEntryRepository.GetExerciseEntriesByUsername("testuser").Returns(expectedEntries);

            var userService = new UserService(_userRepository, _exerciseEntryRepository);

            // Act
            var result = userService.GetUserEntries("testuser-sebToken");

            // Assert
            Assert.That(2, Is.EqualTo(result.Count));
            Assert.That("Push-ups", Is.EqualTo(result[0].Name));
            Assert.That(20, Is.EqualTo(result[0].Count));
        }
    }
}
