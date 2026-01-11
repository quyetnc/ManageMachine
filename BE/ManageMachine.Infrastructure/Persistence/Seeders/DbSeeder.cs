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
            // 1. Seed Machine Types (Upsert)
            var types = new List<MachineType>
            {
                new MachineType { Name = "Vũ khí bộ binh", Description = "Súng trường, súng ngắn, đạn dược" },
                new MachineType { Name = "Phương tiện chiến đấu", Description = "Xe tăng, xe bọc thép, xe vận tải" },
                new MachineType { Name = "Thiết bị thông tin", Description = "Máy bộ đàm, điện thoại chiến trường" },
                new MachineType { Name = "Khí tài quang học", Description = "Ống nhòm, kính ngắm, thiết bị đo xa" },
                new MachineType { Name = "Quân trang", Description = "Quần áo, mũ, giày, ba lô" }
            };

            foreach (var type in types)
            {
                if (!await _context.MachineTypes.AnyAsync(t => t.Name == type.Name))
                {
                    _context.MachineTypes.Add(type);
                }
            }
            await _context.SaveChangesAsync();

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
                ("Súng trường tấn công AK-47", "Vũ khí bộ binh"),
                ("Xe tăng chiến đấu chủ lực T-90S", "Phương tiện chiến đấu"),
                ("Máy bộ đàm chiến thuật PRC-25", "Thiết bị thông tin"),
                ("Ống nhòm quân sự 8x30", "Khí tài quang học"),
                ("Súng ngắn K-54", "Vũ khí bộ binh"),
                ("Xe bọc thép BTR-60PB", "Phương tiện chiến đấu"),
                ("Mũ chống đạn A2", "Quân trang"),
                ("Giày quân nhu cấp tá", "Quân trang"),
                ("Kính ngắm quang học PSO-1", "Khí tài quang học"),
                ("Đài vô tuyến điện sóng ngắn VRH-811", "Thiết bị thông tin")
            };

            // Strategy: Update existing machines first
            // Also enforce sequential serial numbers
            int currentSerial = 1;

            for (int i = 0; i < machines.Count; i++)
            {
                var target = realisticMachines[i % realisticMachines.Count];
                var type = machineTypes.FirstOrDefault(t => t.Name == target.Type) ?? machineTypes.First();
                
                machines[i].Name = target.Name;
                machines[i].MachineTypeId = type.Id;
                machines[i].Description = $"{target.Name} - Trang bị tiêu chuẩn. Đã qua kiểm tra kỹ thuật. Sẵn sàng chiến đấu/làm nhiệm vụ.";
                
                // Set Sequential Serial Number
                string code = $"M-{currentSerial:D5}";
                machines[i].SerialNumber = code;
                currentSerial++;
                
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
                    
                    var code = $"M-{currentSerial:D5}";
                    currentSerial++;

                    var newMachine = new Machine
                    {
                        Name = target.Name,
                        MachineTypeId = type.Id,
                        Description = $"{target.Name} - Trang bị mới nhập kho. Tình trạng kỹ thuật tốt.",
                        SerialNumber = code,
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
