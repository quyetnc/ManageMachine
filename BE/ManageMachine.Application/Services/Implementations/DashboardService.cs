using ManageMachine.Application.Common;
using ManageMachine.Application.DTOs.Dashboard;
using ManageMachine.Domain.Entities;
// using Microsoft.EntityFrameworkCore; removed
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services.Implementations
{
    public class DashboardService : IDashboardService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IGenericRepository<MachineType> _typeRepository;

        public DashboardService(IMachineRepository machineRepository, IGenericRepository<MachineType> typeRepository)
        {
            _machineRepository = machineRepository;
            _typeRepository = typeRepository;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            var machines = await _machineRepository.GetAllWithDetailsAsync(); // Or better, just simpler query
            var types = await _typeRepository.GetAllAsync();

            var stats = new DashboardStatsDto
            {
                TotalMachines = machines.Count,
                TotalTypes = types.Count,
                MachinesByType = machines
                    .GroupBy(m => m.MachineType?.Name ?? "Unknown")
                    .Select(g => new MachinesByTypeDto
                    {
                        TypeName = g.Key,
                        Count = g.Count()
                    })
                    .ToList(),
                MachinesByUser = machines
                    .Where(m => m.User != null)
                    .GroupBy(m => m.User.FullName)
                    .Select(g => new MachinesByUserDto
                    {
                         UserOnwerName = g.Key,
                         MachineCount = g.Count()
                    })
                    .OrderByDescending(x => x.MachineCount)
                    .Take(10) // Top 10 users
                    .ToList()
            };

            return stats;
        }
    }
}
