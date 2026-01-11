using System;
using System.Collections.Generic;

namespace ManageMachine.Application.DTOs.Machine
{
    public class MachineTypeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class CreateMachineTypeDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class ParameterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
    
    public class CreateParameterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Unit { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class MachineParameterDto
    {
        public int Id { get; set; } // Entity Id if needed, or just display info
        public int ParameterId { get; set; }
        public string ParameterName { get; set; } = string.Empty;
        public string ParameterUnit { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class CreateMachineParameterDto
    {
        public int ParameterId { get; set; }
        public string Value { get; set; } = string.Empty;
    }
    
    public class UpdateMachineParameterDto
    {
        public int Id { get; set; } // MachineParameter Id? Or identify by param id. Better use ID.
        public string Value { get; set; } = string.Empty;
    }

    public class MachineDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string QRCodeData { get; set; } = string.Empty;
        public int MachineTypeId { get; set; }
        public string MachineTypeName { get; set; } = string.Empty;
        public int? UserId { get; set; } // Owner
        public string? UserFullName { get; set; } 
        public DateTime CreatedAt { get; set; }

        public List<MachineParameterDto> Parameters { get; set; } = new List<MachineParameterDto>();
    }

    public class CreateMachineDto
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int MachineTypeId { get; set; }
        public int? UserId { get; set; } // Admin assigns this
        // Potentially accept parameters on creation
        public List<CreateMachineParameterDto> Parameters { get; set; } = new List<CreateMachineParameterDto>();
    }
}
