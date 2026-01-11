using ManageMachine.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManageMachine.Infrastructure.Persistence.Seeders
{
    public class DbSeeder
    {
        private readonly AppDbContext _context;

        public DbSeeder(AppDbContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            // 1. Seed Machine Types
            if (!await _context.MachineTypes.AnyAsync())
            {
                var types = new List<MachineType>
                {
                    new MachineType { Name = "CNC Lathe", Description = "Computer Numerical Control Lathe" },
                    new MachineType { Name = "Milling Machine", Description = "Vertical Milling Machine" },
                    new MachineType { Name = "3D Printer", Description = "Fused Deposition Modeling Printer" },
                    new MachineType { Name = "Laser Cutter", Description = "CO2 Laser Cutter and Engraver" },
                    new MachineType { Name = "Drill Press", Description = "Benchtop Drill Press" },
                    new MachineType { Name = "Welding Station", Description = "MIG/TIG Welding Setup" }
                };
                await _context.MachineTypes.AddRangeAsync(types);
                await _context.SaveChangesAsync();
            }

            // 2. Update existing users with realistic names if they have generic names like "User 1"
            // Or just ensure we have some users to assign machines to.
            var users = await _context.Users.ToListAsync();
            var realisticNames = new List<string> { "Administrator", "Đơn Vị 1", "Đơn Vị 2", "Đơn Vị 3", "Đơn Vị 4" };
            
            for (int i = 0; i < users.Count; i++)
            {
                // Simple logic: if name is short/generic, update it. Or just update all for demo.
                // Let's update all to ensure "real" look, cycling through realistic names.
                if (i < realisticNames.Count)
                {
                    users[i].FullName = realisticNames[i];
                }
                else
                {
                    users[i].FullName = $"Employee {i + 1}";
                }
            }
            if (users.Any()) await _context.SaveChangesAsync();


            // 3. Seed Machines if few exist, or update existing ones
            var machines = await _context.Machines.Include(m => m.MachineType).ToListAsync();
            var machineTypes = await _context.MachineTypes.ToListAsync();
            var random = new Random();

            var realisticMachines = new List<(string Name, string Type)>
            {
                ("Haas VF-2", "Milling Machine"),
                ("Prusa i3 MK3S+", "3D Printer"),
                ("Mazak Quick Turn 250", "CNC Lathe"),
                ("Glowforge Pro", "Laser Cutter"),
                ("Bridgeport Series I", "Milling Machine"),
                ("Ultimaker S5", "3D Printer"),
                ("Lincoln Electric Power MIG", "Welding Station"),
                ("Jet JDP-17", "Drill Press"),
                ("Formlabs Form 3+", "3D Printer"),
                ("Tormach PCNC 440", "Milling Machine")
            };

            // Strategy: Update existing machines first
            for (int i = 0; i < machines.Count; i++)
            {
                var target = realisticMachines[i % realisticMachines.Count];
                var type = machineTypes.FirstOrDefault(t => t.Name == target.Type) ?? machineTypes.First();
                
                machines[i].Name = target.Name;
                machines[i].MachineTypeId = type.Id;
                machines[i].Description = $"High performance {target.Name} used for precision work. Maintained regularly.";
                machines[i].SerialNumber = $"M-{random.Next(10000, 99999)}";
                
                // Randomly assign owner if null
                if (machines[i].UserId == null && users.Any())
                {
                    machines[i].UserId = users[random.Next(users.Count)].Id;
                }
            }
            
            // If we have very few machines (< 5), add some more
            if (machines.Count < 5 && users.Any())
            {
                for (int i = machines.Count; i < 5; i++) // Ensure at least 5
                {
                    var target = realisticMachines[i % realisticMachines.Count];
                    var type = machineTypes.FirstOrDefault(t => t.Name == target.Type) ?? machineTypes.First();
                    
                    var newMachine = new Machine
                    {
                        Name = target.Name,
                        MachineTypeId = type.Id,
                        Description = $"High performance {target.Name} used for precision work.",
                        SerialNumber = $"M-{random.Next(10000, 99999)}",
                        UserId = users[random.Next(users.Count)].Id,
                        Status = Domain.Enums.MachineStatus.Available,
                        ImageUrl = "" 
                    };
                    _context.Machines.Add(newMachine);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
