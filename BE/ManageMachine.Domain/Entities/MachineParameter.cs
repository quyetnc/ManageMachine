using ManageMachine.Domain.Common;

namespace ManageMachine.Domain.Entities
{
    // Stores the specific value of a parameter for a machine
    public class MachineParameter : BaseEntity
    {
        public int MachineId { get; set; }
        public Machine Machine { get; set; } = null!;

        public int ParameterId { get; set; }
        public Parameter Parameter { get; set; } = null!;

        public string Value { get; set; } = string.Empty; // Storing as string to be flexible (or decimal if strictly numeric)
    }
}
