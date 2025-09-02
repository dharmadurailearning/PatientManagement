namespace PatientManagement.Api.Models
{
    public class Patient
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateOnly DOB { get; set; }
        public string Gender { get; set; } = null!; // Male|Female|Other
        public string City { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();
    }
}