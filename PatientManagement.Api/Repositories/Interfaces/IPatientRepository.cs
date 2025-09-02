using PatientManagement.Api.DTOs;
using PatientManagement.Api.Models;

namespace PatientManagement.Api.Repositories.Interfaces
{
    public interface IPatientRepository
    {
        Task<bool> EmailExistsAsync(string email, Guid? excludeId = null);
        Task<bool> PhoneExistsAsync(string phone, Guid? excludeId = null);
        Task<Patient?> GetByIdAsync(Guid id);
        Task AddAsync(Patient patient);
        Task UpdateAsync(Patient patient);
        Task DeleteAsync(Patient patient);
        Task<PagedResult<Patient>> SearchAsync(SearchPatientsRequest request);
        Task SaveChangesAsync();
    }
}