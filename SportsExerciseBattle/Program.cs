// See https://aka.ms/new-console-template for more information
using Npgsql;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Utilities;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;
using System.Text.Json;
using SportsExerciseBattle.Infrastructure;
using SportsExerciseBattle.Repositories;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Utilities.Exceptions;
using SportsExerciseBattle.API;

namespace SportsExerciseBattle;

class Program
{
    static void Main(string[] args)
    {
        DatabaseInitializer.InitializeDatabase();
        //Console.WriteLine("SELECT u.username, u.elo, " +
        //        "COALESCE(SUM(ee.count), 0) AS total_reps, " +
        //        "FROM Users u " +
        //        "LEFT JOIN ExerciseEntries ee ON u.username = ee.username " +
        //        "WHERE ee.tournament_id = @tournamentId " +
        //        "GROUP BY u.username, u.elo;");

        var serviceProvider = DIConfig.ConfigureServices();
        var serverController = new ServerController(serviceProvider);
        serverController.ListenAsync();

        var userRepo = new UserRepository();
        var exerRepo = new ExerciseEntryRepository();
        var statServ = new StatisticsService(userRepo, exerRepo);
        var tourneyRepo = new TournamentRepository();
        var tourneyServ = new TournamentService(userRepo, exerRepo, tourneyRepo);
        AuthenticationService authServ = new(userRepo);

        var overview = tourneyServ.GetTournamentsOverview();
        if (overview != null)
        {
            foreach (var i in overview)
                Console.WriteLine($"Start: {i.StartTime}, status: {i.Status}, user: {i.Username}, entries: {i.TotalEntries}, reps: {i.TotalReps}");
        }
        try
        {
            authServ.Register("LUkas", "mypw");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException(ex);
        }
        userRepo.UpdateUserToken("LUkas", "invalid");
        var LUkas = userRepo.GetUserByUsername("LUkas");
        Console.WriteLine($"{LUkas}, token: {LUkas.AuthToken}.");
        
        if(LUkas.ELO < 100)
        {
            userRepo.UpdateUserElo(LUkas.Username, LUkas.ELO+2);
        }
        else
        {
            userRepo.UpdateUserElo(LUkas.Username, LUkas.ELO-23);
        }
        if (LUkas.Name == "Lukasz")
        {
            userRepo.UpdateUserProfile(LUkas.Username, new UserProfileDTO
            {
                Name = "Lukas",
                Bio = "haha",
                Image = "|:"
            });

        }
        else
        {
            userRepo.UpdateUserProfile(LUkas.Username, new UserProfileDTO
            {
                Name = "Lukasz",
                Bio = "hohoo",
                Image = "D:"
            });
        }
        authServ.Login(LUkas.Username, "mypw");
        LUkas = userRepo.GetUserByUsername("LUkas");
        Console.WriteLine($"{LUkas}, token: {LUkas.AuthToken}.");

        foreach (var i in exerRepo.GetExerciseEntriesByUsername("david_brown"))
        {
            Console.WriteLine($"HISTORY exercisename: {i.Name}, count: {i.Count}, duration: {i.DurationInSeconds}");
        }
        tourneyServ.AddExerciseEntry("LUkas",
            new ExerciseEntry
            {
                Name = "PushUps",
                Count = 18,
                DurationInSeconds = 7,
                Timestamp = DateTime.UtcNow
            });
        foreach (var i in exerRepo.GetExerciseEntriesByUsername("david_brown"))
        {
            Console.WriteLine($"HISTORY exercisename: {i.Name}, count: {i.Count}, duration: {i.DurationInSeconds}");
        }
        Console.WriteLine("-------------------------");
        Console.WriteLine(exerRepo.GetStats(LUkas.Username));
        Console.WriteLine("-------------------------");
        var davidStats = statServ.GetUserStats("LUkas");
        Console.WriteLine($"USERSTATS {davidStats.Username}: elo = {davidStats.EloRating}, total PUs: { davidStats.TotalPushUps}");


        try
        {
            //authServ.Register("harald", "bauer");
            //authServ.Register("Heike", "makka");
            //authServ.Login("Heike", "titi");
            foreach(var i in exerRepo.GetScores())
            {
                Console.WriteLine($"SCOREBOARD user: {i.Username}, elo: {i.EloRating}, pushups: {i.TotalPushUps}, entries: {i.TotalEntries}");
            }
        }
        catch(Exception ex)
        {
            ExceptionHandler.HandleException(ex);
        }
        try
        { 
            //authServ.Login("Heike", "makka");
            //authServ.Login("harald", "bauer");
            //authServ.Login("Keke", "titi");
        }
        catch (Exception ex)
        {
            ExceptionHandler.HandleException(ex);
        }

        //// Create a new user
        //User user1 = new()
        //{
        //    Username = "john_doe",
        //    Password = "password123",
        //    Name = "John Doe",
        //    Bio = "Fitness enthusiast",
        //    Image = "profile.jpg"
        //};

        //// Create another user
        //User user2 = new()
        //{
        //    Username = "jane_doe",
        //    Password = "password456",
        //    Name = "Jane Doe",
        //    Bio = "Yoga lover",
        //    Image = "profile2.jpg"
        //};

        //// Create a new exercise entry
        //ExerciseEntry entry1 = new()
        //{
        //    ID = 1,
        //    Name = ConstantsSettings.DefaultExerciseName,
        //    Count = 20,
        //    DurationInSeconds = 60,
        //    Timestamp = DateTime.UtcNow,
        //};

        //// Create another exercise entry
        //ExerciseEntry entry2 = new()
        //{
        //    ID = 2,
        //    Name = ConstantsSettings.DefaultExerciseName,
        //    Count = 30,
        //    DurationInSeconds = 90,
        //    Timestamp = DateTime.UtcNow,
        //};

        //// create bonus entry
        //ExerciseEntry bonusEntry = new()
        //{
        //    ID = 3,
        //    Name = ConstantsSettings.DefaultExerciseName,
        //    Count = 10,
        //    DurationInSeconds = 30,
        //    Timestamp = DateTime.UtcNow,
        //};


        //// create a new tournament
        //Tournament tournament = new()
        //{
        //    ID = 1,
        //    StartTime = DateTime.UtcNow,
        //    EndTime = DateTime.UtcNow.AddMinutes(7),
        //    Status = TournamentStatus.RUNNING
        //};

        //// Add users to the tournament
        //user1.AddExerciseEntry(entry1);
        //tournament.AddParticipant(user1, entry1);
        //user2.AddExerciseEntry(entry2);
        //tournament.AddParticipant(user2, entry2);

        //// End the tournament
        //tournament.Status = TournamentStatus.ENDED;
        //// Calculate results
        //tournament.CalculateResults();
        //// print winners to console
        //Console.WriteLine("Winners:");
        //foreach (var winner in tournament.GetWinners())
        //{
        //    Console.WriteLine($"Username: {winner.Username}, ELO: {winner.ELO}");
        //}

        //// print participants to console
        //Console.WriteLine("Participants:");
        //foreach (var participant in tournament.Participants)
        //{
        //    Console.WriteLine($"Username: {participant.User.Username}, Total Push Ups: {participant.GetTotalCount()}");
        //}

        //user1.AddExerciseEntry(bonusEntry);

        //// print user1 entries to console
        //Console.WriteLine("User1 Entries:");
        //foreach (var entry in user1.Entries)
        //{
        //    Console.WriteLine($"Entry ID: {entry.ID}, Name: {entry.Name}, Count: {entry.Count}, Duration: {entry.DurationInSeconds} seconds, Timestamp: {entry.Timestamp}");
        //}

        //string json = JsonSerializer.Serialize(user1);
        //Console.WriteLine("Serialized User1:");
        //Console.WriteLine(json);
        //Console.WriteLine("----------");
        //Console.WriteLine(user1);
        //Console.WriteLine("----------");
        //var deserializedUser = JsonSerializer.Deserialize<User>(json);
        //Console.WriteLine(deserializedUser?.Name);

    }

    // docker run -d --rm --name postgressdb -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -p 5432:5432 -v sebdata:/var/lib/postgresql/data postgres
}