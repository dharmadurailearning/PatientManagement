using System.ComponentModel.DataAnnotations;

namespace PatientManagement.Api.DTOs
{
    public class SearchPatientsRequest
    {
        [Range(0, 200)]
        public int? MinAge { get; set; }

        [Range(0, 200)]
        public int? MaxAge { get; set; }

        public string? Gender { get; set; }

        public string? City { get; set; }

        public int? ConditionId { get; set; }

        [Range(1, int.MaxValue)]
        public int Page { get; set; } = 1;

        [Range(1, 200)]
        public int PageSize { get; set; } = 20;
    }

    public class PagedResult<T>
    {
        public required IReadOnlyList<T> Items { get; init; }
        public required int Total { get; init; }
        public required int Page { get; init; }
        public required int PageSize { get; init; }
    }
}