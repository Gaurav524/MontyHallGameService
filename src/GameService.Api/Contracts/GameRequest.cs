﻿using MediatR;

namespace GameService.Api.Contracts
{
    public class GameRequest : IRequest<GameResponse>
    {
        public int NumberOfSimulations { get; set; }
        public bool IsChangeDoor { get; set; }
    }
}
