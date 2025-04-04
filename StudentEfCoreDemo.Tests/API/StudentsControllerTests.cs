using Xunit;
using Moq;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using StudentEfCoreDemo.API.Controllers;
using StudentEfCoreDemo.Application.DTOs;
using StudentEfCoreDemo.Application.Features.Students.Commands;
using StudentEfCoreDemo.Application.Features.Students.Queries;

namespace StudentEfCoreDemo.Tests.API
{
    public class StudentsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly StudentsController _controller;

        public StudentsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new StudentsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetStudents_ShouldReturnListOfStudents()
        {
            // Arrange
            var students = new List<StudentDto>
            {
                new() { Id = 1, FirstName = "John", LastName = "Doe", Age = 20 },
                new() { Id = 2, FirstName = "Jane", LastName = "Smith", Age = 21 }
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStudentsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(students);

            // Act
            var actionResult = await _controller.GetStudents();

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var returnedStudents = result!.Value as List<StudentDto>;
            returnedStudents.Should().NotBeNull();
            returnedStudents.Should().HaveCount(2);
            returnedStudents.Should().BeEquivalentTo(students);
        }

        [Fact]
        public async Task GetStudent_WhenStudentExists_ShouldReturnStudent()
        {
            // Arrange
            var student = new StudentDto { Id = 1, FirstName = "John", LastName = "Doe", Age = 20 };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(student);

            // Act
            var actionResult = await _controller.GetStudent(1);

            // Assert
            var result = actionResult.Result as OkObjectResult;
            result.Should().NotBeNull();
            var returnedStudent = result!.Value as StudentDto;
            returnedStudent.Should().NotBeNull();
            returnedStudent.Should().BeEquivalentTo(student);
        }

        [Fact]
        public async Task GetStudent_WhenStudentDoesNotExist_ShouldReturnNotFound()
        {
            // Arrange
            _mediatorMock
                .Setup(m => m.Send(It.IsAny<GetStudentByIdQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((StudentDto?)null);

            // Act
            var actionResult = await _controller.GetStudent(1);

            // Assert
            actionResult.Result.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task CreateStudent_ShouldReturnCreatedStudent()
        {
            // Arrange
            var command = new CreateStudentCommand
            {
                FirstName = "John",
                LastName = "Doe",
                Age = 20
            };

            var createdStudent = new StudentDto
            {
                Id = 1,
                FirstName = command.FirstName,
                LastName = command.LastName,
                Age = command.Age
            };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<CreateStudentCommand>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(createdStudent);

            // Act
            var actionResult = await _controller.CreateStudent(command);

            // Assert
            var result = actionResult.Result as CreatedAtActionResult;
            result.Should().NotBeNull();
            result!.ActionName.Should().Be(nameof(StudentsController.GetStudent));
            result.RouteValues.Should().ContainKey("id").And.ContainValue(createdStudent.Id);
            var returnedStudent = result.Value as StudentDto;
            returnedStudent.Should().NotBeNull();
            returnedStudent.Should().BeEquivalentTo(createdStudent);
        }

        [Fact]
        public async Task UpdateStudent_WhenIdsMatch_ShouldUpdateSuccessfully()
        {
            // Arrange
            var command = new UpdateStudentCommand
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Age = 20
            };

            // Act
            var result = await _controller.UpdateStudent(1, command);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock.Verify(m => m.Send(command, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task UpdateStudent_WhenIdsDoNotMatch_ShouldReturnBadRequest()
        {
            // Arrange
            var command = new UpdateStudentCommand
            {
                Id = 1,
                FirstName = "John",
                LastName = "Doe",
                Age = 20
            };

            // Act
            var result = await _controller.UpdateStudent(2, command);

            // Assert
            result.Should().BeOfType<BadRequestResult>();
            _mediatorMock.Verify(m => m.Send(It.IsAny<UpdateStudentCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteStudent_ShouldDeleteSuccessfully()
        {
            // Arrange
            var id = 1;

            // Act
            var result = await _controller.DeleteStudent(id);

            // Assert
            result.Should().BeOfType<NoContentResult>();
            _mediatorMock.Verify(m => m.Send(It.Is<DeleteStudentCommand>(c => c.Id == id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
} 