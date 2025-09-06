using Microsoft.Maui.Layouts;

namespace MPWordleClient;

public partial class PostGame : ContentPage
{
	public PostGame(Dictionary<string, int> playerScores)
	{
		InitializeComponent();
		CreateLeaderboard(playerScores);
	}

	private void CreateLeaderboard(Dictionary<string, int> scores)
	{
		var sorted_results = scores.OrderByDescending(entry => entry.Value);
        foreach(var result in sorted_results)
        {
            var horizontalStack = new FlexLayout()
            {
                JustifyContent = FlexJustify.SpaceBetween,
                FlowDirection = FlowDirection.LeftToRight,
            };

            var leftLabel = new Label
            {
                Text = result.Key,
                VerticalOptions = LayoutOptions.Start,
                FontSize = 30,
                Margin = new(10, 0, 0, 0),
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
            };

            var score = result.Value == -1 ? "0" : result.Value.ToString(); 
            var rightLabel = new Label
            {
                Text = score,
                VerticalOptions = LayoutOptions.End,
                FontSize = 30,
                Margin = new(0, 0, 10, 0),
                TextColor = Colors.Black,
                FontAttributes = FontAttributes.Bold,
            };

            horizontalStack.Children.Add(leftLabel);
            horizontalStack.Children.Add(rightLabel);
            PlayerList.Children.Add(horizontalStack);
        }
    }
}