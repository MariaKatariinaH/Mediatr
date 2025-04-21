using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using StudentEfCoreDemo.API.Controllers;
using StudentEfCoreDemo.Application.DTOs;
using StudentEfCoreDemo.Application.Features.Students.Commands;
using StudentEfCoreDemo.Application.Features.Teams.Commands;
using StudentEfCoreDemo.Application.Features.Teams.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Tests.API
{
    public class TeamsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TeamsController _controller;

        public TeamsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new TeamsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetTeams_ShouldReturnListOfTeams()
        {
            // Arrange
            var teams = new List<TeamDto>
            {
                new() { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 },
                new() { Id = 2, Name = "Pallopöllöt", SportType = "Jalkapallo", FoundedDate = new DateTime(2022, 10, 01), HomeStadium = "Sulkava", MaxRosterSize = 25 }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTeamsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(teams);

            // Act
            var actionResult = await _controller.GetTeams();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var returnedTeams = result!.Value as List<TeamDto>;
            returnedTeams.Should().NotBeNull();
            returnedTeams.Should().HaveCount(2);
            returnedTeams.Should().BeEquivalentTo(teams);
        }

        [Fact]
        public async Task GetTeam_WhenTeamExists_ShouldReturnTeam()
        {
            // Arrange
            var team = new TeamDto { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTeamByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(team);

            // Act
            var actionResult = await _controller.GetTeam(1);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var returnedTeam = result!.Value as TeamDto;
            returnedTeam.Should().NotBeNull();
            returnedTeam.Should().BeEquivalentTo(team);
        }

        [Fact]
        public async Task GetTeam_WhenTeamDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetTeamByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((TeamDto?)null);

            // Act
            var actionResult = await _controller.GetTeam(1);

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateTeam_ShouldReturnCreatedTeam()
        {
            // Arrange
            var command = new CreateTeamCommand
            {
                Name = "Pallokuninkaat",
                SportType = "Jalkapallo",
                FoundedDate = new DateTime(2025, 02, 01),
                HomeStadium = "Hirvensalmi",
                MaxRosterSize = 25
            };

            var createdTeam = new TeamDto
            {
                Id = 1,
                Name = command.Name,
                SportType = command.SportType,
                FoundedDate = command.FoundedDate,
                HomeStadium = command.HomeStadium,
                MaxRosterSize = command.MaxRosterSize
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateTeamCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdTeam);

            // Act
            var actionResult = await _controller.CreateTeam(command);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            result!.ActionName.Should().Be(nameof(TeamsController.GetTeam));
            result.RouteValues.Should().ContainKey("id").And.ContainValue(createdTeam.Id);
            var returnedTeam = result.Value as TeamDto;
            returnedTeam.Should().NotBeNull();
            returnedTeam.Should().BeEquivalentTo(createdTeam);
        }

        [Fact]
        public async Task UpdateTeam_WhenIdsMatch_ShouldUpdateSuccessfully()
        {
            // Arrange
            var command = new UpdateTeamCommand
            {
                Id = 1,
                Name = "Pallokuninkaat",
                SportType = "Jalkapallo",
                FoundedDate = new DateTime(2025, 02, 01),
                HomeStadium = "Hirvensalmi",
                MaxRosterSize = 25
            };

            // Act
            var result = await _controller.UpdateTeam(1, command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateTeam_WhenIdsDoNotMatch_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new UpdateTeamCommand
            {
                Id = 1,
                Name = "Pallokuninkaat",
                SportType = "Jalkapallo",
                FoundedDate = new DateTime(2025, 02, 01),
                HomeStadium = "Hirvensalmi",
                MaxRosterSize = 25
            };

            // Act
            var result = await _controller.UpdateTeam(2, command);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateTeamCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteTeam_ShouldDeleteSuccessfully()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.DeleteTeam(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteTeamCommand>(c => c.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}