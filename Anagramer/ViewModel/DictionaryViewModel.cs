using Anagramer.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anagramer.ViewModel
{
    public class DictionaryViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private Trie _dictionary;
        private readonly object _dictionaryLock = new object();
        
        public bool DictionaryReady
        {
            get
            {
                return _dictionaryReady;
            }
            private set
            {
                if(value != _dictionaryReady)
                {
                    _dictionaryReady = value;
                    PropertyChanged(this, DICTIONARY_READY_CHANGED_ARGS);
                }
            }
        }
        private bool _dictionaryReady;
        private static readonly PropertyChangedEventArgs DICTIONARY_READY_CHANGED_ARGS = new PropertyChangedEventArgs(nameof(DictionaryReady));

        public DictionaryViewModel()
        {
            _dictionaryReady = false;
        }
     
        public bool Contains(string word)
        {
            return _dictionary.Contains(word);
        }
    }
}
