using PatientManagement.Api.DTOs;
using PatientManagement.Api.Models;
using PatientManagement.Api.Repositories.Implementations;
using PatientManagement.Api.Repositories.Interfaces;
using PatientManagement.Api.Services.Interfaces;

namespace PatientManagement.Api.Services.Implementations
{
    public class PatientService : IPatientService
    {
        private readonly IPatientRepository _patients;
        private readonly IConditionRepository _conditions;
        public PatientService(IPatientRepository patients, IConditionRepository conditions)
        {
            _patients = patients; _conditions = conditions;
        }

        public async Task<Patient> CreateAsync(PatientCreateDto dto)
        {
            if (await _patients.EmailExistsAsync(dto.Email))
                throw new InvalidOperationException("Email already exists");
            if (await _patients.PhoneExistsAsync(dto.Phone))
                throw new InvalidOperationException("Phone already exists");
            if (dto.DOB > DateOnly.FromDateTime(DateTime.UtcNow.Date))
                throw new InvalidOperationException("DOB can't be in the future");

            var patient = new Patient
            {
                Id = Guid.NewGuid(),
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                DOB = dto.DOB,
                Gender = dto.Gender,
                City = dto.City,
                Email = dto.Email,
                Phone = dto.Phone
            };

            if (dto.ConditionIds?.Any() == true)
            {
                var conds = await _conditions.GetByIdsAsync(dto.ConditionIds);
                foreach (var c in conds)
                {
                    patient.PatientConditions.Add(new PatientCondition
                    {
                        PatientId = patient.Id,
                        ConditionId = c.Id,
                        DiagnosedDate = dto.DiagnosedDate ?? DateOnly.FromDateTime(DateTime.UtcNow.Date)
                    });
                }
            }

            await _patients.AddAsync(patient);
            await _patients.SaveChangesAsync();
            return await _patients.GetByIdAsync(patient.Id) ?? patient;
        }

        public async Task<Patient> UpdateAsync(Guid id, PatientUpdateDto dto)
        {
            var existing = await _patients.GetByIdAsync(id) ?? throw new KeyNotFoundException("Patient not found");

            if (await _patients.PhoneExistsAsync(dto.Phone, excludeId: id))
                throw new InvalidOperationException("Phone already exists");
            if (dto.DOB > DateOnly.FromDateTime(DateTime.UtcNow.Date))
                throw new InvalidOperationException("DOB can't be in the future");

            existing.FirstName = dto.FirstName;
            existing.LastName = dto.LastName;
            existing.DOB = dto.DOB;
            existing.Gender = dto.Gender;
            existing.City = dto.City;
            existing.Phone = dto.Phone;

            if (dto.ConditionIds != null)
            {
                existing.PatientConditions.Clear();
                if (dto.ConditionIds.Any())
                {
                    var conds = await _conditions.GetByIdsAsync(dto.ConditionIds);
                    foreach (var c in conds)
                    {
                        existing.PatientConditions.Add(new PatientCondition
                        {
                            PatientId = existing.Id,
                            ConditionId = c.Id,
                            DiagnosedDate = dto.DiagnosedDate ?? DateOnly.FromDateTime(DateTime.UtcNow.Date)
                        });
                    }
                }
            }

            await _patients.UpdateAsync(existing);
            await _patients.SaveChangesAsync();
            return await _patients.GetByIdAsync(existing.Id) ?? existing;
        }

        public async Task DeleteAsync(Guid id)
        {
            var existing = await _patients.GetByIdAsync(id) ?? throw new KeyNotFoundException("Patient not found");
            await _patients.DeleteAsync(existing);
            await _patients.SaveChangesAsync();
        }

        public Task<Patient?> GetAsync(Guid id) => _patients.GetByIdAsync(id);
        public Task<PagedResult<Patient>> SearchAsync(SearchPatientsRequest request) => _patients.SearchAsync(request);
    }
}
