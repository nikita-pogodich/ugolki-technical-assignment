using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.ServiceInitializer
{
    public class ServiceInitializer : IServiceInitializer
    {
        private readonly List<IInitializableService> _services = new();

        public void AddService(IInitializableService initializableService)
        {
            _services.Add(initializableService);
        }

        public void Dispose()
        {
            _services.Clear();
        }

        public async UniTask InitializeAsync(CancellationToken cancellation)
        {
            foreach (IInitializableService initializableService in _services)
            {
                await initializableService.InitializeAsync(cancellation);
            }
        }
    }
}