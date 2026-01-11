using ManageMachine.Domain.Common;
using ManageMachine.Domain.Enums;
using System.Collections.Generic;

namespace ManageMachine.Domain.Entities
{
    public class Machine : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // For mobile app display
        public string QRCodeData { get; set; } = string.Empty; // Store unique string for QR code generation
        public string SerialNumber { get; set; } = string.Empty; // Human-readable unique code (e.g. M-12345)

        public int MachineTypeId { get; set; }
        public MachineType MachineType { get; set; } = null!;

        public int? UserId { get; set; }
        public User? User { get; set; } // Owner

        // Lending System
        public int? TenantId { get; set; }
        public User? Tenant { get; set; } // Current Holder
        public MachineStatus Status { get; set; } = MachineStatus.Available;

        public ICollection<MachineParameter> Parameters { get; set; } = new List<MachineParameter>();
        public ICollection<MachineTransferRequest> TransferRequests { get; set; } = new List<MachineTransferRequest>();
    }
}
