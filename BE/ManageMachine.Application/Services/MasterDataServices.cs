using ManageMachine.Application.DTOs.Machine;
using ManageMachine.Application.DTOs.Dashboard; // Add this
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services
{
    public interface IMachineTypeService
    {
        Task<IReadOnlyList<MachineTypeDto>> GetAllAsync();
        Task<MachineTypeDto?> GetByIdAsync(int id);
        Task<MachineTypeDto> CreateAsync(CreateMachineTypeDto createDto);
        Task UpdateAsync(int id, CreateMachineTypeDto updateDto);
        Task DeleteAsync(int id);
    }

    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetStatsAsync();
    }

    public interface IParameterService
    {
        Task<IReadOnlyList<ParameterDto>> GetAllAsync();
        Task<ParameterDto?> GetByIdAsync(int id);
        Task<ParameterDto> CreateAsync(CreateParameterDto createDto);
        Task UpdateAsync(int id, CreateParameterDto updateDto);
        Task DeleteAsync(int id);
    }
}
