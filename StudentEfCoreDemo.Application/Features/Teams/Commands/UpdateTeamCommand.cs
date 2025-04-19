using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Application.Features.Teams.Commands
{
    public record UpdateTeamCommand : IRequest
    {
        public int Id { get; init; }
        public string Name { get; init; } = string.Empty;
        public DateTime FoundedDate { get; init; }
        public string HomeStadium { get; init; } = string.Empty;
        public int MaxRosterSize { get; init; }
    }
}