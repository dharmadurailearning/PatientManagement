namespace PatientManagement.Api.Models
{
    public class Condition
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public ICollection<PatientCondition> PatientConditions { get; set; } = new List<PatientCondition>();
    }
}