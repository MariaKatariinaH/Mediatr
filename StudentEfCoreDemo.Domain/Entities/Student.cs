namespace StudentEfCoreDemo.Domain.Entities
{
    public class Student
    {
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private int _age;

        public int Id { get; set; }

        public string FirstName
        {
            get => _firstName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("First name cannot be empty or whitespace.", nameof(FirstName));
                _firstName = value;
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Last name cannot be empty or whitespace.", nameof(LastName));
                _lastName = value;
            }
        }

        public int Age
        {
            get => _age;
            set
            {
                if (value < 0)
                    throw new ArgumentException("Age cannot be negative.", nameof(Age));
                _age = value;
            }
        }
    }
} 