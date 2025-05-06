using SportsExerciseBattle.Models.Messages;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsExerciseBattle.API.Endpoints
{
    public class ScoreEndpoint : IHttpEndpoint
    {
        private readonly StatisticsService _statisticsService;
        private readonly AuthenticationService _authenticationService;

        public ScoreEndpoint(StatisticsService statisticsService, AuthenticationService authenticationService)
        {
            _statisticsService = statisticsService;
            _authenticationService = authenticationService;
        }
        public Response HandleRequest(
            string method,
            string path,
            string? body,
            Dictionary<string, string> headers,
            Dictionary<string, string>? routeParams = null)
        {
            switch (method)
            {
                case "GET":
                    return GetScoreboard(headers);
                default:
                    return new Response(405, "Method not allowed.");
            }
        }
        private Response GetScoreboard(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization"))
            {
                return new Response(401, "Unauthorized");
            }
            var token = headers["Authorization"];
            try
            {
                _authenticationService.AuthenticateAction(token);
                var scoreboard = _statisticsService.GetScoreboard();

                var jsonScoreboard = JsonSerializer.Serialize(scoreboard, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                return new Response(200, jsonScoreboard);
            }
            catch (Exception ex)
            {
                return ExceptionHandler.HandleException(ex);
            }
        }
    }
}
