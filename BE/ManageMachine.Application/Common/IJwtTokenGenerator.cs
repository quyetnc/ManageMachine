using ManageMachine.Domain.Entities;

namespace ManageMachine.Application.Common
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(User user);
    }
}
