using ManageMachine.Application.DTOs.Auth;
using System.Threading.Tasks;

namespace ManageMachine.Application.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
    }
}
