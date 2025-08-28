using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MPWordleClient
{
    public static class WordManager
    {
        private static List<string> currentWords = [];
        public static string CurrentWord = string.Empty;
        private static int index = 0;

        public static void LoadWords(IEnumerable<string> words)
        {
            index = 0;
            CurrentWord = string.Empty;
            currentWords.Clear();
            foreach (var word in words)
                currentWords.Add(word);
        }
        public static string GetNewWord()
        {
            Random rand = new();
            if (index == currentWords.Count) return string.Empty;
            CurrentWord = currentWords[index].ToUpperInvariant();
            index++;
            return CurrentWord;
        }

        public static bool IsWordValid(string word)
        {
            return true;
        }

       

        public static WordResult GetResults(string userWord)
        {
            List<Color> colors = [];
            List<char> compWord = [];
            foreach(var letter in CurrentWord)
                compWord.Add(letter);
            bool isCorrect = userWord.Equals(CurrentWord, StringComparison.OrdinalIgnoreCase);
            for (int i = 0; i < userWord.Length; i++)
            {
                char letter = userWord[i].ToString().ToUpperInvariant()[0];
                if (compWord[i] == letter)
                {
                    compWord[i] = '#';
                    colors.Add(Colors.Green);
                }
                
                else
                    colors.Add((Color)Application.Current.Resources["DarkGrey"]);
            }
            for (int i = 0; i < userWord.Length; i++)
            {
                char letter = userWord[i].ToString().ToUpperInvariant()[0];
                if (compWord.Contains(letter))
                {
                    var idx = compWord.FindIndex(lt => lt == letter);
                    if(idx != i)
                    {
                        compWord[idx] = '#';
                        colors[i] = Colors.Yellow;
                    }
                }
            }
            
            return new(isCorrect, colors);
        }

    }
}
