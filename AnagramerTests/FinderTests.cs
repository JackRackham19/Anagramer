using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Anagramer.Utilities;

namespace AnagramerTests
{
    [TestClass]
    public class FinderTests
    {
        public static Trie dictionary;

        static FinderTests()
        {
            dictionary = Trie.ReadTrieAsync(@"dictionary.txt", s => s.ToLowerInvariant()).Result;
        }

        [TestMethod]
        public void Dog()
        {
            Assert.IsNotNull(dictionary);
            var results = Anagram.Find("dog", dictionary);
            Assert.IsTrue(results.Any());
            Assert.IsTrue(results.Contains("god"));
        }

        [TestMethod]
        public void JessicaDrobot()
        {
            Assert.IsNotNull(dictionary);
            var results = Anagram.Find("JessicaDrobot", dictionary);
            Assert.IsTrue(results.Any());            
        }
    }
}
