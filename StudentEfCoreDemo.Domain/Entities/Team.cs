using System;

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