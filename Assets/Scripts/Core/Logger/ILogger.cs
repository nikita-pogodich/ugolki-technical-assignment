using System;

namespace Core.Logger
{
    public interface ILogger
    {
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(string message);
        void LogException(string message, Exception exception);
    }
}