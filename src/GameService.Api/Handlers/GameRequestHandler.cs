using AutoMapper;
using GameService.Api.Contracts;
using GameService.Core.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GameService.Api.Handlers
{
    public class GameRequestHandler : IRequestHandler<GameRequest, GameResponse>
    {
       
        private readonly ILogger<GameRequestHandler> _logger;

        public GameRequestHandler(ILogger<GameRequestHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<GameResponse> Handle(GameRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var isReadyToChange = request != null && request.IsChangeDoor;

                if (request != null)
                {
                    var wins = await RunGames(isReadyToChange, request.NumberOfSimulations);
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

        private async Task<int> RunGames(bool isReadyToChange, int numberOfSimulations)
        {
            var count = 0;

            for (var i = 0; i < numberOfSimulations; i++)
            {
                var isWin = await RunGame(isReadyToChange);
                if (isWin)
                    count++;
            }
            return count;
        }

        private async Task<bool> RunGame(bool isReadyToChange)
        {
            var randomNumber = new Random();
            var prize = randomNumber.Next(0, 3);
            var selection = randomNumber.Next(0, 3);

            List<Option> options = new() { new Option(), new Option(), new Option() };
            options[prize].IsCar = true;
            options[selection].IsSelected = true;
            var selectedChoice = options[selection];
            var randomlyDisplayedDoor = randomNumber.Next(0, 2);

            var displayedChoice = DisplayedChoiceToPlayer(options, randomlyDisplayedDoor);
            options.Remove(displayedChoice);

            if (isReadyToChange)
            {
                var initialChoice = options.FirstOrDefault(x => x.IsSelected);
                selectedChoice = options.FirstOrDefault(x => !x.IsSelected);
                if (selectedChoice != null) selectedChoice.IsSelected = true;
            }

            return selectedChoice != null && selectedChoice.IsCar;
        }

        private static Option DisplayedChoiceToPlayer(IEnumerable<Option> options, int randomlyDisplayedDoor)
        {
            var choicesToShow = options.Where(x => !x.IsSelected && !x.IsCar).ToList();
            var displayedChoice = choicesToShow.ElementAt(choicesToShow.Count() == 1 ? 0 : randomlyDisplayedDoor);
            return displayedChoice;
        }
    }
}
