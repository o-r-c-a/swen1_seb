using SportsExerciseBattle.Models.Messages;
using SportsExerciseBattle.Utilities.Exceptions.CustomExceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Utilities.Exceptions
{
    public static class ExceptionHandler
    {
        public static Response HandleException(Exception ex)
        {
            Console.WriteLine($"[Exception] {ex.Message}");

            return ex switch
            {
                JsonException => new Response(400, "Invalid JSON structure."),
                UnauthorizedException => new Response(401, "Unauthorized access."),
                UserNotFoundException => new Response(404, "User not found."),
                UserAlreadyExistsException => new Response(409, "User already exists."),
                _ => new Response(500, "An unexpected error occurred.")
            };
        }
    }
}
