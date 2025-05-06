using SportsExerciseBattle.Models.Messages;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Utilities.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsExerciseBattle.API.Endpoints
{
    internal class StatsEndpoint : IHttpEndpoint
    {
        private readonly AuthenticationService _authenticationService;
        private readonly StatisticsService _statisticsService;
        public StatsEndpoint(AuthenticationService authenticationService, StatisticsService statisticsService, UserService userService)
        {
            _authenticationService = authenticationService;
            _statisticsService = statisticsService;
        }
        public Response? HandleRequest(
            string method,
            string path,
            string? body,
            Dictionary<string, string> headers,
            Dictionary<string, string>? routeParams = null
            )
        {
            switch (method)
            {
                case "GET":
                    return GetUserStats(headers);
                default:
                    return new Response(405, "Method not allowed.");
            }
        }

        private Response GetUserStats(Dictionary<string, string> headers)
        {
            // Check if the user is authenticated
            if (!headers.ContainsKey("Authorization"))
            {
                return new Response(401, "Unauthorized. No Authentification Token.");
            }
            // Validate the token
            var token = headers["Authorization"];
            try
            {
                _authenticationService.AuthenticateAction(token);
                var stats = _statisticsService.GetUserStats(token);
                var jsonStats = JsonSerializer.Serialize(stats, new JsonSerializerOptions
                {
                    WriteIndented = true
                });
                return new Response(200, jsonStats);
            }
            catch (Exception ex)
            {
                return ExceptionHandler.HandleException(ex);
            }
        }
    }
}
