using System.ComponentModel.DataAnnotations;
using static PatientManagement.Api.DTOs.PatientResponseDto;

namespace PatientManagement.Api.DTOs
{
    public record PatientCreateDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; init; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; init; } = null!;

        [Required]
        public DateOnly DOB { get; init; }

        [Required]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Invalid gender")]
        public string Gender { get; init; } = null!;

        [Required, MaxLength(100)]
        public string City { get; init; } = null!;

        [Required, EmailAddress]
        public string Email { get; init; } = null!;

        [Required, MaxLength(20)]
        public string Phone { get; init; } = null!;

        public List<int>? ConditionIds { get; init; }

        public DateOnly? DiagnosedDate { get; init; }
    }

    public record PatientUpdateDto
    {
        [Required, MaxLength(100)]
        public string FirstName { get; init; } = null!;

        [Required, MaxLength(100)]
        public string LastName { get; init; } = null!;

        [Required]
        public DateOnly DOB { get; init; }

        [Required]
        [RegularExpression("Male|Female|Other", ErrorMessage = "Invalid gender")]
        public string Gender { get; init; } = null!;

        [Required, MaxLength(100)]
        public string City { get; init; } = null!;

        [Required, MaxLength(20)]
        public string Phone { get; init; } = null!;

        public List<int>? ConditionIds { get; init; }

        public DateOnly? DiagnosedDate { get; init; }
    }

    public record PatientResponseDto(
        Guid Id,
        string FirstName,
        string LastName,
        DateOnly DOB,
        string Gender,
        string City,
        string Email,
        string Phone,
        List<ConditionItem> Conditions)
    {
        public record ConditionItem(int Id, string Name, DateOnly DiagnosedDate);
    }
}