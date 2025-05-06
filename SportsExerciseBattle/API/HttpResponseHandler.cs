using SportsExerciseBattle.Models.Messages;
using SportsExerciseBattle.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.API
{
    public class HttpResponseHandler
    {
        public void SendResponse(StreamWriter writer, Response? response)
        {
            if (response == null)
            {
                response = new Response(500, "Internal server error");
            }

            int statusCode = response.StatusCode;

            string responseBody = response.ResponseBody is string
                ? response.ResponseBody
                : Helpers.CreateStandardJsonResponse(response.ResponseBody);

            writer.WriteLine($"HTTP/1.1 {statusCode}");
            writer.WriteLine("Content-Type: application/json");
            writer.WriteLine("Content-Length: " + responseBody.Length);
            writer.WriteLine();
            writer.WriteLine(responseBody);

            writer.Flush();

        }
    }
}
