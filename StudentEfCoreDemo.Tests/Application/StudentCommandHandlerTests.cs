using Xunit;
using Moq;
using FluentAssertions;
using StudentEfCoreDemo.Application.Features.Students.Commands;
using StudentEfCoreDemo.Application.Interfaces;
using StudentEfCoreDemo.Domain.Entities;
using StudentEfCoreDemo.Application.DTOs;

namespace StudentEfCoreDemo.Tests.Application
{
    public class StudentCommandHandlerTests
    {
        private readonly Mock<IStudentRepository> _repositoryMock;
        private readonly CreateStudentCommandHandler _handler;

        public StudentCommandHandlerTests()
        {
            _repositoryMock = new Mock<IStudentRepository>();
            _handler = new CreateStudentCommandHandler(_repositoryMock.Object);
        }

        [Fact]
        public async Task Handle_CreateStudentCommand_ShouldCreateStudentSuccessfully()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 20
            };

            var expectedStudent = new Student
            {
                Id = 1,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Age = command.Age
            };

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Student>()))
                .ReturnsAsync(expectedStudent);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(expectedStudent.Id);
            result.FirstName.Should().Be(expectedStudent.FirstName);
            result.LastName.Should().Be(expectedStudent.LastName);
            result.Age.Should().Be(expectedStudent.Age);

            _repositoryMock.Verify(
                repo => repo.AddAsync(It.Is<Student>(s =>
                    s.FirstName == command.FirstName &&
                    s.LastName == command.LastName &&
                    s.Age == command.Age)),
                Times.Once);
        }

        [Fact]
        public async Task Handle_CreateStudentCommand_WhenRepositoryThrowsException_ShouldPropagateException()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 20
            };

            _repositoryMock
                .Setup(repo => repo.AddAsync(It.IsAny<Student>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _handler.Handle(command, CancellationToken.None));
        }
    }
} 