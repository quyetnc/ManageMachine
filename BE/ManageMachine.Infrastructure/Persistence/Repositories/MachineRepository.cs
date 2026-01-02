using ManageMachine.Application.Common;
using ManageMachine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ManageMachine.Infrastructure.Persistence.Repositories
{
    public class MachineRepository : GenericRepository<Machine>, IMachineRepository
    {
        public MachineRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<Machine?> GetByIdWithDetailsAsync(int id)
        {
            return await _context.Machines
                .Include(m => m.MachineType)
                .Include(m => m.Parameters)
                    .ThenInclude(mp => mp.Parameter)
                .FirstOrDefaultAsync(m => m.Id == id);
        }
    }
}
