#nullable enable

namespace Core.Logger
{
    public class DualLogger : IDualLogger
    {
        public DualLogger(ILogger mandatory, ILogger? debug)
        {
            Mandatory = mandatory;
            Debug = debug;
        }

        public ILogger Mandatory { get; }
        public ILogger? Debug { get; }
    }
}