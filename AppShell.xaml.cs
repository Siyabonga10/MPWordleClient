namespace MPWordleClient
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("GameCreation", typeof(GameCreation));
            Routing.RegisterRoute("Waiting", typeof(Waiting));
            Routing.RegisterRoute("Gameplay", typeof(Gameplay));
        }
    }
}
