using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Api.Contracts
{
    public class GameResponse
    {
        public int NumberOfSimulations { get; set; }

        public int NumberOfWin { get; set; }

        public int NumberOfLose { get; set; }

        public GameResponse()
        {
        }

        public GameResponse(int numberOfSimulations)
        {
            NumberOfSimulations = numberOfSimulations;
        }
    }
}
