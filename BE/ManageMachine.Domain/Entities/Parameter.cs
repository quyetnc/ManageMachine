using ManageMachine.Domain.Common;
using System.Collections.Generic;

namespace ManageMachine.Domain.Entities
{
    // Defines what kind of parameters can exist (e.g., "Operating Temperature", "C")
    public class Parameter : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty; // e.g., "RPM", "°C", "kg"
        public string Description { get; set; } = string.Empty;

        public ICollection<MachineParameter> MachineParameters { get; set; } = new List<MachineParameter>();
    }
}
