using Xunit;
using Microsoft.EntityFrameworkCore;
using Moq;
using FluentAssertions;
using StudentEfCoreDemo.Application.Interfaces;
using StudentEfCoreDemo.Domain.Entities;
using StudentEfCoreDemo.Infrastructure.Data;
using StudentEfCoreDemo.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace StudentEfCoreDemo.Tests.Infrastructure
{
    public class StudentRepositoryTests
    {
        private readonly DbContextOptions<StudentContext> _options;
        private readonly StudentContext _context;
        private readonly StudentRepository _repository;

        public StudentRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<StudentContext>()
                .UseInMemoryDatabase(databaseName: $"StudentDb_{Guid.NewGuid()}")
                .Options;

            _context = new StudentContext(_options);
            _repository = new StudentRepository(_context);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnAllStudents()
        {
            // Arrange
            var students = new List<Student>
            {
                new() { Id = 1, FirstName = "John", LastName = "Doe", Age = 20 },
                new() { Id = 2, FirstName = "Jane", LastName = "Smith", Age = 21 }
            };

            await _context.Students.AddRangeAsync(students);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo(students);
        }

        [Fact]
        public async Task GetByIdAsync_WhenStudentExists_ShouldReturnStudent()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe", Age = 20 };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(student);
        }

        [Fact]
        public async Task GetByIdAsync_WhenStudentDoesNotExist_ShouldReturnNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task AddAsync_ShouldAddStudentAndSaveChanges()
        {
            // Arrange
            var student = new Student { FirstName = "John", LastName = "Doe", Age = 20 };

            // Act
            var result = await _repository.AddAsync(student);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEquivalentTo(student);

            var savedStudent = await _context.Students.FindAsync(result.Id);
            savedStudent.Should().NotBeNull();
            savedStudent.Should().BeEquivalentTo(student);
        }

        [Fact]
        public async Task UpdateAsync_ShouldUpdateStudentAndSaveChanges()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe", Age = 20 };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            student.FirstName = "Jane";
            student.LastName = "Smith";
            student.Age = 21;

            // Act
            await _repository.UpdateAsync(student);

            // Assert
            var updatedStudent = await _context.Students.FindAsync(student.Id);
            updatedStudent.Should().NotBeNull();
            updatedStudent.Should().BeEquivalentTo(student);
        }

        [Fact]
        public async Task DeleteAsync_WhenStudentExists_ShouldDeleteStudentAndSaveChanges()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe", Age = 20 };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var deletedStudent = await _context.Students.FindAsync(1);
            deletedStudent.Should().BeNull();
        }

        [Fact]
        public async Task DeleteAsync_WhenStudentDoesNotExist_ShouldNotThrowException()
        {
            // Act & Assert
            await _repository.DeleteAsync(1);
        }

        [Fact]
        public async Task ExistsAsync_WhenStudentExists_ShouldReturnTrue()
        {
            // Arrange
            var student = new Student { Id = 1, FirstName = "John", LastName = "Doe", Age = 20 };
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.ExistsAsync(1);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ExistsAsync_WhenStudentDoesNotExist_ShouldReturnFalse()
        {
            // Act
            var result = await _repository.ExistsAsync(1);

            // Assert
            result.Should().BeFalse();
        }
    }
} 