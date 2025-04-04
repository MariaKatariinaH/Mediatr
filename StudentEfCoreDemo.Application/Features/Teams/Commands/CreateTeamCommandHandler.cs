using MediatR;
using StudentEfCoreDemo.Application.DTOs;
using StudentEfCoreDemo.Application.Features.Students.Commands;
using StudentEfCoreDemo.Application.Interfaces;
using StudentEfCoreDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Application.Features.Teams.Commands
{
    public class CreateTeamCommandHandler : IRequestHandler<CreateStudentCommand, StudentDto>
    {
        private readonly ITeamsRepository _repository;

        public CreateTeamCommandHandler(ITeamsRepository repository)
        {
            _repository = repository;
        }

        public async Task<StudentDto> Handle(CreateStudentCommand request, CancellationToken cancellationToken)
        {
            var student = new Student
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Age = request.Age
            };

            var createdStudent = await _repository.AddAsync(student);
            return new StudentDto
            {
                Id = createdStudent.Id,
                FirstName = createdStudent.FirstName,
                LastName = createdStudent.LastName,
                Age = createdStudent.Age
            };
        }
    }
}
}
