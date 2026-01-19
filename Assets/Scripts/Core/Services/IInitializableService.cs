using System.Threading;
using Cysharp.Threading.Tasks;

namespace Core.Services
{
    public interface IInitializableService
    {
        bool IsInitialized { get; }
        UniTask InitializeAsync(CancellationToken cancellation);
    }
}