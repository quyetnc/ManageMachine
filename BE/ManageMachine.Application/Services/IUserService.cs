using ManageMachine.Application.DTOs.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
        Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
        Task ResetPasswordAsync(int userId, string newPassword);
        Task<UserDto> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteUserAsync(int id);
        Task<UserDto> GetUserByIdAsync(int id);
    }
}
