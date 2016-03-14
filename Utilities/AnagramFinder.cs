using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagramer.Utilities
{
    public static class Anagram
    {
        public static IEnumerable<string> Find(string source, Trie dictionary)
        {
            return FindHelper(string.Empty, source, dictionary.Root, dictionary.Root);
        }

        private static IEnumerable<string> FindHelper(string current, string source, Trie.Node currentNode, Trie.Node root)
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
                var subResults = FindHelper(current + letter, removed, currentNode.Children[letter], root);
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
                var subResults = FindHelper(string.Empty, cleaned, root, root);
                foreach(var result in subResults)
                {
                    yield return $"{current} {result}";
                }
            }
        }
    }
}
