using Serilog;

namespace LightsOut
{
    public partial class App
    {
        private const string DefaultConsoleOutputTemplate = "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] [@{SourceContext:l}] {Message}{NewLine}{Exception}";

        public App()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.ColoredConsole(outputTemplate: DefaultConsoleOutputTemplate)
                .CreateLogger();

            Log.Logger.Information("Logger Initialized");
        }
    }
}