using ManageMachine.Domain.Entities;
using System.Threading.Tasks;

namespace ManageMachine.Application.Common
{
    public interface IMachineRepository : IGenericRepository<Machine>
    {
        Task<Machine?> GetByIdWithDetailsAsync(int id);
        Task<IReadOnlyList<Machine>> GetAllWithDetailsAsync();
    }
}
