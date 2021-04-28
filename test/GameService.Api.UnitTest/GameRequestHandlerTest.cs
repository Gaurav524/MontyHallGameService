using AutoMapper;
using FluentAssertions;
using GameService.Api.Contracts;
using GameService.Api.Handlers;
using GameService.Core.Interfaces;
using GameService.Core.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace GameService.Api.UnitTest
{
    public class GameRequestHandlerTest
    {
        private GameRequestHandler _sut;
        private readonly Mock<IMapper> mapperMock;
        private readonly Mock<IEmulateGameService> emulateGameServiceMock;
        private readonly Mock<ILogger<GameRequestHandler>> loggerMock;

        public GameRequestHandlerTest()
        {
            emulateGameServiceMock = new Mock<IEmulateGameService>();
            mapperMock = new Mock<IMapper>();
            loggerMock = new Mock<ILogger<GameRequestHandler>>();

            _sut = new GameRequestHandler(
                emulateGameServiceMock.Object,
                mapperMock.Object,
                loggerMock.Object);
        }

        [Fact]
        public async Task Handle_GameRequestWithZeroSimulations_WillReturnNull()
        {
            var gameRequest = new GameRequest
            {
                NumberOfSimulations = 0,
                ReadyToChangeDoor = true
            };

            var response = await _sut.Handle(gameRequest, default);

            response
                .Should()
                .BeNull();
        }

        [Fact]
        public async Task Handle_GameRequestWithZeroSimulations_WillNotReturnNull()
        {
            var gameRequestSpecification = new GameRequestSpecification()
            {
                NumberOfSimulations = 100,
                ReadyToChangeDoor = true
            };

            mapperMock.Setup(x =>
                    x.Map<GameRequestSpecification>(It.IsAny<GameRequest>()))
                .Returns(gameRequestSpecification);


            var gameRequest = new GameRequest
            {
                NumberOfSimulations = 100,
                ReadyToChangeDoor = true
            };

            var response = await _sut.Handle(gameRequest, default);

            response
                .Should()
                .NotBeNull();
        }
    }
}
