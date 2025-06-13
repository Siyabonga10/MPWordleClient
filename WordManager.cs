using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MPWordleClient
{
    internal class WordManager
    {
        readonly Trie.Trie allWords;
        readonly string[] wordList;
        public string CurrentWord { get; set; }
        readonly Dictionary<char, int> letterCount;
        public WordManager(string wordsFilePath)
        {
            allWords = new Trie.Trie();
            wordList = LoadWords(wordsFilePath).Result;
            foreach (string word in wordList)
            {
                allWords.Insert(word.ToLowerInvariant());
            }
            CurrentWord = "";
            letterCount = [];
            DetermineLetterCount();
            GetNewWord();

        }

        private void DetermineLetterCount()
        {
            letterCount.Clear();
            foreach (char letter in CurrentWord)
            {
                if (letterCount.ContainsKey(letter))
                {
                    letterCount[letter]++;
                }
                else
                {
                    letterCount[letter] = 1;
                }
            }
        }

        public void GetNewWord()
        {
            Random rand = new();
            CurrentWord = wordList[rand.Next(0, wordList.Length - 1)].ToUpperInvariant();
            DetermineLetterCount();
        }

        public bool IsWordValid(string word)
        {
            return allWords.Search(word.ToLowerInvariant());
        }

        private static async Task<string[]> LoadWords(string wordsFilePath)
        {
            try
            {
                using var stream = await FileSystem.OpenAppPackageFileAsync(wordsFilePath);
                using var reader = new StreamReader(stream);
                if (reader.EndOfStream)  
                {
                    return Array.Empty<string>();
                }
                var content = reader.ReadToEnd();
                return content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading words from file: {ex.Message}");
                return Array.Empty<string>();
            }
        }

        public WordResult GetResults(string userWord)
        {
            List<Color> colors = [];
            bool isCorrect = userWord.Equals(CurrentWord, StringComparison.OrdinalIgnoreCase);
            for (int i = 0; i < userWord.Length; i++)
            {
                char letter = userWord[i].ToString().ToUpperInvariant()[0];
                if (CurrentWord[i] == letter)
                {
                    colors.Add(Colors.Green);
                    letterCount[letter]--;
                }
                else if (letterCount.ContainsKey(letter))
                {
                    if (letterCount[letter] > 0)
                    {
                        colors.Add(Colors.Yellow);
                        letterCount[letter]--;
                    }
                    else
                    {
                        colors.Add(Color.FromRgb(12, 12, 12));
                    }
                }
                else
                {
                    colors.Add(Color.FromRgb(12, 12, 12));
                }
            }
            DetermineLetterCount();
            return new(isCorrect, colors);
        }

    }
}
