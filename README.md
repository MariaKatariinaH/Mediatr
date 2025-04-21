# StudentEfCoreDemo - Clean Architecture Implementation Guide

This guide explains how to implement new features in the StudentEfCoreDemo system using Clean Architecture principles and CQRS pattern with MediatR.

## Project Structure

The solution is organized into four projects following Clean Architecture principles:

- **StudentEfCoreDemo.Domain**: Contains business entities and domain logic
- **StudentEfCoreDemo.Application**: Contains business rules, interfaces, and CQRS commands/queries
- **StudentEfCoreDemo.Infrastructure**: Contains data access and external service implementations
- **StudentEfCoreDemo.API**: Contains API controllers and configuration

## Step-by-Step Guide for Implementing New Features

### 1. Domain Layer (StudentEfCoreDemo.Domain)

Start by defining your domain entities in the Domain layer:

1. Create a new entity class in `Domain/Entities/`

   ```csharp
   namespace StudentEfCoreDemo.Domain.Entities
   {
   	public class Team
   	{
   		private string _name = string.Empty;
   		private string _sportType = string.Empty;
   		private DateTime _foundedDate;
   		private string _homeStadium = string.Empty;
   		private int _maxRosterSize;

   		public int Id { get; set; }

   		public string Name
   		{
   			get => _name;
   			set
   			{
   				if (string.IsNullOrWhiteSpace(value))
   					throw new ArgumentException("Name cannot be empty or whitespace.", nameof(Name));
   				_name = value;
   			}
   		}
   		public string SportType
   		{
   			get => _sportType;
   			set
   			{
   				if (string.IsNullOrWhiteSpace(value))
   					throw new ArgumentException("Sport type cannot be empty or whitespace.", nameof(SportType));
   				_sportType = value;
   			}
   		}
   		public DateTime FoundedDate
   		{ get => _foundedDate;
   			set
   			{
   				if (value > DateTime.Now)
   					throw new ArgumentException("Founding date cannot be in the future,", nameof(FoundedDate));
   				_foundedDate = value;
   			}
   		}
   		public string HomeStadium
   		{
   			get => _homeStadium;
   			set
   			{
   				if (string.IsNullOrWhiteSpace(value))
   					throw new ArgumentException("Home stadium cannot be empty or whitespace.", nameof(HomeStadium));
   				_homeStadium = value;
   			}
   		}
   		public int MaxRosterSize
   		{
   			get => _maxRosterSize;
   			set
   			{
   				if (value < 0)
   					throw new ArgumentException("MaxRosterSize cannot be negative.", nameof(MaxRosterSize));
   				_maxRosterSize = value;
   			}
   		}
   	}
   }

   ```

2. Add any domain-specific validation or business rules
3. Keep the domain layer pure and free of dependencies on other layers

### 2. Application Layer (StudentEfCoreDemo.Application)

The Application layer is where most of the implementation work happens. Follow these steps:

1.  **Create DTOs**

    - Create a new DTO class in `Application/DTOs/`

    ```csharp
     namespace StudentEfCoreDemo.Application.DTOs
     {
       public class TeamDto
       {
         public int Id { get; set; }
         public string Name { get; set; } = string.Empty;
         public string SportType { get; set; } = string.Empty;
         public DateTime FoundedDate { get; set; }
         public string HomeStadium { get; set; } = string.Empty;
         public int MaxRosterSize { get; set; }
       }
     }

    ```

2.  **Define Repository Interface**

    - Create a new interface in `Application/Interfaces/`

    ```csharp
     namespace StudentEfCoreDemo.Application.Interfaces
     {
       public interface ITeamsRepository
       {
         Task<IEnumerable<Team>> GetAllAsync();
         Task<Team?> GetByIdAsync(int id);
         Task<Team> AddAsync(Team team);
         Task UpdateAsync(Team team);
         Task DeleteAsync(int id);
         Task<bool> ExistsAsync(int id);
       }
     }

    ```

3.  **Create Commands and Queries**

    - Create command classes in `Application/Features/YourFeature/Commands/`

    ```csharp
      namespace StudentEfCoreDemo.Application.Features.Teams.Commands
      {
        public record CreateTeamCommand : IRequest<TeamDto>
        {
          public string Name { get; init; } = string.Empty;
          public string SportType { get; init; } = string.Empty;
          public DateTime FoundedDate { get; init; }
          public string HomeStadium { get; init; } = string.Empty;
          public int MaxRosterSize { get; init; }
        }
      }

      namespace StudentEfCoreDemo.Application.Features.Teams.Commands
      {
        public record DeleteTeamCommand(int Id) : IRequest;
      }

      namespace StudentEfCoreDemo.Application.Features.Teams.Commands
      {
        public record UpdateTeamCommand : IRequest
        {
          public int Id { get; init; }
          public string Name { get; init; } = string.Empty;
          public string SportType { get; init; } = string.Empty;
          public DateTime FoundedDate { get; init; }
          public string HomeStadium { get; init; } = string.Empty;
          public int MaxRosterSize { get; init; }
        }
      }
    ```
    - Create query classes in `Application/Features/YourFeature/Queries/

    ```csharp
      namespace StudentEfCoreDemo.Application.Features.Teams.Queries
      {
        public record GetTeamByIdQuery(int Id) : IRequest<TeamDto?>;
      }

      namespace StudentEfCoreDemo.Application.Features.Teams.Queries
      {
        public record GetTeamsQuery : IRequest<List<TeamDto>>;
      }

    ```

4.  **Implement Command/Query Handlers**

    - Create handlers in the same folders as their commands/queries

    ```csharp
    namespace StudentEfCoreDemo.Application.Features.Teams.Commands
    {
      public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, TeamDto>
      {
        private readonly ITeamsRepository _repository;

        public CreateTeamCommandHandler(ITeamsRepository repository)
        {
         	_repository = repository;
        }

        public async Task<TeamDto> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
        {
         	var team = new Team
         	{
         	  Name = request.Name,
         		SportType = request.SportType,
         		FoundedDate = request.FoundedDate,
         		HomeStadium = request.HomeStadium,
         		MaxRosterSize = request.MaxRosterSize
         	};

         	var createdTeam = await _repository.AddAsync(team);
         	return new TeamDto
         	{
         		Id = createdTeam.Id,
         		Name = createdTeam.Name,
         		SportType = createdTeam.SportType,
         		FoundedDate = createdTeam.FoundedDate,
         		HomeStadium = createdTeam.HomeStadium,
         		MaxRosterSize = createdTeam.MaxRosterSize
         	};
        }
      }
    }

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
            SportType = request.SportType,
            FoundedDate = request.FoundedDate,
            HomeStadium = request.HomeStadium,
            MaxRosterSize = request.MaxRosterSize,
          };
       	  await _repository.UpdateAsync(team);
       	}
      }
    }

   namespace StudentEfCoreDemo.Application.Features.Teams.Queries
   {
    public class GetTeamByIdQueryHandler : IRequestHandler<GetTeamByIdQuery, TeamDto?>
    {
      private readonly ITeamsRepository _repository;
      public GetTeamByIdQueryHandler(ITeamsRepository repository)
      {
        _repository = repository;
      }
      public async Task<TeamDto?> Handle(GetTeamByIdQuery request, CancellationToken cancellationToken)
      {
        var team = await _repository.GetByIdAsync(request.Id);
        if (team == null)
        {
          return null;
        }

      	return new TeamDto
     		{
     			Id = team.Id,
     			Name = team.Name,
     			SportType = team.SportType,
     			FoundedDate = team.FoundedDate,
     			HomeStadium = team.HomeStadium,
     			MaxRosterSize = team.MaxRosterSize,
     		};
     	}
    }
   }

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
    ```

### 3. Infrastructure Layer (StudentEfCoreDemo.Infrastructure)

Implement the data access and external service integrations:

1.  **Update DbContext**

    - Add your entity to `Infrastructure/Data/StudentContext.cs`

    ```csharp
     namespace StudentEfCoreDemo.Infrastructure.Data
     {
       public class StudentContext : DbContext
       {
        public StudentContext(DbContextOptions<StudentContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Team> Teams { get; set; } = null!;
       }
     }

    ```

2.  **Implement Repository**

    - Create repository implementation in `Infrastructure/Repositories/`

      ```csharp
      namespace StudentEfCoreDemo.Infrastructure.Repositories
      {
       public class TeamRepository : ITeamsRepository
       {
         private readonly StudentContext _context;

         public TeamRepository(StudentContext context)
         {
         	_context = context;
         }

         public async Task<IEnumerable<Team>> GetAllAsync()
         {
         	return await _context.Teams.ToListAsync();
         }

         public async Task<Team?> GetByIdAsync(int id)
         {
         	return await _context.Teams.FindAsync(id);
         }

         public async Task<Team> AddAsync(Team team)
         {
         	_context.Teams.Add(team);
         	await _context.SaveChangesAsync();
         	return team;
         }

         public async Task UpdateAsync(Team team)
         {
         	_context.Entry(team).State = EntityState.Modified;
         	await _context.SaveChangesAsync();
         }

         public async Task DeleteAsync(int id)
         {
         	var team = await _context.Teams.FindAsync(id);
         	if (team != null)
         	{
         		_context.Teams.Remove(team);
         		await _context.SaveChangesAsync();
         	}
         }

         public async Task<bool> ExistsAsync(int id)
         {
         	return await _context.Teams.AnyAsync(e => e.Id == id);
         }
       }
      }
      ```

### 4. API Layer (StudentEfCoreDemo.API)

Create the API endpoints:

1.  **Create Controller**

    - Create a new controller in `API/Controllers/`

    ```csharp
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

    ```

2.  **Register Dependencies**

  - Add repository registration in `Program.cs`

  ```csharp
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // Add MediatR
    builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(StudentEfCoreDemo.Application.AssemblyReference).Assembly));

    // Add DbContext
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<StudentContext>(options => options.UseSqlServer(connectionString));

    // Add Repositories
    builder.Services.AddScoped<IStudentRepository, StudentRepository>();
    builder.Services.AddScoped<ITeamsRepository, TeamRepository>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
      app.UseSwagger();
      app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
  ```

### 5. Database Migration

After implementing the feature, create and apply the database migration:

1. Open Package Manager Console
2. Set Default Project to `StudentEfCoreDemo.Infrastructure`
3. Run the following commands:

   ```powershell
   Add-Migration AddYourEntity
   Update-Database
   ```

  ```csharp
  namespace StudentEfCoreDemo.Infrastructure.Migrations
  {
   	/// <inheritdoc />
   	public partial class AddTeamEntity : Migration
   	{
   		/// <inheritdoc />
   		protected override void Up(MigrationBuilder migrationBuilder)
   		{
   			migrationBuilder.CreateTable(
   				name: "Teams",
   				columns: table => new
   				{
   					Id = table.Column<int>(type: "int", nullable: false)
   						.Annotation("SqlServer:Identity", "1, 1"),
   					Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
   					SportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
   					FoundedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
   					HomeStadium = table.Column<string>(type: "nvarchar(max)", nullable: false),
   					MaxRosterSize = table.Column<int>(type: "int", nullable: false)
   				},
   				constraints: table =>
   				{
   					table.PrimaryKey("PK_Teams", x => x.Id);
   				});
   		}

   		/// <inheritdoc />
   		protected override void Down(MigrationBuilder migrationBuilder)
   		{
   			migrationBuilder.DropTable(
   				name: "Teams");
   		}
   	}
  }
  ```

## Best Practices

1. **Dependency Direction**

   - Domain layer should have no dependencies
   - Application layer depends only on Domain
   - Infrastructure depends on Application
   - API depends on Application and Infrastructure

2. **CQRS Pattern**

   - Use commands for write operations (Create, Update, Delete)
   - Use queries for read operations (Get, List)
   - Keep commands and queries focused and single-purpose

3. **DTOs**

   - Use DTOs to decouple API models from domain models
   - Keep DTOs simple and focused on data transfer
   - Avoid business logic in DTOs

4. **Repository Pattern**

   - Keep repository interfaces in Application layer
   - Implement repositories in Infrastructure layer
   - Use async/await for all database operations

5. **Error Handling**
   - Implement proper error handling in handlers
   - Return appropriate HTTP status codes from controllers
   - Consider adding validation using FluentValidation

## Testing

1. **Unit Tests**

   - Test command/query handlers
   - Test domain logic
   - Mock dependencies using Moq

2. **Integration Tests**
   - Test repository implementations
   - Test API endpoints
   - Use test database for integration tests

## Example Implementation

For a complete example, look at the Student feature implementation in the codebase, which follows all these principles and patterns.
