using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportsExerciseBattle.Utilities
{
    public static class ConstantsSettings
    {
        public const string ServerUrl = "http://localhost:8000/";
        public const int ServerPort = 10001;

        public const int DefaultTournamentTime = 120; // in seconds
        public const int DefaultELO = 100;
        public const string DefaultExerciseName = "PushUps";
        public const int ELOWinPoints = 2;
        public const int ELODrawPoints = 1;
        public const int ELOLosePoints = -1;
    }
}
