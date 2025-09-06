using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;

namespace MPWordleClient
{
    public class WordleUI
    {
        public static int DisplayHeight { get; set; } = 0;
        public static int DisplayWidth { get; set; } = 0;
        public static Border? GetGridElement(Grid grid, int row, int col)
        {
            return (Border?)grid.Children
                .FirstOrDefault(child => grid.GetRow(child) == row && grid.GetColumn(child) == col);
        }

        private static Border CreateKeypad(int width, int height)
        {
            var color = Application.Current?.Resources["BorderAccent"] as Color;
            Border keypad = new()
            {
      
                StrokeThickness = 2,
                Stroke = new SolidColorBrush(color),
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(2)
                },
                Content = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 20,
                    HeightRequest = height - 4,
                    WidthRequest = width - 4,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.White,
                },

            };
            return keypad;
        }

        public static void AddBorderText(Border target, string contents)
        {
            Label? label = (Label?)target.Content;
            if (label != null)
            {
                label.Text = contents;
            }
        }

        private static void AddCallback(Border target, System.EventHandler<Microsoft.Maui.Controls.TappedEventArgs> callback)
        {
            TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
            tapGestureRecognizer.Tapped += callback;
            target.GestureRecognizers.Add(tapGestureRecognizer);
        }

        public static void CreateAndAddKeypad(HorizontalStackLayout layout, string btnText, int width, int height, System.EventHandler<Microsoft.Maui.Controls.TappedEventArgs> callback)
        {
            Border newKeypad = CreateKeypad(width, height);
            AddBorderText(newKeypad, btnText);
            AddCallback(newKeypad, callback);
            layout.Add(newKeypad);
        }

        public static void CreateAndAddGridCell(Grid GridLayout, int row, int col)
        {
            var dimension = DisplayWidth / 7;
            dimension = Math.Clamp(dimension, 40, 90); // Ensure a reasonable size
            var color = Application.Current?.Resources["BorderAccent"] as Color;
            Border label = new()
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 2,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(2),
                },
                Content = new Label
                {
                    FontSize = 26,
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = dimension,
                    WidthRequest = dimension,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    TextColor = Colors.White
                },
            };

            GridLayout.Add(label, row, col);
        }

        public static void ResetGrid(Grid GridLayout)
        {
            foreach (var child in GridLayout.Children)
            {
                if (child is Border border)
                {
                    Label? label = (Label?)border.Content;
                    if (label != null)
                    {
                        label.Text = "";
                        border.BackgroundColor = Colors.Transparent;
                        border.RotationX = 0; 

                    }
                }
            }
        }

        private static Border? GetKeypad(Grid keyboard, char letter)
        {
            foreach (HorizontalStackLayout child in keyboard.Children)
            {
                foreach (Border keypad in child.Children)
                {
                    Label? label = (Label?)keypad.Content;
                    if (label != null && label.Text == letter.ToString().ToUpper())
                    {
                        return keypad;
                    }
                }
            }
            return null;
        }

        public static void SetKeypadColor(Grid keyboard, char letter, Color color)
        {
            Border? keypad = GetKeypad(keyboard, letter);
            var pale_green = Application.Current?.Resources["PaleGreen"] as Color;
            var pale_yellow = Application.Current?.Resources["PaleYellow"] as Color;
            var light_grey = Application.Current?.Resources["LightGrey"] as Color;
            var dark_grey = Application.Current?.Resources["DarkGrey"] as Color;
            if (keypad != null)
            {
                if (keypad.BackgroundColor == pale_green)
                    return;
                else if (keypad.BackgroundColor == pale_yellow && color == pale_green)
                    keypad.BackgroundColor = color;
                else if (keypad.BackgroundColor == light_grey || keypad.BackgroundColor == dark_grey)
                    keypad.BackgroundColor = color;
            }
        }

        public static void SetKeypadLetterColors(Grid keyboard, string letters, List<Color> colors)
        {
            for (int i = 0; i < letters.Length; i++)
            {
                char letter = letters[i];
                if (i < colors.Count)
                {
                    SetKeypadColor(keyboard, letter, colors[i]);
                }
            }
        }
        public static async Task SetGridRowColor(Grid gridLayout, int row, List<Color> colors)
        {
            for (int col = 0; col < colors.Count; col++)
            {
                Border? cell = GetGridElement(gridLayout, row, col);
                if (cell != null)
                {
                    cell.BackgroundColor = colors[col];
                    await AnimateGridRow(gridLayout, row, col);
                }
            }
        }
        public static async Task AnimateGridRow(Grid gridLayout, int row, int col)
        {
            Border? cell = GetGridElement(gridLayout, row, col);
            Label? label = (Label?)cell?.Content;
            string tmp = label?.Text ?? string.Empty;
            if (cell != null)
            {
                await cell.RotateXTo(cell.RotationX + 360, 400, Easing.SinInOut);
            }          
        }

    }
}
