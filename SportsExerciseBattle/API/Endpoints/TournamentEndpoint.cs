using SportsExerciseBattle.Models;
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
    internal class TournamentEndpoint : IHttpEndpoint
    {
        private readonly TournamentService _tournamentService;
        private readonly AuthenticationService _authenticationService;

        public TournamentEndpoint(TournamentService tournamentService, AuthenticationService authenticationService)
        {
            _tournamentService = tournamentService;
            _authenticationService = authenticationService;
        }
        public Response HandleRequest(
            string method,
            string path,
            string? body,
            Dictionary<string, string> headers,
            Dictionary<string, string>? routeParams = null
            )
        {
            switch(method)
            {
                case "GET":
                    return GetTournaments(headers);
                default:
                    return new Response(405, "Method not allowed.");
            }
        }
        private Response GetTournaments(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization"))
            {
                return new Response(401, "Unauthorized");
            }
            var token = headers["Authorization"];
            try
            {
                _authenticationService.AuthenticateAction(token);
                      //List<TournamentStatsDTO>?
                var tournaments = _tournamentService.GetTournamentsOverview();
                if (tournaments == null || tournaments.Count < 1)
                {
                    return new Response(404, "No tournaments found.");
                }
                var jsonTournaments = JsonSerializer.Serialize(tournaments, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                return new Response(200, jsonTournaments);
            }
            catch (Exception ex)
            {
                return ExceptionHandler.HandleException(ex);
            }
        }
    }
}
