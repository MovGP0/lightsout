using Serilog;

namespace LightsOut
{
    public partial class App
    {
        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole()
                .CreateLogger();

            Log.Logger.Information("Logger Initialized");
        }
    }
}