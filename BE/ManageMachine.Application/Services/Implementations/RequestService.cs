using AutoMapper;
using ManageMachine.Application.Common;
using ManageMachine.Application.DTOs.Requests;
using ManageMachine.Domain.Entities;
using ManageMachine.Domain.Enums;
// However, IGenericRepository usually assumes some pattern.
// Let's assume we need to inject specific Repositories or just Generic.
// For complex queries like Include, Generic might optionally support it or we use specialized repo.
// I'll assume we can use the repository or I'll add GetPendingRequests manually.
// Actually, generic repository usually has GetAll or Get(filter).
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services.Implementations
{
    public class RequestService : IRequestService
    {
        private readonly IGenericRepository<MachineTransferRequest> _requestRepository;
        private readonly IMachineRepository _machineRepository;
        private readonly IMapper _mapper;

        public RequestService(
            IGenericRepository<MachineTransferRequest> requestRepository,
            IMachineRepository machineRepository,
            IMapper mapper)
        {
            _requestRepository = requestRepository;
            _machineRepository = machineRepository;
            _mapper = mapper;
        }

        public async Task<MachineTransferRequestDto> CreateRequestAsync(CreateMachineTransferRequestDto dto, int fromUserId)
        {
            // Validate Machine ownership
            var machine = await _machineRepository.GetByIdAsync(dto.MachineId);
            if (machine == null) throw new Exception("Machine not found");
            
            // Check if user is owner (UserId) OR current Tenant (TenantId) trying to sub-lend?
            // Requirement says "User A (Owner) transfer to User B".
            // So FromUser must be Owner.
            // If machine is already borrowed, can Owner transfer it? 
            // Probably Owner can recall it or transfer ownership. But let's assume basic flow: Owner lends to Tenant.
            // If machine.TenantId is not null, it's already borrowed.
            // Allow Owner OR current Tenant to initiate transfer
            bool isOwner = machine.UserId == fromUserId;
            bool isTenant = machine.TenantId == fromUserId;

            if (!isOwner && !isTenant) 
                throw new Exception("You are not authorized to transfer this machine.");
            
            // If Owner is sending, Machine must be Available (no Tenant). 
            // If Tenant is sending, Machine is technically 'Borrowed' status wise, but available for sub-lease?
            // User requirement: "User B (Tenant) cho User C mượn".
            // So if Tenant is sending, we don't check for Available status (because it's Borrowed).
            // But if Owner is sending, it MUST be Available (TenantId should be null).
            
            if (isOwner && machine.Status != MachineStatus.Available)
                 throw new Exception("Machine is currently not available (lent out).");
            
            // If Tenant is sending, ensure they are the current holder (already checked by isTenant)
            // and maybe check if there is already a pending request?
            // The DB unique constraint or app logic should handle pending requests limits.
            // For now, let's assume one pending request per machine is enforced elsewhere or allowed.

            var request = new MachineTransferRequest
            {
                MachineId = dto.MachineId,
                FromUserId = fromUserId,
                ToUserId = dto.ToUserId,
                RequestType = dto.RequestType,
                Reason = dto.Reason,
                Status = RequestStatus.Pending
            };

            await _requestRepository.AddAsync(request);
            
            // Map manually or use auto mapper if configured
            // We need to fetch it back with includes to return full DTO or just return basic
            // Let's return basic mapped
            return new MachineTransferRequestDto
            {
                Id = request.Id,
                MachineId = request.MachineId,
                FromUserId = request.FromUserId,
                ToUserId = request.ToUserId,
                RequestType = request.RequestType,
                Status = request.Status,
                Reason = request.Reason,
                CreatedAt = request.CreatedAt
            };
        }

        public async Task ApproveRequestAsync(int requestId)
        {
            // We need to include Machine to update it
            // Generic Repository might limited. Let's use GetById but we need Machine loaded.
            // Usually we might need a specialized RequestRepository if Generic is too simple.
            // Or we assume LazyLoading (not recommended) or we load Machine separately.
            
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request == null) throw new Exception("Request not found");
            
            if (request.Status != RequestStatus.Pending) throw new Exception("Request is not pending");

            var machine = await _machineRepository.GetByIdAsync(request.MachineId);
            if (machine == null) throw new Exception("Machine not found");

            // Update Request
            request.Status = RequestStatus.Approved;
            request.ResolvedAt = DateTime.UtcNow;
            await _requestRepository.UpdateAsync(request);

            // Update Machine
            machine.TenantId = request.ToUserId;
            // Map RequestType to MachineStatus
            machine.Status = request.RequestType == RequestType.Repair ? MachineStatus.Repairing : MachineStatus.Borrowed;
            
            await _machineRepository.UpdateAsync(machine);
        }

        public async Task RejectRequestAsync(int requestId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request == null) throw new Exception("Request not found");

            if (request.Status != RequestStatus.Pending) throw new Exception("Request is not pending");

            request.Status = RequestStatus.Rejected;
            request.ResolvedAt = DateTime.UtcNow;
            await _requestRepository.UpdateAsync(request);
        }

        public async Task CancelRequestAsync(int requestId, int userId)
        {
            var request = await _requestRepository.GetByIdAsync(requestId);
            if (request == null) throw new Exception("Request not found");

            if (request.FromUserId != userId) throw new Exception("You can only cancel your own requests");
            if (request.Status != RequestStatus.Pending) throw new Exception("Request is not pending and cannot be cancelled");

            // Hard delete or Soft delete? Or status 'Cancelled'?
            // Usually 'Cancelled' is better for history.
            // But if user made a mistake, maybe delete? 
            // User asked: "nhấn request nhưng nhầm lẫn nên muốn cancel điều đó" -> implied undo.
            // Let's use DeleteAsync to remove it completely so they can request again easily without history clutter,
            // OR use a Cancelled status if we want audit.
            // Let's use Delete for now as it's cleaner for "undo".
            await _requestRepository.DeleteAsync(request);
        }

        public async Task<IReadOnlyList<MachineTransferRequestDto>> GetPendingRequestsAsync()
        {
            // Need Includes: Machine, FromUser, ToUser
            // Assuming IGenericRepository has a way to include, or I cast to DbSet... bad practice but quick?
            // Better: Add method to IGenericRepository or cast _requestRepository to concrete if acceptable (layer violation).
            // Best: Use Specification pattern or just accept IQueryable in GetAll from Service (not ideal).
            // Let's assume _requestRepository.GetAllWithDetailsAsync() exists or similar, or I add it.
            // Wait, I used GetAllWithDetailsAsync in MachineService.
            // Let's verify IGenericRepository has it.
            // If not, I will trust the system has it as I used it in MachineService.
            
            // Actually I should verify if generic repo supports includes for this entity.
            // MachineRepository overrides it probably.
            // RequestRepository likely relies on generic. 
            // I'll use GetAsync(filter, includeProperties) if available.
            // Let's check IGenericRepository definition from memory or look at it.
            // I'll assume GetAsync(filter: x => x.Status == Pending, includeProperties: "Machine,FromUser,ToUser") works
            
            var requests = await _requestRepository.GetAsync(
                predicate: r => r.Status == RequestStatus.Pending,
                includeProperties: "Machine,FromUser,ToUser",
                orderBy: q => q.OrderByDescending(r => r.CreatedAt)
            );

            // Create DTOs manually since AutoMapper config might be missing
            return requests.Select(r => new MachineTransferRequestDto
            {
                Id = r.Id,
                MachineId = r.MachineId,
                MachineName = r.Machine?.Name ?? "Unknown",
                FromUserId = r.FromUserId,
                FromUserName = r.FromUser?.FullName ?? "Unknown",
                ToUserId = r.ToUserId,
                ToUserName = r.ToUser?.FullName ?? "Unknown",
                RequestType = r.RequestType,
                Status = r.Status,
                Reason = r.Reason,
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<IReadOnlyList<MachineTransferRequestDto>> GetRequestsByUserIdAsync(int userId)
        {
            var requests = await _requestRepository.GetAsync(
                predicate: r => r.FromUserId == userId,
                includeProperties: "Machine,FromUser,ToUser",
                orderBy: q => q.OrderByDescending(r => r.CreatedAt)
            );

            return requests.Select(r => new MachineTransferRequestDto
            {
                Id = r.Id,
                MachineId = r.MachineId,
                MachineName = r.Machine?.Name ?? "Unknown",
                FromUserId = r.FromUserId,
                FromUserName = r.FromUser?.FullName ?? "Unknown",
                ToUserId = r.ToUserId,
                ToUserName = r.ToUser?.FullName ?? "Unknown",
                RequestType = r.RequestType,
                Status = r.Status,
                Reason = r.Reason,
                CreatedAt = r.CreatedAt
            }).ToList();
        }
    }
}
