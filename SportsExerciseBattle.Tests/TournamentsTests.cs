using NSubstitute;
using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Tests
{
    public class TournamentsTests
    {
        private readonly IUserRepository _userRepository;
        private readonly IExerciseEntryRepository _exerciseEntryRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly TournamentService _tournamentService;
        public TournamentsTests()
        {
            TournamentService.ResetInstance();
            _userRepository = Substitute.For<IUserRepository>();
            _exerciseEntryRepository = Substitute.For<IExerciseEntryRepository>();
            _tournamentRepository = Substitute.For<ITournamentRepository>();
            _tournamentService = TournamentService.GetInstance(_userRepository, _exerciseEntryRepository, _tournamentRepository);
        }
        [Test]
        public void GetTournamentsOverview_ReturnsCorrectData()
        {
            // Arrange
            var expectedOverview = new List<TournamentStatsDTO>
            {
                new TournamentStatsDTO
                {
                    Username = "user1",
                    Status = TournamentStatus.RUNNING,
                    TotalReps = 100
                }
            };

            _tournamentRepository.GetTournamentsOverview().Returns(expectedOverview);

            var tournamentService = new TournamentService(
                _userRepository,
                _exerciseEntryRepository,
                _tournamentRepository);

            // Act
            var result = tournamentService.GetTournamentsOverview();

            // Assert
            Assert.That(1, Is.EqualTo(result.Count));
            Assert.That("user1", Is.EqualTo(result[0].Username));
            Assert.That(100, Is.EqualTo(result[0].TotalReps));
        }
    }
}
