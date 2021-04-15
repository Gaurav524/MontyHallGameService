using GameService.Api.Contracts;
using GameService.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using GameService.Api.Validators;
using GameService.Core.Exceptions;
using GameService.Core.Models;

namespace GameService.Api.Handlers
{
    public class GameRequestHandler : IRequestHandler<GameRequest, GameResponse>
    {
        private readonly IEmulateGameService _emulateGameService;
        private readonly IMapper _mapper;
        private readonly ILogger<GameRequestHandler> _logger;

        public GameRequestHandler(IEmulateGameService emulateGameService, IMapper mapper, ILogger<GameRequestHandler> logger)
        {
            _emulateGameService = emulateGameService ?? throw new ArgumentNullException(nameof(emulateGameService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GameResponse> Handle(GameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var validator = new GameRequestValidator().Validate(request);
                if (!validator.IsValid)
                {
                    _logger.LogError("Validation failed");
                    throw new ValidationException();
                }

                var gameRequestSpecification = _mapper.Map<GameRequestSpecification>(request);

                var isReadyToChange = gameRequestSpecification != null && gameRequestSpecification.ReadyToChangeDoor;
                
                if (gameRequestSpecification != null)
                {
                    var wins = await PlayGames(
                        isReadyToChange,
                        gameRequestSpecification.NumberOfSimulations);
                    return new GameResponse
                    {
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
