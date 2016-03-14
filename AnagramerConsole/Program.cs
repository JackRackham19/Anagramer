using Anagramer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AnagramerConsole
{
    class Program
    {
        private static readonly Regex dictionaryArgPattern = new Regex("^[/-]d(?:ictionary)?$", RegexOptions.Compiled);
        private static readonly Regex maxWordsArgPattern = new Regex("^[/-]w(?:ords)?$", RegexOptions.Compiled);
        
        static void Main(string[] args)
        {
            string dictionaryPath = "dictionary.txt";
            uint maxWords = 4;
            
            for(int i = 0; i < args.Length; i++)
            {
                if(dictionaryArgPattern.IsMatch(args[i]))
                {
                    dictionaryPath = args[++i];
                }
                if(maxWordsArgPattern.IsMatch(args[i]))
                {
                    maxWords = uint.Parse(args[++i]);
                }
            }

            var dictionary = Trie.ReadTrieAsync(dictionaryPath, s => s.ToLowerInvariant()).Result;
            var results = Anagram.Find(args[0], dictionary, maxWords);
            foreach(var entry in results)
            {
                Console.WriteLine(entry);
            }
        }        
    }
}
