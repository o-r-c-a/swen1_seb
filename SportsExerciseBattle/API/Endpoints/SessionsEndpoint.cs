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
    public class SessionsEndpoint : IHttpEndpoint
    {
        private readonly AuthenticationService _authService;

        public SessionsEndpoint(AuthenticationService authService)
        {
            _authService = authService;
        }

        // This method now returns a ResponseObject
        public Response HandleRequest(
            string method,
            string path,
            string? body,
            Dictionary<string, string> headers,
            Dictionary<string, string>? routeParams = null)
        {
            switch (method)
            {
                case "POST":
                    return LoginUser(body!);
                default:
                    return new Response(405, "Method not allowed.");
            }
        }

        private Response LoginUser(string body)
        {
            try
            {
                var user = JsonSerializer.Deserialize<User>(body);
                if (user == null)
                    throw new JsonException("Invalid JSON payload.");

                _authService.Login(user.Username, user.Password);

                return new Response(200, $"Login successful.");
            }
            catch (Exception ex)
            {
                return ExceptionHandler.HandleException(ex);
            }
        }
    }
}
