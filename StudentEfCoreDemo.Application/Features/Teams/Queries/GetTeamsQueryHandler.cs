using MediatR;
using StudentEfCoreDemo.Application.DTOs;
using StudentEfCoreDemo.Application.Interfaces;
using StudentEfCoreDemo.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Application.Features.Teams.Queries
{
    public class GetTeamsQueryHandler : IRequestHandler<GetTeamsQuery, List<TeamDto>>
    {
        private readonly ITeamsRepository _repository;
        public GetTeamsQueryHandler(ITeamsRepository repository)
        {
            _repository = repository;
        }
        public async Task<List<TeamDto>> Handle(GetTeamsQuery request, CancellationToken cancellationToken)
        {
            var teams = await _repository.GetAllAsync();
            return teams.Select(s => new TeamDto
            {
                Id = s.Id,
                Name = s.Name,
                SportType = s.SportType,
                FoundedDate = s.FoundedDate,
                HomeStadium = s.HomeStadium,
                MaxRosterSize = s.MaxRosterSize,
            }).ToList();
        }
    }
}