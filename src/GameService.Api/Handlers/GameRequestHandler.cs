using GameService.Api.Contracts;
using GameService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GameService.Api.Handlers
{
    public class GameRequestHandler : IRequestHandler<GameRequest, GameResponse>
    {
        private readonly IEmulateGameService _emulateGameService;
        private readonly ILogger<GameRequestHandler> _logger;

        public GameRequestHandler(IEmulateGameService emulateGameService, ILogger<GameRequestHandler> logger)
        {
            _emulateGameService = emulateGameService ?? throw new ArgumentNullException(nameof(emulateGameService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GameResponse> Handle(GameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var isReadyToChange = request != null && request.ReadyToChangeDoor;

                if (request != null)
                {
                    var wins = await PlayGames(isReadyToChange, request.NumberOfSimulations);
                    return new GameResponse { 
                        NumberOfSimulations = request.NumberOfSimulations, 
                        NumberOfWin = wins, 
                        NumberOfLose = request.NumberOfSimulations - wins
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
            return null;
        }

        private async Task<int> PlayGames(bool isReadyToChange, int numberOfSimulations)
        {
            var count = 0;

            for (var i = 0; i < numberOfSimulations; i++)
            {
                var isWin = await _emulateGameService.PlayGame(isReadyToChange);
                if (isWin)
                    count++;
            }
            return count;
        }
    }
}
