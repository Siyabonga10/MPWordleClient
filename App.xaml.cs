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
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
   
            return new Window(new AppShell());
        }
    }
}