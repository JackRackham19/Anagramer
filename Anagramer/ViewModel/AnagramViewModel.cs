using Anagramer.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Anagramer.ViewModel
{
    public class AnagramViewModel : INotifyPropertyChanged
    {        
        public event PropertyChangedEventHandler PropertyChanged;

        public string Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                if(value != _subject)
                {
                    _subject = value;
                    PropertyChanged(this, SUBJECT_CHANGED_ARGS);
                }
            }
        }
        private string _subject;
        private static readonly PropertyChangedEventArgs SUBJECT_CHANGED_ARGS = new PropertyChangedEventArgs(nameof(Subject));

        public string Dictionary
        {
            get
            {
                return _dictionary;
            }
            set
            {
                if(value != _dictionary)
                {
                    _dictionary = value;
                    loadedDictionary = Trie.ReadTrieAsync(_dictionary).Result;
                    PropertyChanged(this, DICTIONARY_CHANGED_ARGS);
                }
            }
        }
        private string _dictionary;
        private static readonly PropertyChangedEventArgs DICTIONARY_CHANGED_ARGS = new PropertyChangedEventArgs(nameof(Dictionary));

        private Trie loadedDictionary;

        public uint MaxWords
        {
            get
            {
                return _maxWords;
            }
            set
            {
                if(value != _maxWords)
                {
                    _maxWords = value;
                    PropertyChanged(this, MAX_WORDS_CHANGED_ARGS);
                }
            }
        }
        private uint _maxWords;
        private static readonly PropertyChangedEventArgs MAX_WORDS_CHANGED_ARGS = new PropertyChangedEventArgs(nameof(MaxWords));

        public ObservableCollection<string> Anagrams
        {
            get;
        }
        
        public AnagramViewModel()
        {
            Anagrams = new ObservableCollection<string>();
        }

        public void CalculateAnagrams(CancellationToken cancelToken = default(CancellationToken))
        {
            var query = Anagram.Find(Subject, loadedDictionary, MaxWords);
            Anagrams.Clear();
            foreach(var result in query)
            {
                if(!cancelToken.IsCancellationRequested)
                {
                    Anagrams.Add(result);
                }
            }
        }

        public static async Task<Trie> LoadDictionaryAsync(string path, CancellationToken cancelToken = default(CancellationToken))
        {
            return await Trie.ReadTrieAsync(path, cancelToken);
        }
    }
}
