using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentEfCoreDemo.Application.DTOs
{
    public class TeamDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime FoundedDate { get; set; }
        public string HomeStadium { get; set; } = string.Empty;
        public int MaxRosterSize { get; set; }
    }
}