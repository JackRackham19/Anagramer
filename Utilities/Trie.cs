using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anagramer.Utilities
{
    public class Trie
    {
        public uint Length
        {
            get;
            private set;
        }

        public Node Root
        {
            get;
        }

        public Trie()
        {
            Root = new Node();
            Length = 0u;
        }
        
        public static async Task<Trie> ReadTrieAsync(string filePath, CancellationToken cancelToken = default(CancellationToken))
        {
            using(var reader = new StreamReader(filePath))
            {
                return await ReadTrieAsync(reader, s => s, cancelToken);
            }
        }

        public static async Task<Trie> ReadTrieAsync(string filePath, Func<string, string> clean, CancellationToken cancelToken = default(CancellationToken))
        {
            using (var reader = new StreamReader(filePath))
            {
                return await ReadTrieAsync(reader, clean, cancelToken);
            }
        }

        public static async Task<Trie> ReadTrieAsync(StreamReader reader, CancellationToken cancelToken = default(CancellationToken))
        {
            return await ReadTrieAsync(reader, s => s, cancelToken);
        }

        public static async Task<Trie> ReadTrieAsync(StreamReader reader, Func<string, string> clean, CancellationToken cancelToken = default(CancellationToken))
        {
            var retval = new Trie();
            while(!reader.EndOfStream && !cancelToken.IsCancellationRequested)
            {
                var rawLine = await reader.ReadLineAsync();
                retval.Add(clean(rawLine));
            }
            return retval;
        }

        public bool Add(string word)
        {
            bool retval = AddHelper(Root, word, word.Length);
            if (retval)
            {
                Length++;
            }
            return retval;
        }

        public bool Contains(string word)
        {
            Node currentNode = Root;

            foreach (var letter in word)
            {
                var child = currentNode.Children[letter];                
                if(null == child)
                {
                    return false;
                }
                currentNode = child;
            }

            return currentNode.IsWord;
        }

        private static bool AddHelper(Node node, string word, int length)
        {

            if (0 == length)
            {
                bool retval = !node.IsWord;
                node.IsWord = true;
                return retval;
            }
            else
            {
                var firstLetter = word.First();
                if (!node.Children.ContainsKey(firstLetter))
                {
                    node.Children.Add(firstLetter, new Node() {Parent=node});
                }
                return AddHelper(node.Children[firstLetter], word.Substring(1), length - 1);
            }
        }

        public sealed class Node
        {
            public Node Parent
            {
                get;
                set;
            }

            public bool IsWord
            {
                get;
                set;
            }

            public Dictionary<char, Node> Children
            {
                get
                {
                    return _children;
                }
            }
            private readonly Dictionary<char, Node> _children = new Dictionary<char, Node>();
        }
    }
}
