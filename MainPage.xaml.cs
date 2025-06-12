using Microsoft.Maui.Controls.Shapes;

namespace MPWordleClient
{
    public partial class MainPage : ContentPage
    {
        int RowIndex = 0;
        int ColumnIndex = 0;
        readonly WordManager GameWords;
        string currentWord;
        public MainPage()
        {
            InitializeComponent();
            GameWords = new WordManager("FiveLetterWords.txt");
            currentWord = GameWords.GenerateRandomWord().ToUpperInvariant();
            
            InitialiseGrid();
            InitialiseKeyboard();
        }

        private void InitialiseGrid()
        {
            for (int i = 0; i < 5; i++)
                for (int j = 0; j < 5; j++)
                    WordleUI.CreateAndAddGridCell(GridLayout, i, j);
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
            currentWord = GameWords.GenerateRandomWord().ToUpperInvariant();
            RowIndex = 0;
            ColumnIndex = 0;

            WordleUI.ResetGrid(GridLayout);
            Keyboard.Children.Clear();
            InitialiseKeyboard();
        }

        private HorizontalStackLayout CreateKeypadRow(string rowData, bool isEnd)
        {
            HorizontalStackLayout keyboardRow = new()
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 4,
                Padding = 2
            };

            if (isEnd)
                WordleUI.CreateAndAddKeypad(keyboardRow, "Enter", 37 + 37 / 2, 65, OnEnter);

            foreach (char letter in rowData.ToUpper())
                WordleUI.CreateAndAddKeypad(keyboardRow, letter.ToString(), 37, 65, OnKeypadButtonClicked);

            if (isEnd)
                WordleUI.CreateAndAddKeypad(keyboardRow, "Del", 37 + 37 / 2, 65, OnDelete);
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

        public void OnEnter(object? sender, EventArgs e)
        {
            if (ColumnIndex != 5)
                return;

            bool isCorrect = true;
            for (int i = 0; i < 5; i++)
            {
                Border? currentCell = WordleUI.GetGridElement(GridLayout, RowIndex, i);
                if (currentCell == null)
                    return;

                Label? label = (Label?)currentCell.Content;
                if (label == null)
                    return;
                
                if (label.Text == currentWord[i].ToString())
                {
                    WordleUI.SetKeypadColor(Keyboard, label.Text[0], Color.FromRgb(0, 100, 0));
                    currentCell.BackgroundColor = Color.FromRgb(0, 100, 0);
                }
                else if (currentWord.Contains(label.Text))
                {
                    WordleUI.SetKeypadColor(Keyboard, label.Text[0], Color.FromRgb(100, 100, 0));
                    currentCell.BackgroundColor = Color.FromRgb(100, 100, 0);
                    isCorrect = false;
                }
                else
                {
                    isCorrect = false;
                    WordleUI.SetKeypadColor(Keyboard, label.Text[0], Colors.Transparent); 
                }
            }
            if (isCorrect)
                CreatePopUp( "Congratulations!", "You guessed the word!");
            else if (RowIndex == 4)
                CreatePopUp("Game Over", $"The word was: {currentWord}");
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

    }
}
