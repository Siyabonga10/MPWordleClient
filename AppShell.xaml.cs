namespace MPWordleClient
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("Waiting", typeof(Waiting));
        }
    }
}
