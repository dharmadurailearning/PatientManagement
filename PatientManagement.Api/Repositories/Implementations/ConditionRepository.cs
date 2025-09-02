using Microsoft.EntityFrameworkCore;
using PatientManagement.Api.Data;
using PatientManagement.Api.Models;
using PatientManagement.Api.Repositories.Interfaces;


namespace PatientManagement.Api.Repositories.Implementations
{
    public class ConditionRepository : IConditionRepository
    {
        private readonly PatientDbContext _db;
        public ConditionRepository(PatientDbContext db) => _db = db;
        public Task<List<Condition>> GetByIdsAsync(IEnumerable<int> ids)
        => _db.Conditions.Where(c => ids.Contains(c.Id)).ToListAsync();
    }
}