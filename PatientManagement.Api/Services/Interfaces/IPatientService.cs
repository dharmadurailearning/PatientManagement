using PatientManagement.Api.DTOs;
using PatientManagement.Api.Models;

namespace PatientManagement.Api.Services.Interfaces
{
    public interface IPatientService
    {
        Task<Patient> CreateAsync(PatientCreateDto dto);
        Task<Patient> UpdateAsync(Guid id, PatientUpdateDto dto);
        Task DeleteAsync(Guid id);
        Task<Patient?> GetAsync(Guid id);
        Task<PagedResult<Patient>> SearchAsync(SearchPatientsRequest request);
    }
}