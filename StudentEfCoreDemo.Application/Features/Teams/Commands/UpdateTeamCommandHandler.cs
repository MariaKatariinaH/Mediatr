using MediatR;
using StudentEfCoreDemo.Application.Interfaces;
using StudentEfCoreDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Application.Features.Teams.Commands
{
    public class UpdateTeamCommandHandler : IRequestHandler<UpdateTeamCommand>
    {
        private readonly ITeamsRepository _repository;
        public UpdateTeamCommandHandler(ITeamsRepository repository)
        {
            _repository = repository;
        }
        public async Task Handle(UpdateTeamCommand request, CancellationToken cancellationToken)
        {
            var team = new Team
            {
                Id = request.Id,
                Name = request.Name,
                FoundedDate = request.FoundedDate,
                HomeStadium = request.HomeStadium,
                MaxRosterSize = request.MaxRosterSize,
            };

            await _repository.UpdateAsync(team);
        }
    }
}