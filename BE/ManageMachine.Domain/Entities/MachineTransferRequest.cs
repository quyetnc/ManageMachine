using ManageMachine.Domain.Common;
using ManageMachine.Domain.Enums;
using System;

namespace ManageMachine.Domain.Entities
{
    public class MachineTransferRequest : BaseEntity
    {
        public int MachineId { get; set; }
        public Machine Machine { get; set; } = null!;

        public int FromUserId { get; set; }
        public User FromUser { get; set; } = null!;

        public int ToUserId { get; set; }
        public User ToUser { get; set; } = null!;

        public RequestType RequestType { get; set; }
        public RequestStatus Status { get; set; } = RequestStatus.Pending;
        public string Reason { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ResolvedAt { get; set; }
    }
}
