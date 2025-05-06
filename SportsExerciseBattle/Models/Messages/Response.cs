using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models.Messages
{
    public class Response(int statusCode, string responseBody)
    {
        public int StatusCode { get; set; } = statusCode;
        public string ResponseBody { get; set; } = responseBody;
    }
}
