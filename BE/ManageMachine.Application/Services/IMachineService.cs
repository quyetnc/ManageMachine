using ManageMachine.Application.DTOs.Machine;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services
{
    public interface IMachineService
    {
        Task<IReadOnlyList<MachineDto>> GetAllAsync();
        Task<MachineDto?> GetByIdAsync(int id);
        Task<MachineDto> CreateAsync(CreateMachineDto createDto);
        Task UpdateAsync(int id, CreateMachineDto updateDto); // Reusing CreateDto or update DTO, usually create fits update
        Task DeleteAsync(int id);
        Task<MachineDto?> GetByQRCodeAsync(string qrCodeData);
        Task<IReadOnlyList<MachineDto>> GetByUserIdAsync(int userId);

        // Parameter logic can be here or separate if complex
        Task AddParameterToMachineAsync(int machineId, CreateMachineParameterDto paramDto);
    }
}
