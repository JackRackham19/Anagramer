using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagramer.Utilities
{
    public static class Anagram
    {
        public static IEnumerable<string> Find(string source, Trie dictionary, uint maxWords = uint.MaxValue)
        {
            return FindHelper(string.Empty, source, dictionary.Root, dictionary.Root, maxWords, 1);
        }

        private static IEnumerable<string> FindHelper(string current, string source, Trie.Node currentNode, Trie.Node root, uint maxWords, uint currentWords)
        {
            var cleaned = source.ToLowerInvariant();
            var distinctLetters = cleaned.Distinct();  
                        
            foreach (var letter in distinctLetters)
            {
                var firstIndex = cleaned.IndexOf(letter);
                if(!currentNode.Children.ContainsKey(letter))
                {
                    continue;
                }
                var removed = cleaned.Remove(firstIndex, 1);                
                var subResults = FindHelper(current + letter, removed, currentNode.Children[letter], root, maxWords, currentWords);
                foreach(var result in subResults)
                {
                    yield return result;
                }
            }

            if(currentNode.IsWord)
            {
                if(source == string.Empty)
                {
                    yield return current;
                }
                else if(currentWords < maxWords)
                {
                    var subResults = FindHelper(current + ' ', cleaned, root, root, maxWords, currentWords + 1);
                    foreach (var result in subResults)
                    {
                        yield return result;
                    }
                }
            }
        }
    }
}
