using Anagramer.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dictionary = Trie.ReadTrieAsync(@"C:\Users\Martin\OneDrive\Documents\Programming\12dicts-5.0\3esl.txt", s => s.ToLowerInvariant()).Result;
            var results = Anagram.Find(args[0], dictionary);
            foreach(var entry in results)
            {
                Console.WriteLine(entry);
            }
        }        
    }
}
