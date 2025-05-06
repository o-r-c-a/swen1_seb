using SportsExerciseBattle.Interfaces;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Services
{
    public class TournamentService : ITournamentService
    {
        private static TournamentService? _instance;
        private readonly IUserRepository _userRepository;
        private readonly IExerciseEntryRepository _exerciseEntryRepository;
        private readonly ITournamentRepository _tournamentRepository;
        public TournamentService(IUserRepository userRepository, IExerciseEntryRepository exerciseEntryRepository, ITournamentRepository tournamentRepository)
        {
            _userRepository = userRepository;
            _exerciseEntryRepository = exerciseEntryRepository;
            _tournamentRepository = tournamentRepository;
        }
        public static TournamentService GetInstance(IUserRepository userRepository, IExerciseEntryRepository exerciseEntryRepository, ITournamentRepository tournamentRepository)
        {
            if (_instance == null)
            {
                _instance = new TournamentService(userRepository, exerciseEntryRepository, tournamentRepository);
            }
            return _instance;
        }
        public static void ResetInstance() => _instance = null;

        public List<TournamentStatsDTO>? GetTournamentsOverview()
        {
            var tournaments = _tournamentRepository.GetTournamentsOverview();
            if (tournaments == null || tournaments.Count < 1)
                return null;
            return tournaments;
        }

        // check wether theres a tournament running (always check)
        public Tournament? GetRunningTournament()
        {
            var currTournament = _tournamentRepository.GetCurrentTournament();
            if (currTournament == null)
                return null;
            if (currTournament.EndTime < DateTime.UtcNow)
            {
                FinishTournament(currTournament);
                return null;
            }
            return currTournament;
        }
        private void FinishTournament(Tournament tournament)
        {
            tournament.Status = TournamentStatus.ENDED;
            CalculateResults(tournament);
            _tournamentRepository.UpdateTournamentStatus(tournament);
        }

        private void CalculateResults(Tournament tournament)
        {
            var participants = _tournamentRepository.RetrieveParticipants(tournament);
            if (participants == null || participants.Count < 1)
                return;

            // get the winners
            var winners = RetrieveTournamentWinners(participants);
            if (winners.Count == 1)
            {
                _userRepository.UpdateUserElo(winners[0], participants.First(u => u.Username == winners[0]).EloRating += ConstantsSettings.ELOWinPoints);
            }
            else if (winners.Count > 1)
            {
                foreach (var winner in winners)
                {
                    _userRepository.UpdateUserElo(winner, participants.First(u => u.Username == winners[0]).EloRating += ConstantsSettings.ELODrawPoints);
                }
            }
            // deduct points from non-winners
            foreach (var participant in participants)
            {
                if (!winners.Contains(participant.Username))
                {
                    _userRepository.UpdateUserElo(participant.Username, participant.EloRating += ConstantsSettings.ELOLosePoints);
                }
            }
        }

        private List<string> RetrieveTournamentWinners(List<UserScoreDTO> participants)
        {
            List<string> winners = [];
            int? maxReps = 0;
            foreach (var entry in participants)
            {
                if (entry.TotalPushUps > maxReps)
                {
                    maxReps = entry.TotalPushUps;
                    winners.Clear();
                    winners.Add(entry.Username);
                }
                else if (entry.TotalPushUps == maxReps)
                {
                    winners.Add(entry.Username);
                }
            }
            return winners;
        }

        private Tournament StartTournament()
        {
            var tournament = new Tournament();
            tournament.StartTime = DateTime.UtcNow;
            tournament.EndTime = tournament.StartTime.AddSeconds(ConstantsSettings.DefaultTournamentTime);
            tournament.Status = TournamentStatus.RUNNING;
            _tournamentRepository.AddTournament(tournament);
            Console.WriteLine("Tournament started!");
            return _tournamentRepository.GetCurrentTournament() ?? throw new Exception("Couldn't create tournament.");
        }

        public void AddExerciseEntry(string token, ExerciseEntry entry)
        {
            var tournament = GetRunningTournament();
            var user = _userRepository.GetUserByToken(token);
            if (user == null)
            {
                throw new Exception("Invalid token");
            }
            // check if the tournament is running
            if (tournament != null)
            {
                _exerciseEntryRepository.AddEntry(user.Username, entry, tournament.ID);
                Console.WriteLine($"Added Exercise to current tournament (id: {tournament.ID})");
            }
            else
            {
                tournament = StartTournament();
                _exerciseEntryRepository.AddEntry(user.Username, entry, tournament.ID);
                Console.WriteLine($"Started new tournament (id: {tournament.ID}) & added Exercise to it.");
            }
        }
    }
}
