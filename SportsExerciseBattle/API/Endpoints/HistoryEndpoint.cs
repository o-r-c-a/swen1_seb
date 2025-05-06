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
    public class HistoryEndpoint : IHttpEndpoint
    {
        private readonly AuthenticationService _authenticationService;
        private readonly UserService _userService;
        private readonly TournamentService _tournamentService;

        public HistoryEndpoint(AuthenticationService authenticationService, UserService userService, TournamentService tournamentService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
            _tournamentService = tournamentService;
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
                    return GetHistory(headers);
                case "POST":
                    return AddEntry(headers, body!);
                default:
                    return new Response(405, "Method not allowed.");
            }
        }
        private Response GetHistory(Dictionary<string, string> headers)
        {
            if (!headers.ContainsKey("Authorization"))
            {
                return new Response(401, "Unauthorized");
            }
            var token = headers["Authorization"];
            try
            {
                _authenticationService.AuthenticateAction(token);
                var history = _userService.GetUserEntries(token);
                if(history == null)
                {
                    return new Response(404, "No history yet.");
                }
                var jsonHistory = JsonSerializer.Serialize(history, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                return new Response(200, jsonHistory);
            }
            catch (Exception ex)
            {
                return ExceptionHandler.HandleException(ex);
            }
        }

        private Response AddEntry(Dictionary<string, string> headers, string body)
        {
            if (!headers.ContainsKey("Authorization"))
            {
                return new Response(401, "Unauthorized");
            }
            var token = headers["Authorization"];
            try
            {
                _authenticationService.AuthenticateAction(token);
                var entry = JsonSerializer.Deserialize<ExerciseEntry>(body);
                if (entry == null)
                {
                    return new Response(400, "Invalid request body.");
                }
                _tournamentService.AddExerciseEntry(token, entry);
                return new Response(201, "Entry added successfully.");
            }
            catch (Exception ex)
            {
                return ExceptionHandler.HandleException(ex);
            }
        }
    }
}
