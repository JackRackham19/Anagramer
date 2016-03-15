using Anagramer.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

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
        private CancellationTokenSource currentExecution;
        private readonly object currentExecutionLock = new object();

        public string Status
        {
            get
            {
                return _status;
            }
            private set
            {
                if(value != _status)
                {
                    _status = value;
                    PropertyChanged(this, STATUS_CHANGED_ARGS);
                }
            }
        }
        private string _status;
        private static readonly PropertyChangedEventArgs STATUS_CHANGED_ARGS = new PropertyChangedEventArgs(nameof(Status));

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
            _maxWords = 4;
            _subject = string.Empty;
            
            Task.Run(() =>
            {
                var defaultDictionary = "dictionary.txt";
                loadedDictionary = LoadDictionary(defaultDictionary);
                Dictionary = defaultDictionary;
            });

            PropertyChanged += AnagramViewModel_PropertyChanged;
        }

        private void AnagramViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(sender != this)
            {
                throw new InvalidOperationException("This should only ever be responding to itself");
            }
            switch(e.PropertyName)
            {
                case nameof(MaxWords):
                case nameof(Subject):
                    lock (currentExecutionLock)
                    {
                        if (null != currentExecution && !currentExecution.IsCancellationRequested)
                        {
                            currentExecution.Cancel();
                            Task.Delay(TimeSpan.FromMilliseconds(1000));
                        }
                        currentExecution = new CancellationTokenSource();
                        Task.Run(() => CalculateAnagrams(currentExecution.Token));
                    }
                    break;
                case nameof(Dictionary):
                    Status = "Loading Dictionary";
                    Task.Run(() => {
                        loadedDictionary = LoadDictionary(Dictionary);
                        CalculateAnagrams();
                    });
                    break;
                default:
                    break;
            }
        }

        public void CalculateAnagrams(CancellationToken cancelToken = default(CancellationToken))
        {
            Application.Current.Dispatcher.Invoke(() => { Status = "Anagraming..."; });
            var cleaned = Subject.RemoveWhitespace().ToLowerInvariant();
            var query = Anagram.Find(cleaned, loadedDictionary, MaxWords);
            Application.Current.Dispatcher.Invoke(() => Anagrams.Clear());

            foreach (var result in query)
            {
                if (!cancelToken.IsCancellationRequested)
                {
                    Application.Current.Dispatcher.Invoke(() => Anagrams.Add(result), DispatcherPriority.Background, cancelToken);
                }
            }
            Application.Current.Dispatcher.Invoke(() => { Status = "Done."; });
        }

        public static async Task<Trie> LoadDictionaryAsync(string path, CancellationToken cancelToken = default(CancellationToken))
        {
            return await Trie.ReadTrieAsync(path, cancelToken);
        }

        public static Trie LoadDictionary(string path)
        {
            return LoadDictionaryAsync(path).Result;
        }
    }
}
