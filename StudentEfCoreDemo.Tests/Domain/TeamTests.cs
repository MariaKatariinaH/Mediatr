using FluentAssertions;
using StudentEfCoreDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Tests.Domain
{
    public class TeamTests
    {
        [Fact]
        public void CreateTeam_WithValidData_ShouldCreateSuccesfully()
        {
            //Arrange
            var name = "Mikkelin mailamestarit";
            var sportType = "Pesäpallo";
            var foundedDate = new DateTime(2024, 10, 04);
            var homeStadium = "Urheilupuisto";
            var maxRosterSize = 30;

            //Act
            var team = new Team
            {
                Name = name,
                SportType = sportType,
                FoundedDate = foundedDate,
                HomeStadium = homeStadium,
                MaxRosterSize = maxRosterSize
            };

            //Assert
            team.Should().NotBeNull();
            team.Name.Should().Be(name);
            team.SportType.Should().Be(sportType);
            team.FoundedDate.Should().Be(foundedDate);
            team.HomeStadium.Should().Be(homeStadium);
            team.MaxRosterSize.Should().Be(maxRosterSize);
        }
        [Theory]
        [InlineData("", "Pesäpallo", "2024-10-04", "Urheilupuisto", 30)]
        [InlineData("Mailamestarit", "", "2024-10-04", "Urheilupuisto", 30)]
        [InlineData("Mailamestarit", "Pesäpallo", "2024-10-04" , "Urheilupuisto", -1)]
        public void CreateTeam_WithInvalidData_ShouldThrowException(string name, string sportType, DateTime foundedDate, string homeStadium, int maxRosterSize)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var team = new Team
                {
                    Name = name,
                    SportType = sportType,
                    FoundedDate = foundedDate,
                    HomeStadium = homeStadium,
                    MaxRosterSize = maxRosterSize
                };
            });
        }
    }
}