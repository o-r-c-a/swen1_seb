using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Models.Messages
{
    public class Request
    {
        public string? Method { get; set; }
        public string? Path { get; set; }
        public string? Version { get; set; }
        public Dictionary<string, string> Headers { get; set; } = [];
        public string? Body { get; set; }

    }
}
