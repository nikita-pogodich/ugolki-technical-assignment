using System;
using System.Threading;
using Core.Services;
using Cysharp.Threading.Tasks;

namespace Core.ServiceInitialization
{
    public interface IServiceInitializer : IDisposable
    {
        void AddService(IInitializableService initializableService);
        UniTask InitializeAsync(CancellationToken cancellation);
    }
}