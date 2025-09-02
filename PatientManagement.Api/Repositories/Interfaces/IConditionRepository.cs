using PatientManagement.Api.Models;


namespace PatientManagement.Api.Repositories.Interfaces
{
    public interface IConditionRepository
    {
        Task<List<Condition>> GetByIdsAsync(IEnumerable<int> ids);
    }
}