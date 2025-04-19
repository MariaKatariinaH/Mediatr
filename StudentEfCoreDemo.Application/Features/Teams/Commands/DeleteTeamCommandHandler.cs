using MediatR;
using StudentEfCoreDemo.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Application.Features.Teams.Commands
{
    public class DeleteTeamCommandHandler : IRequestHandler<DeleteTeamCommand>
    {
        private readonly ITeamsRepository _repository;
        public DeleteTeamCommandHandler(ITeamsRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(DeleteTeamCommand request, CancellationToken cancellationToken)
        {
            await _repository.DeleteAsync(request.Id);
        }
    }
}