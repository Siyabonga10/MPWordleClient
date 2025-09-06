using Microsoft.Extensions.Logging;

namespace MPWordleClient;

public partial class Account : ContentPage
{
	private ILogger<Account> _logger;
	public Account(ILogger<Account> logger)
	{
		_logger = logger;
		InitializeComponent();
		LoginBtn.WidthRequest = CreateAccBtn.Width;

    }

    override protected async void OnAppearing()
	{
		base.OnAppearing();
		if(await MpClient.CheckLoggedin())
            await Navigation.PushModalAsync(new GameCreation());
    }
	public async void OnPlayerLogin(object sender, EventArgs e)
	{
		var username = await DisplayPromptAsync("Login", "Username", maxLength: 10);
		var password = await DisplayPromptAsync("Login", "Password", maxLength: 10);

		var response = await MpClient.LoginPlayerAsync(username, password);
		if (response.LoggedIn)
		{
			await Navigation.PushModalAsync(new GameCreation());
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
            await Navigation.PushModalAsync(new GameCreation());
        else
			await DisplayAlert("Login failed", "", response.OutcomeMsg);
    }
}