using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GameService.Api.Contracts;

namespace GameService.Api.Validators
{
    public sealed class GameRequestValidator : AbstractValidator<GameRequest>
    {
        public GameRequestValidator()
        {
            RuleFor(x => x.NumberOfSimulations)
                .NotEqual(0);
        }
    }
}
