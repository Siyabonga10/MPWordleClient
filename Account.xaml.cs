namespace MPWordleClient;

public partial class Account : ContentPage
{
	public Account()
	{
		InitializeComponent();
		LoginBtn.WidthRequest = CreateAccBtn.Width;
	}

	public async void OnPlayerLogin(object sender, EventArgs e)
	{
		var username = await DisplayPromptAsync("Login", "Username", maxLength: 10);
		var password = await DisplayPromptAsync("Login", "Password", maxLength: 10);

		var response = await MpClient.LoginPlayerAsync(username, password);
		if (response.LoggedIn)
		{
			Application.Current.MainPage = new GameCreation();
        }
		else
			await DisplayAlert("Login failed", "", response.OutcomeMsg);
	}
    public async void OnPlayerSignup(object sender, EventArgs e)
    {
        var username = await DisplayPromptAsync("Create Account", "Username", maxLength: 10);
        var password = await DisplayPromptAsync("Create Account", "Password", maxLength: 10);

        var response = await MpClient.CreatePlayerAsync(username, password);
		if (response.LoggedIn)
            Application.Current.MainPage = new GameCreation();
        else
			await DisplayAlert("Login failed", "", response.OutcomeMsg);
    }
}