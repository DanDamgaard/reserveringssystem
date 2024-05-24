using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace program.Service
{

    public class Logger
    {
        public Logger()
        {
            Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Debug()
                    .WriteTo.Console()
                    .WriteTo.File("Logs\\Log.txt")
                    .CreateLogger();
        }
        public void Information(string message)
        {
            Log.Information(message);
        }

        public void Error(string message)
        {
            Log.Error(message);
        }
        public void Warning(string message)
        {
            Log.Warning(message);
        }
    }


}
