using ManageMachine.Domain.Enums;
using System;

namespace ManageMachine.Application.DTOs.Requests
{
    public class MachineTransferRequestDto
    {
        public int Id { get; set; }
        public int MachineId { get; set; }
        public string MachineName { get; set; } = string.Empty;
        public int FromUserId { get; set; }
        public string FromUserName { get; set; } = string.Empty;
        public int ToUserId { get; set; }
        public string ToUserName { get; set; } = string.Empty;
        public RequestType RequestType { get; set; }
        public RequestStatus Status { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? ResolvedAt { get; set; }
    }

    public class CreateMachineTransferRequestDto
    {
        public int MachineId { get; set; }
        public int ToUserId { get; set; }
        public RequestType RequestType { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
