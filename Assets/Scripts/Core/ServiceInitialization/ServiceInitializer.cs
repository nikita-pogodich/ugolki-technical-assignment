using System;
using System.Collections.Generic;
using System.Threading;
using Core.Services;
using Cysharp.Threading.Tasks;

namespace Core.ServiceInitialization
{
    public partial class ServiceInitializer : IServiceInitializer
    {
        private readonly List<Type> _servicesInitializationOrder = new();
        private readonly List<IInitializableService> _servicesToInitialize = new();

        partial void FillServicesInitializationOrder(List<Type> list);

        public ServiceInitializer()
        {
            FillServicesInitializationOrder(_servicesInitializationOrder);
        }

        public void AddService(IInitializableService initializableService)
        {
            _servicesToInitialize.Add(initializableService);
        }

        public void Dispose()
        {
            _servicesToInitialize.Clear();
        }

        public async UniTask InitializeAsync(CancellationToken cancellation)
        {
            _servicesToInitialize.Sort(ServicesComparison);

            foreach (IInitializableService initializableService in _servicesToInitialize)
            {
                await initializableService.InitializeAsync(cancellation);
            }
        }

        private int ServicesComparison(IInitializableService serviceA, IInitializableService serviceB)
        {
            int indexA = _servicesInitializationOrder.IndexOf(serviceA.GetType());
            int indexB = _servicesInitializationOrder.IndexOf(serviceB.GetType());
            return indexA.CompareTo(indexB);
        }
    }
}