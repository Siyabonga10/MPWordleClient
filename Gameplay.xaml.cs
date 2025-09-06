using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Timers;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.Controls.Shapes;

namespace MPWordleClient
{
    public partial class Gameplay : ContentPage, INotifyPropertyChanged
    {
        int RowIndex = 0;
        int ColumnIndex = 0;
        readonly int height;
        readonly int width;
        Task? computingResult;
        private int current_time = 30;
        private string _timeText;
        public string TimeText
        {
            get => _timeText;
            set
            {
                _timeText = value;
                OnPropertyChanged();
            }
        }
        private System.Timers.Timer timer;
        private readonly ILogger<Gameplay> _logger;
        public event PropertyChangedEventHandler? _propertyChanged;
        public Gameplay(ILogger<Gameplay> logger)
        {
            _logger = logger;
            computingResult = null;
            
            InitializeComponent();
            WordManager.GetNewWord();

            var sizeInfo = DeviceDisplay.Current.MainDisplayInfo;
            height = (int)(sizeInfo.Height / sizeInfo.Density);
            width = (int)(sizeInfo.Width / sizeInfo.Density);

            WordleUI.DisplayHeight = height;
            WordleUI.DisplayWidth = width;

            InitialiseGrid();
            InitialiseKeyboard();
            _timeText = "00:00";
            current_time = 30;
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += OnEverySecond;
            timer.AutoReset = true;
            timer.Enabled = true;
            this.BindingContext = this;
            TimerLabel.SetBinding(Label.TextProperty, static (Gameplay pg) => pg.TimeText);

            MpClient.GameOverUpdate += OnGameOver;
        }

        private async void OnEverySecond(object? Timer, ElapsedEventArgs e)
        {
            current_time -= 1;
            TimeText = "";
            if(current_time >= 60)
            {
                TimeText += $"{current_time / 60}:";
            }
            TimeText += (current_time % 60).ToString();
            if(current_time == -1)
            {
                timer.Enabled = false;
                timer.AutoReset = false;
                await MpClient.SubmitPostGameResults(Game.GameID, 10);
            }
        }

        private void InitialiseGrid()
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    WordleUI.CreateAndAddGridCell(GridLayout, i, j);

            GridLayout.Padding = new Thickness(0, 0.2 * height, 0, 0);
        }

        private void InitialiseKeyboard()
        {
            string[] rows = { "qwertyuiop", "asdfghjkl", "zxcvbnm" };

            int rowIndex = 0;
            foreach (string row in rows)
            {

                Keyboard.Add(CreateKeypadRow(row, rowIndex == 2), 0, rowIndex);
                rowIndex++;
            }
        }

        private void Reset()
        {
            RowIndex = 0;
            ColumnIndex = 0;

            WordleUI.ResetGrid(GridLayout);
            Keyboard.Children.Clear();
            InitialiseKeyboard();
            WordManager.GetNewWord();
        }

        private HorizontalStackLayout CreateKeypadRow(string rowData, bool isEnd)
        {
            var dimension = width / 11;
            dimension = Math.Clamp(dimension, 20, 60);
            var keyHeight = (int)(1.3 * dimension);
            HorizontalStackLayout keyboardRow = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 4,
                Padding = 2
            };

            if (isEnd)
                WordleUI.CreateAndAddKeypad(keyboardRow, "Enter", dimension + dimension / 2, keyHeight, OnEnter);

            foreach (char letter in rowData.ToUpper())
                WordleUI.CreateAndAddKeypad(keyboardRow, letter.ToString(), dimension, keyHeight, OnKeypadButtonClicked);

            if (isEnd)
                WordleUI.CreateAndAddKeypad(keyboardRow, "Del", dimension + dimension / 2, keyHeight, OnDelete);
            return keyboardRow;
        }

        private void OnKeypadButtonClicked(object? sender, EventArgs e)
        {
            if (sender is Border button)
            {
                Label? label = (Label?)button.Content;
                string? letter = label?.Text;

                if (ColumnIndex < 5)
                {
                    var targetCell = WordleUI.GetGridElement(GridLayout, RowIndex, ColumnIndex);
                    if (targetCell != null)
                    {
                        Label? targetGrid = (Label?)targetCell.Content;
                        if (targetGrid != null)
                        {
                            targetGrid.Text = letter;
                            ColumnIndex++;
                        }
                    }
                }
            }
        }

        public void OnDelete(object? sender, EventArgs e)
        {
            ColumnIndex = Math.Clamp(ColumnIndex - 1, 0, 5);
            var targetCell = WordleUI.GetGridElement(GridLayout, RowIndex, ColumnIndex);
            if (targetCell != null)
            {
                Label? targetGrid = (Label?)targetCell.Content;
                if (targetGrid != null)
                {
                    targetGrid.Text = "";
                }
            }
        }

        public async void OnEnter(object? sender, EventArgs e)
        {
            if (ColumnIndex != 5 || computingResult != null)
                return;

            string word = string.Empty;
            for(int i = 0; i < 5; i++)
            {
                Border? currentCell = WordleUI.GetGridElement(GridLayout, RowIndex, i);
                if (currentCell == null)
                    return;
                Label? label = (Label?)currentCell.Content;
                if (label == null)
                    return;
                word += label.Text.ToUpperInvariant();
            }
            if (!WordManager.IsWordValid(word)) return;
            WordResult result = WordManager.GetResults(word);
            try
            {
                computingResult = WordleUI.SetGridRowColor(GridLayout, RowIndex, result.colorCodes);
                await computingResult;
                WordleUI.SetKeypadLetterColors(Keyboard, word, result.colorCodes);
                computingResult = null;
            } catch (Exception ex)
            {
                Console.WriteLine($"Error setting grid row color: {ex.Message}");
                return;
            }
                 
            if (result.isCorrect)
                CreatePopUp( "Congratulations!", "You guessed the word!");
            else if (RowIndex == 4)
                CreatePopUp("Game Over", $"The word was: {WordManager.CurrentWord}");
            else
            {
                RowIndex++;
                ColumnIndex = 0;
            }
        }

        private async void CreatePopUp(string heading, string message)
        {
            await DisplayAlert(heading, message, "OK");
            Reset();
        }

        protected override void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            _propertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName)); 
            base.OnPropertyChanged(propertyName);
        }

        public async void OnGameOver(Object? sender, Dictionary<string, int> scores_data)
        {
            await Navigation.PushModalAsync(new PostGame(scores_data));
        }

    }
}
