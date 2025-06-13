using Microsoft.Maui;
using Microsoft.Maui.Controls.Shapes;

namespace MPWordleClient
{
    public class WordleUI
    {
        public static Border? GetGridElement(Grid grid, int row, int col)
        {
            return (Border?)grid.Children
                .FirstOrDefault(child => grid.GetRow(child) == row && grid.GetColumn(child) == col);
        }

        private static Border CreateKeypad(int width, int height)
        {
            Border keypad = new()
            {
                BackgroundColor = Color.FromRgb(80, 80, 80),
                StrokeThickness = 0,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(2)
                },
                Content = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 20,
                    HeightRequest = height,
                    WidthRequest = width,
                    TextColor = Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
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
            Border label = new()
            {
                Stroke = new SolidColorBrush(Color.FromRgb(70, 70, 70)),
                StrokeThickness = 2,
                StrokeShape = new RoundRectangle
                {
                    CornerRadius = new CornerRadius(2),
                },
                Content = new Label
                {
                    FontSize = 26,
                    FontAttributes = FontAttributes.Bold,
                    HeightRequest = 70,
                    WidthRequest = 60,
                    TextColor = Colors.White,
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
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
            if (keypad != null)
            {
                keypad.BackgroundColor = color;
            }
        }

        public static void SetGridRowColor(Grid gridLayout, int row, List<Color> colors)
        {
            for (int col = 0; col < colors.Count; col++)
            {
                Border? cell = GetGridElement(gridLayout, row, col);
                if (cell != null)
                {
                    cell.BackgroundColor = colors[col];
                }
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
    }
}
