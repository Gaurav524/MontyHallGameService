using GameService.Core.Interfaces;
using GameService.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameService.Application.Services
{
    public class EmulateGameService : IEmulateGameService
    {
        public async Task<bool> PlayGame(bool isReadyToChange)
        {
            var randomNumber = new Random();
            var prize = randomNumber.Next(0, 3);
            var selection = randomNumber.Next(0, 3);

            List<Option> options = new() { new Option(), new Option(), new Option() };
            options[prize].IsCar = true;
            options[selection].IsSelected = true;
            var selectedChoice = options[selection];
            var randomlyDisplayedDoor = randomNumber.Next(0, 2);

            var displayedChoice = await DisplayedChoiceToPlayer(options, randomlyDisplayedDoor);
            options.Remove(displayedChoice);

            if (isReadyToChange)
            {
                var initialChoice = options.FirstOrDefault(x => x.IsSelected);
                selectedChoice = options.FirstOrDefault(x => !x.IsSelected);
                if (selectedChoice != null) selectedChoice.IsSelected = true;
            }

            return selectedChoice != null && selectedChoice.IsCar;
        }

        private async Task<Option> DisplayedChoiceToPlayer(IEnumerable<Option> options, int randomlyDisplayedDoor)
        {
            var choicesToShow = options.Where(x => !x.IsSelected && !x.IsCar).ToList();
            var displayedChoice = choicesToShow.ElementAt(choicesToShow.Count() == 1 ? 0 : randomlyDisplayedDoor);
            return displayedChoice;
        }
    }
}
