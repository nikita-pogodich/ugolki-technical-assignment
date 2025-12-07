using System;
using UnityEngine;

namespace Core.Logger
{
    public class UnityLogger : ILogger
    {
        [HideInCallstack]
        public void LogInfo(string message)
        {
            Debug.Log(message);
        }

        [HideInCallstack]
        public void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }

        [HideInCallstack]
        public void LogError(string message)
        {
            Debug.LogError(message);
        }

        [HideInCallstack]
        public void LogException(string message, Exception exception)
        {
            Debug.LogError(message);
        }
    }
}