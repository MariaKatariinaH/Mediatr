using MediatR;
using StudentEfCoreDemo.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Application.Features.Teams.Queries
{
    public record GetTeamsQuery : IRequest<List<TeamDto>>;
}