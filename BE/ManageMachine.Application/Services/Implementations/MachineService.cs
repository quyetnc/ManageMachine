using AutoMapper;
using ManageMachine.Application.Common;
using ManageMachine.Application.DTOs.Machine;
using ManageMachine.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services.Implementations
{
    public class MachineService : IMachineService
    {
        private readonly IMachineRepository _machineRepository;
        private readonly IGenericRepository<MachineParameter> _machineParameterRepository;
        private readonly IGenericRepository<Parameter> _parameterRepository;
        private readonly IMapper _mapper;

        public MachineService(
            IMachineRepository machineRepository,
            IGenericRepository<MachineParameter> machineParameterRepository,
            IGenericRepository<Parameter> parameterRepository,
            IMapper mapper)
        {
            _machineRepository = machineRepository;
            _machineParameterRepository = machineParameterRepository;
            _parameterRepository = parameterRepository;
            _mapper = mapper;
        }

        public async Task<MachineDto> CreateAsync(CreateMachineDto createDto)
        {
            var machine = _mapper.Map<Machine>(createDto);
            machine.QRCodeData = Guid.NewGuid().ToString(); // Simple QR code generation
            
            // Parameters are already mapped by AutoMapper into 'machine.Parameters'
            // and will be saved automatically by the repository due to cascade persistence.
            
            await _machineRepository.AddAsync(machine);

            // // Add parameters if any
            // if (createDto.Parameters != null && createDto.Parameters.Any())
            // {
            //     foreach (var p in createDto.Parameters)
            //     {
            //         var machineParam = new MachineParameter
            //         {
            //             MachineId = machine.Id,
            //             ParameterId = p.ParameterId,
            //             Value = p.Value
            //         };
            //         await _machineParameterRepository.AddAsync(machineParam);
            //     }
            // }
            return await GetByIdAsync(machine.Id) ?? throw new Exception("Failed to retrieve created machine");
        }

        public async Task DeleteAsync(int id)
        {
            var machine = await _machineRepository.GetByIdAsync(id);
            if (machine != null)
            {
                await _machineRepository.DeleteAsync(machine);
            }
        }

        public async Task<IReadOnlyList<MachineDto>> GetAllAsync()
        {
            var machines = await _machineRepository.GetAllAsync();
            // TODO: Include navigation properties in Repository layer or mapped manually if LazyLoading is off
            // Assuming Repository handles Includes or we use specific Get methods.
            // For now, mapping might be partial if Includes aren't set.
            // I'll rely on Repository implementation to include children or use ProjectTo.
            // For simplicity, I'll return mapped list.
            return _mapper.Map<IReadOnlyList<MachineDto>>(machines);
        }

        public async Task<MachineDto?> GetByIdAsync(int id)
        {
            var machine = await _machineRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<MachineDto>(machine);
        }

        public async Task<MachineDto?> GetByQRCodeAsync(string qrCodeData)
        {
            var machines = await _machineRepository.GetAsync(m => m.QRCodeData == qrCodeData);
            var machine = machines.FirstOrDefault();
            return _mapper.Map<MachineDto>(machine);
        }

        public async Task UpdateAsync(int id, CreateMachineDto updateDto)
        {
            var machine = await _machineRepository.GetByIdAsync(id);
            if (machine == null) throw new Exception("Machine not found");

            _mapper.Map(updateDto, machine);
            await _machineRepository.UpdateAsync(machine);
        }

        public async Task AddParameterToMachineAsync(int machineId, CreateMachineParameterDto paramDto)
        {
            var machineParam = new MachineParameter
            {
                MachineId = machineId,
                ParameterId = paramDto.ParameterId,
                Value = paramDto.Value
            };
            await _machineParameterRepository.AddAsync(machineParam);
        }
    }
}
