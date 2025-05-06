using NSubstitute.Core;
using SportsExerciseBattle.Models;
using SportsExerciseBattle.Models.Messages;
using SportsExerciseBattle.Services;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsExerciseBattle.API.Endpoints
{
    public class UsersEndpoint : IHttpEndpoint
    {
        private readonly AuthenticationService _authenticationService;
        private readonly StatisticsService _statisticsService;
        private readonly UserService _userService;

        public UsersEndpoint(AuthenticationService authenticationService, StatisticsService statisticsService, UserService userService)
        {
            _authenticationService = authenticationService;
            _statisticsService = statisticsService;
            _userService = userService;
        }
        public Response HandleRequest(
            string method,
            string path,
            string? body,
            Dictionary<string, string> headers,
            Dictionary<string, string>? routeParams = null
            )
        {
            switch (method)
            {
                case "POST":
                    return RegisterUser(body!);
                case "GET" when routeParams != null && routeParams.ContainsKey("username"):
                    return GetUserData(routeParams["username"], headers);
                case "PUT" when routeParams != null && routeParams.ContainsKey("username"):
                    return UpdateUserData(routeParams["username"], body!, headers);
            }
            // Handle the request here
            return new Response(405, "Method not allowed.");
        }
        private Response RegisterUser(string body)
        {
            try
            {
                User? user = JsonSerializer.Deserialize<User>(body);
                if (user == null)
                {
                    return new Response(400, "[Register] Invalid request body.");
                }
                _authenticationService.Register(user.Username, user.Password);
                return new Response(201, "[Register] User registered successfully.");
            }
            catch (UserAlreadyExistsException)
            {
                return new Response(409, "[Register] User already exists.");
            }
            catch (Exception ex)
            {
                return new Response(500, $"[Register] Internal server error: {ex.Message}");
            }
        }
        private Response GetUserData(string username, Dictionary<string, string> headers)
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
                _authenticationService.AuthenticateAction(token, username);
                var userProfile = _userService.GetUserProfile(username);
                var jsonUserProfile = JsonSerializer.Serialize(userProfile, new JsonSerializerOptions
                {
                    WriteIndented = true,
                });
                return new Response(200, jsonUserProfile);
            }
            catch (Exception ex)
            {
                return new Response(401, "Unauthorized.");
            }
        }
        private Response UpdateUserData(string username, string body, Dictionary<string, string> headers)
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
                _authenticationService.AuthenticateAction(token, username);
                var request = JsonSerializer.Deserialize<UserProfileDTO>(body);
                Console.WriteLine(request);
                _userService.UpdateUserProfile(username, request!);

                if (request == null)
                {
                    return new Response(400, "Invalid request body.");
                }
                return new Response(200, "User data updated successfully.");
            }
            catch (Exception ex)
            {
                return new Response(401, "Unauthorized.");
            }
        }
    }
}
