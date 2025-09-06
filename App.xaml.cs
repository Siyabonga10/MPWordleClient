using Microsoft.Extensions.Logging;


namespace MPWordleClient
{
    public partial class App : Application
    {
        ILogger<App> _logger;
        public App(ILogger<App> logger)
        {
            InitializeComponent();
            _logger = logger;
            AppLogger.InitialiseLogger(logger);
            _logger.LogInformation($"Resource count: {Resources.Count}");

            // Check specific resources
            if (Resources.TryGetValue("Background", out var bgColor))
                _logger.LogInformation($"Background color found: {bgColor}");
            else
                _logger.LogInformation("Background color NOT found!");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
   
            return new Window(new AppShell());
        }
    }
}