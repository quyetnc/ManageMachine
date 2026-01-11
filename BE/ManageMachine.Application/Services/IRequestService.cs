using ManageMachine.Application.DTOs.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services
{
    public interface IRequestService
    {
        Task<MachineTransferRequestDto> CreateRequestAsync(CreateMachineTransferRequestDto dto, int fromUserId);
        Task ApproveRequestAsync(int requestId);
        Task RejectRequestAsync(int requestId);
        Task CancelRequestAsync(int requestId, int userId);
        Task<IReadOnlyList<MachineTransferRequestDto>> GetPendingRequestsAsync();
        Task<IReadOnlyList<MachineTransferRequestDto>> GetRequestsByUserIdAsync(int userId);
    }
}
