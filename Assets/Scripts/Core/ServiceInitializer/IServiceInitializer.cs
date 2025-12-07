using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.ServiceInitializer
{
    public interface IServiceInitializer : IDisposable
    {
        void AddService(IInitializableService initializableService);
        UniTask InitializeAsync(CancellationToken cancellation);
    }
}