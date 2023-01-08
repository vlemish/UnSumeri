using Serilog.Events;

namespace Core.Log.Models
{
    public class SerilogConfiguration
    {
        public LogEventLevel MinimumLogLevel { get; set; }
    
        public bool EnableFileLogging { get; set; }

        public bool EnableConsoleLogging{ get; set; }

        public string FilePath { get; set; }
    }
}
