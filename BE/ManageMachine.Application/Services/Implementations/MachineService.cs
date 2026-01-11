using AutoMapper;
using ManageMachine.Application.Common;
using ManageMachine.Application.DTOs.Machine;
using ManageMachine.Application.DTOs.Requests;
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
        private readonly IGenericRepository<MachineTransferRequest> _requestRepository;
        private readonly IMapper _mapper;

        public MachineService(
            IMachineRepository machineRepository,
            IGenericRepository<MachineParameter> machineParameterRepository,
            IGenericRepository<Parameter> parameterRepository,
            IGenericRepository<MachineTransferRequest> requestRepository,
            IMapper mapper)
        {
            _machineRepository = machineRepository;
            _machineParameterRepository = machineParameterRepository;
            _parameterRepository = parameterRepository;
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        public async Task<MachineDto> CreateAsync(CreateMachineDto createDto)
        {
            var machine = _mapper.Map<Machine>(createDto);
            
            // Generate incremental serial number
            string nextCode = await GenerateNextSerialNumberAsync();
            machine.SerialNumber = nextCode;

            // Use SerialNumber for QR Data as well (or keep GUID if distinct uniqueness is needed globally, but Serial is unique per tenant usually)
            // User wanted to replace GUID, so let's use the Code.
            // machine.QRCodeData = nextCode; // Removed as per refactor
            
            await _machineRepository.AddAsync(machine);

            return await GetByIdAsync(machine.Id) ?? throw new Exception("Failed to retrieve created machine");
        }

        private async Task<string> GenerateNextSerialNumberAsync()
        {
            // Get the machine with the "largest" SerialNumber
            // Assuming format M-XXXXX where X is digit.
            // String comparison works for fixed length padding. 
            // If length varies, we might need more complex logic or just rely on the fact we always pad to 5.
            
            var lastMachines = await _machineRepository.GetAsync(
                orderBy: q => q.OrderByDescending(m => m.SerialNumber)
            );
            var lastMachine = lastMachines.FirstOrDefault();

            if (lastMachine == null || string.IsNullOrEmpty(lastMachine.SerialNumber))
            {
                return "M-00001";
            }

            // Extract numeric part
            // format M-00001
            var parts = lastMachine.SerialNumber.Split('-');
            if (parts.Length < 2) return "M-00001"; // Fallback

            if (int.TryParse(parts[1], out int currentMax))
            {
                return $"M-{currentMax + 1:D5}";
            }

            return $"M-{Guid.NewGuid().ToString().Substring(0, 5)}"; // Fallback if parsing fails
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


        public async Task<MachineDto?> GetBySerialNumberAsync(string serialNumber)
        {
            // Case insensitive search
            var machines = await _machineRepository.GetAsync(m => m.SerialNumber.ToLower() == serialNumber.ToLower());
            var machine = machines.FirstOrDefault();
            return _mapper.Map<MachineDto>(machine);
        }

        public async Task<IReadOnlyList<MachineDto>> GetByUserIdAsync(int userId)
        {
            var all = await _machineRepository.GetAllWithDetailsAsync();
            // User sees machines they OWN OR machines they are RENTING (TenantId) using
            var mine = all.Where(m => m.UserId == userId || m.TenantId == userId).ToList();
            return _mapper.Map<IReadOnlyList<MachineDto>>(mine);
        }

        public async Task ReturnMachineAsync(int machineId, int userId)
        {
            var machine = await _machineRepository.GetByIdAsync(machineId);
            if (machine == null) throw new Exception("Machine not found");

            if (machine.TenantId != userId) throw new Exception("You can only return machines you are currently holding.");

            // Create a "Return" request log
            var returnLog = new MachineTransferRequest
            {
                MachineId = machineId,
                FromUserId = userId,
                ToUserId = machine.UserId.GetValueOrDefault(), // Returned to owner? Or just Available (0)?
                // Technically "ToUserId" is who received it. 
                // If returning to storage/pool (Available), maybe ToUserId is Owner or null?
                // Let's set ToUserId = Owner for traceability, even if status is Available.
                RequestType = Domain.Enums.RequestType.Return,
                Status = Domain.Enums.RequestStatus.Approved,
                Reason = "Machine Returned",
                CreatedAt = DateTime.UtcNow,
                ResolvedAt = DateTime.UtcNow
            };
            await _requestRepository.AddAsync(returnLog);

            machine.TenantId = null;
            machine.Status = Domain.Enums.MachineStatus.Available;

            await _machineRepository.UpdateAsync(machine);
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
        public async Task<IReadOnlyList<MachineTransferRequestDto>> GetHistoryAsync(int machineId)
        {
            // Use same DTO as requests for simplicity, or create specific History DTO.
            // Reusing MachineTransferRequestDto is fine.
            var requests = await _requestRepository.GetAsync(
                predicate: r => r.MachineId == machineId && (r.Status == Domain.Enums.RequestStatus.Approved || r.Status == Domain.Enums.RequestStatus.Rejected),
                includeProperties: "FromUser,ToUser",
                orderBy: q => q.OrderByDescending(r => r.CreatedAt)
            );

            return requests.Select(r => new MachineTransferRequestDto
            {
                Id = r.Id,
                MachineId = r.MachineId,
                FromUserId = r.FromUserId,
                FromUserName = r.FromUser?.FullName ?? "Unknown",
                ToUserId = r.ToUserId,
                ToUserName = r.ToUser?.FullName ?? "Unknown", // For return, this might be Owner
                RequestType = r.RequestType,
                Status = r.Status,
                Reason = r.Reason,
                CreatedAt = r.CreatedAt,
                ResolvedAt = r.ResolvedAt
            }).ToList();
        }
    }
}
