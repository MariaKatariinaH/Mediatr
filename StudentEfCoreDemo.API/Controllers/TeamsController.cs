using MediatR;
using Microsoft.AspNetCore.Mvc;
using StudentEfCoreDemo.Application.DTOs;
using StudentEfCoreDemo.Application.Features.Teams.Commands;
using StudentEfCoreDemo.Application.Features.Teams.Queries;

namespace StudentEfCoreDemo.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TeamsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TeamsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeamDto>>> GetTeams()
        {
            var query = new GetTeamsQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TeamDto>> GetTeam(int id)
        {
            var query = new GetTeamByIdQuery(id);
            var result = await _mediator.Send(query);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TeamDto>> CreateTeam(CreateTeamCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetTeam), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTeam(int id, UpdateTeamCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var command = new DeleteTeamCommand(id);
            await _mediator.Send(command);
            return NoContent();
        }
    }
}