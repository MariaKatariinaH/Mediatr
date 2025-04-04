using Xunit;
using FluentAssertions;
using StudentEfCoreDemo.Domain.Entities;

namespace StudentEfCoreDemo.Tests.Domain
{
    public class StudentTests
    {
        [Fact]
        public void CreateStudent_WithValidData_ShouldCreateSuccessfully()
        {
            // Arrange
            var firstName = "John";
            var lastName = "Doe";
            var age = 20;

            // Act
            var student = new Student
            {
                FirstName = firstName,
                LastName = lastName,
                Age = age
            };

            // Assert
            student.Should().NotBeNull();
            student.FirstName.Should().Be(firstName);
            student.LastName.Should().Be(lastName);
            student.Age.Should().Be(age);
        }

        [Theory]
        [InlineData("", "Doe", 20)]
        [InlineData("John", "", 20)]
        [InlineData("John", "Doe", -1)]
        public void CreateStudent_WithInvalidData_ShouldThrowException(string firstName, string lastName, int age)
        {
            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() =>
            {
                var student = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Age = age
                };
            });
        }
    }
} 