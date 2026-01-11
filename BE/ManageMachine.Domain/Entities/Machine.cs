using ManageMachine.Domain.Common;
using System.Collections.Generic;

namespace ManageMachine.Domain.Entities
{
    public class Machine : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty; // For mobile app display
        public string QRCodeData { get; set; } = string.Empty; // Store unique string for QR code generation

        public int MachineTypeId { get; set; }
        public MachineType MachineType { get; set; } = null!;

        public int? UserId { get; set; }
        public User? User { get; set; }

        public ICollection<MachineParameter> Parameters { get; set; } = new List<MachineParameter>();
    }
}
