using FluentAssertions;
using Moq;
using StudentEfCoreDemo.Application.Features.Teams.Commands;
using StudentEfCoreDemo.Application.Interfaces;
using StudentEfCoreDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Tests.Application
{
    public class TeamCommandHandlerTests
    {
        private readonly Mock<ITeamsRepository> _repositoryMock;
        private readonly CreateTeamCommandHandler _handler;

        public TeamCommandHandlerTests()
        {
            _repositoryMock = new Mock<ITeamsRepository>();
            _handler = new CreateTeamCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CreateTeamCommand_ShouldCreateTeamSuccessfully()
        {
            // Arrange
            var command = new CreateTeamCommand
            {
                Name = "Mailamestarit",
                SportType = "Pesäpallo",
                FoundedDate = new DateTime(2024, 10, 04),
                HomeStadium = "Urheilupuisto",
                MaxRosterSize = 30
            };

            var expectedTeam = new Team
            {
                Id = 1,
                Name = command.Name,
                SportType = command.SportType,
                FoundedDate = command.FoundedDate,
                HomeStadium = command.HomeStadium,
                MaxRosterSize = command.MaxRosterSize
            };

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Team>()))
                .ReturnsAsync(expectedTeam);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedTeam.Id);
            result.Name.Should().Be(expectedTeam.Name);
            result.SportType.Should().Be(expectedTeam.SportType);
            result.FoundedDate.Should().Be(expectedTeam.FoundedDate);
            result.HomeStadium.Should().Be(expectedTeam.HomeStadium);
            result.MaxRosterSize.Should().Be(expectedTeam.MaxRosterSize);

            _repositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Team>(s =>
                    s.Name == command.Name &&
                    s.SportType == command.SportType &&
                    s.FoundedDate == command.FoundedDate &&
                    s.HomeStadium == command.HomeStadium &&
                    s.MaxRosterSize == command.MaxRosterSize)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_CreateTeamCommand_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var command = new CreateTeamCommand
            {
                Name = "Mailamestarit",
                SportType = "Pesäpallo",
                FoundedDate = new DateTime(2024, 10, 04),
                HomeStadium = "Urheilupuisto",
                MaxRosterSize = 30
            };

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Team>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }

    }
}