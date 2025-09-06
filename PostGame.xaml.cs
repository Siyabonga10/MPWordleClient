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
        var style = Application.Current?.Resources["BaseLabel"] as Style;
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
                Style = style
            };

            var score = result.Value == -1 ? "0" : result.Value.ToString(); 
            var rightLabel = new Label
            {
                Text = score,
                VerticalOptions = LayoutOptions.End,
                Style = style
            };

            horizontalStack.Children.Add(leftLabel);
            horizontalStack.Children.Add(rightLabel);
            PlayerList.Children.Add(horizontalStack);
        }
    }
}