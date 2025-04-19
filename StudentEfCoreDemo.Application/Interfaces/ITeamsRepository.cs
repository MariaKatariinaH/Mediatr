using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentEfCoreDemo.Domain.Entities;

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
