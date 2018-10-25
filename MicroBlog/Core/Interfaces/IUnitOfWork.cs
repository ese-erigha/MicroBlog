using System;
using System.Threading.Tasks;

namespace MicroBlog.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        Task<bool> Commit();
    }
}
