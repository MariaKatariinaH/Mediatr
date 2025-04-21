using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using StudentEfCoreDemo.Domain.Entities;
using StudentEfCoreDemo.Infrastructure.Data;
using StudentEfCoreDemo.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Tests.Infrastructure
{
    public class TeamRepositoryTests
    {
        private readonly DbContextOptions<StudentContext> _options;
        private readonly StudentContext _context;
        private readonly TeamRepository _repository;

        public TeamRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<StudentContext>()
                .UseInMemoryDatabase(databaseName: $"TeamDb_{Guid.NewGuid()}")
                .Options;

            _context = new StudentContext(_options);
            _repository = new TeamRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllTeams()
        {
            // Arrange
            var teams = new List<Team>
            {
                new() { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 },
                new() { Id = 2, Name = "Pallopöllöt", SportType = "Jalkapallo", FoundedDate = new DateTime(2022, 10, 01), HomeStadium = "Sulkava", MaxRosterSize = 25 }
            };

            await _context.Teams.AddRangeAsync(teams);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(teams);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTeamExists_ShouldReturnTeam()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(team);
        }

        [Fact]
        public async Task GetByIdAsync_WhenTeamDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddTeamAndSaveChanges()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 };

            // Act
            var result = await _repository.AddAsync(team);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(team);

            var savedTeam = await _context.Teams.FindAsync(result.Id);
            savedTeam.Should().NotBeNull();
            savedTeam.Should().BeEquivalentTo(team);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateTeamAndSaveChanges()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            team.Name = "Pallo-oravat";
            team.SportType = "Jääpallo";
            team.FoundedDate = new DateTime(2020, 08, 06);
            team.HomeStadium = "Mikkeli";
            team.MaxRosterSize = 25;

            // Act
            await _repository.UpdateAsync(team);

            // Assert
            var updatedTeam = await _context.Teams.FindAsync(team.Id);
            updatedTeam.Should().NotBeNull();
            updatedTeam.Should().BeEquivalentTo(team);
        }

        [Fact]
        public async Task DeleteAsync_WhenTeamExists_ShouldDeleteTeamAndSaveChanges()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var deletedTeam = await _context.Teams.FindAsync(1);
            deletedTeam.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WhenTeamDoesNotExist_ShouldNotThrowException()
        {
            // Act & Assert
            await _repository.DeleteAsync(1);
        }

        [Fact]
        public async Task ExistsAsync_WhenTeamExists_ShouldReturnTrue()
        {
            // Arrange
            var team = new Team { Id = 1, Name = "Pallokuninkaat", SportType = "Jalkapallo", FoundedDate = new DateTime(2025, 02, 01), HomeStadium = "Hirvensalmi", MaxRosterSize = 25 };
            await _context.Teams.AddAsync(team);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_WhenTeamDoesNotExist_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(1);

            // Assert
            result.Should().BeFalse();
        }
    }
}