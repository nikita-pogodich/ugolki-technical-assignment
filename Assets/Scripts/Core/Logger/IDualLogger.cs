#nullable enable

namespace Core.Logger
{
    public interface IDualLogger
    {
        ILogger Mandatory { get; }
        ILogger? Debug { get; }
    }
}