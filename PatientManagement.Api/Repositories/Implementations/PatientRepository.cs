using Microsoft.EntityFrameworkCore;
using PatientManagement.Api.Data;
using PatientManagement.Api.DTOs;
using PatientManagement.Api.Models;
using PatientManagement.Api.Repositories.Interfaces;

namespace PatientManagement.Api.Repositories.Implementations
{
    public class PatientRepository : IPatientRepository
    {
        private readonly PatientDbContext _db;
        public PatientRepository(PatientDbContext db) => _db = db;

        public Task<bool> EmailExistsAsync(string email, Guid? excludeId = null)
            => _db.Patients.AnyAsync(p => p.Email == email && (!excludeId.HasValue || p.Id != excludeId));

        public Task<bool> PhoneExistsAsync(string phone, Guid? excludeId = null)
            => _db.Patients.AnyAsync(p => p.Phone == phone && (!excludeId.HasValue || p.Id != excludeId));

        public Task<Patient?> GetByIdAsync(Guid id)
            => _db.Patients
                  .Include(p => p.PatientConditions)
                  .ThenInclude(pc => pc.Condition)
                  .FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Patient patient)
        {
            await _db.Patients.AddAsync(patient);
        }

        public Task UpdateAsync(Patient patient)
        {
            _db.Patients.Update(patient);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(Patient patient)
        {
            _db.Patients.Remove(patient);
            return Task.CompletedTask;
        }

        public async Task<PagedResult<Patient>> SearchAsync(SearchPatientsRequest req)
        {
            var q = _db.Patients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(req.Gender))
                q = q.Where(p => p.Gender == req.Gender);
            if (!string.IsNullOrWhiteSpace(req.City))
                q = q.Where(p => p.City == req.City);
            if (req.ConditionId.HasValue)
                q = q.Where(p => p.PatientConditions.Any(pc => pc.ConditionId == req.ConditionId));
            if (req.MinAge.HasValue)
            {
                var minDob = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-req.MinAge.Value));
                q = q.Where(p => p.DOB <= minDob);
            }
            if (req.MaxAge.HasValue)
            {
                var maxDob = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-req.MaxAge.Value));
                q = q.Where(p => p.DOB >= maxDob);
            }

            var total = await q.CountAsync();

            var items = await q
                .Include(p => p.PatientConditions)
                .ThenInclude(pc => pc.Condition)
                .OrderBy(p => p.LastName).ThenBy(p => p.FirstName)
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            return new PagedResult<Patient>
            {
                Items = items,
                Total = total,
                Page = req.Page,
                PageSize = req.PageSize
            };
        }

        public Task SaveChangesAsync() => _db.SaveChangesAsync();
    }
}