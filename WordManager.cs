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
        public WordManager(string wordsFilePath)
        {
            allWords = new Trie.Trie();
            wordList = LoadWords(wordsFilePath).Result;
            foreach (string word in wordList)
            {
                allWords.Insert(word.ToLowerInvariant());
            }
        }

        public string GenerateRandomWord()
        {
            Random rand = new();
            return wordList[rand.Next(0, wordList.Length - 1)];
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

    }
}
