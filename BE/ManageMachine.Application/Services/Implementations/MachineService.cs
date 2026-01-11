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
            var machines = await _machineRepository.GetAllWithDetailsAsync();
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

        public async Task<IReadOnlyList<MachineDto>> GetByUserIdAsync(int userId)
        {
            // Assuming Repository has GetAsync supporting expression, but need Includes (MachineType, etc.)
            // GetAsync usually returns IReadOnlyList directly but might not Include.
            // Best to use GetAllWithDetailsAsync and filter in memory if volume low, OR add specialized method to Repo.
            // For now, let's try _machineRepository.GetAsync(m => m.UserId == userId) provided it maps correctly.
            // Actually, if we want MachineType name, we need Includes.
            // Let's assume GetAllWithDetailsAsync returns Queryable or List. It returns List.
            // Optimization: Add GetByUserIdWithDetailsAsync to Repo later. For now:
            var all = await _machineRepository.GetAllWithDetailsAsync();
            var mine = all.Where(m => m.UserId == userId).ToList();
            return _mapper.Map<IReadOnlyList<MachineDto>>(mine);
        }

        public async Task UpdateAsync(int id, CreateMachineDto updateDto)
        {
            // 1. Load machine with existing parameters
            var machine = await _machineRepository.GetByIdWithDetailsAsync(id);
            if (machine == null) throw new Exception($"Machine with id {id} not found");

            // 2. Update scalar properties (ignore Parameters in AutoMapper if possible, or mapping overrides it)
            // To be safe, we map scalars, but we need to ensure AutoMapper doesn't replace the Parameters collection with new objects immediately causing issues.
            // A safer way is to map properties manually or configure Ignore for Parameters in the map.
            // For now, let's map but assume we need to fix the collection. 
            // Actually, if we map CreateMachineDto -> Machine, AutoMapper MIGHT replace the collection reference or clear+add.
            // Let's do scalar update explicitly to avoid messing up the collection tracking.
            
            machine.Name = updateDto.Name;
            machine.Description = updateDto.Description;
            machine.ImageUrl = updateDto.ImageUrl;
            machine.MachineTypeId = updateDto.MachineTypeId;
            machine.UserId = updateDto.UserId;

            // 3. Sync Parameters
            if (updateDto.Parameters != null)
            {
                // Update existing or Add new
                foreach (var paramDto in updateDto.Parameters)
                {
                    var existingParam = machine.Parameters.FirstOrDefault(p => p.ParameterId == paramDto.ParameterId);
                    if (existingParam != null)
                    {
                        // Update
                        existingParam.Value = paramDto.Value;
                    }
                    else
                    {
                        // Add New
                        machine.Parameters.Add(new MachineParameter
                        {
                            MachineId = machine.Id,
                            ParameterId = paramDto.ParameterId,
                            Value = paramDto.Value
                        });
                    }
                }

                // Optional: Remove parameters not in DTO? 
                // If the UI sends ALL active parameters, then missing ones implies deletion.
                // Let's assume yes for a full update.
                var dtoParamIds = updateDto.Parameters.Select(p => p.ParameterId).ToList();
                var paramsToRemove = machine.Parameters.Where(p => !dtoParamIds.Contains(p.ParameterId)).ToList();
                
                foreach (var p in paramsToRemove)
                {
                    machine.Parameters.Remove(p);
                }
            }

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
