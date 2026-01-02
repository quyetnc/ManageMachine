using ManageMachine.Domain.Common;
using System.Collections.Generic;

namespace ManageMachine.Domain.Entities
{
    public class MachineType : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation property for Machines of this type
        public ICollection<Machine> Machines { get; set; } = new List<Machine>();
    }
}
