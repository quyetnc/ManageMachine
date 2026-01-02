using System;
using System.Threading.Tasks;

namespace ManageMachine.Application.Common
{
    public interface IUnitOfWork : IDisposable
    {
        Task<int> SaveChangesAsync();
    }
}
