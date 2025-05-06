using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.API.Endpoints.Initializer
{
    public static class EndpointInitializer
    {
        public static void InitializeEndpoints(IServiceProvider serviceProvider, HttpRequestHandler requestHandler)
        {
            var usersEndpoint = serviceProvider.GetRequiredService<UsersEndpoint>();
            var sessionsEndpoint = serviceProvider.GetRequiredService<SessionsEndpoint>();
            var statsEndpoint = serviceProvider.GetRequiredService<StatsEndpoint>();
            var scoreEnpoint = serviceProvider.GetRequiredService<ScoreEndpoint>();
            var tournamentEndpoint = serviceProvider.GetRequiredService<TournamentEndpoint>();
            var historyEnpoint = serviceProvider.GetRequiredService<HistoryEndpoint>();


            // add endpoints to requestHandler
            requestHandler.AddEndpoint("/users", usersEndpoint);
            requestHandler.AddEndpoint("/users/{username}", usersEndpoint);
            requestHandler.AddEndpoint("/sessions", sessionsEndpoint);
            requestHandler.AddEndpoint("/stats", statsEndpoint);
            requestHandler.AddEndpoint("/score", scoreEnpoint);
            requestHandler.AddEndpoint("/history", historyEnpoint);
            requestHandler.AddEndpoint("/tournament", tournamentEndpoint);
        }
    }
}
