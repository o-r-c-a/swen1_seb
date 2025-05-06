using Microsoft.Extensions.DependencyInjection;
using SportsExerciseBattle.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.API.Endpoints;

namespace SportsExerciseBattle.API
{
    public static class DIConfig
    {
        public static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            // Repositories (Transient)
            services.AddTransient<UserRepository>();
            services.AddTransient<ExerciseEntryRepository>();
            services.AddTransient<TournamentRepository>();

            // Services (Singleton)
            services.AddSingleton<AuthenticationService>(sp =>
                AuthenticationService.GetInstance(sp.GetRequiredService<UserRepository>()));
            services.AddSingleton<StatisticsService>(sp =>
                StatisticsService.GetInstance(sp.GetRequiredService<UserRepository>(),
                                              sp.GetRequiredService<ExerciseEntryRepository>()));
            services.AddSingleton<TournamentService>(sp =>
                TournamentService.GetInstance(sp.GetRequiredService<UserRepository>(),
                                                sp.GetRequiredService<ExerciseEntryRepository>(),
                                                sp.GetRequiredService<TournamentRepository>()));
            services.AddSingleton<UserService>(sp =>
                UserService.GetInstance(sp.GetRequiredService<UserRepository>(),
                                        sp.GetRequiredService<ExerciseEntryRepository>()));

            // Endpoints (Transient)
            services.AddTransient<UsersEndpoint>();
            services.AddTransient<SessionsEndpoint>();
            services.AddTransient<StatsEndpoint>();
            services.AddTransient<ScoreEndpoint>();
            services.AddTransient<TournamentEndpoint>();
            services.AddTransient<HistoryEndpoint>();

            return services.BuildServiceProvider();
        }
    }
}
