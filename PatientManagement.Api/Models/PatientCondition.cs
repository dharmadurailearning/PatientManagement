namespace PatientManagement.Api.Models
{
    public class PatientCondition
    {
        public Guid PatientId { get; set; }
        public int ConditionId { get; set; }
        public DateOnly DiagnosedDate { get; set; }
        public Patient Patient { get; set; } = null!;
        public Condition Condition { get; set; } = null!;
    }
}