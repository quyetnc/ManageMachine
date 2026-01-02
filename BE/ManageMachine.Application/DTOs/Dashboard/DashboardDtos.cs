using System.Collections.Generic;

namespace ManageMachine.Application.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public int TotalMachines { get; set; }
        public int TotalTypes { get; set; }
        public List<MachinesByTypeDto> MachinesByType { get; set; } = new List<MachinesByTypeDto>();
    }

    public class MachinesByTypeDto
    {
        public string TypeName { get; set; } = string.Empty;
        public int Count { get; set; }
    }
}
