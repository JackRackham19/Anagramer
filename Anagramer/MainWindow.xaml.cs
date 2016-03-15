using Anagramer.ViewModel;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Anagramer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new AnagramViewModel(); ;
        }
        
        private void dictionaryBrowse_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as AnagramViewModel;
            var dialog = new OpenFileDialog();
            
            if(true == dialog.ShowDialog())
            {
                vm.Dictionary = dialog.FileName;
            }
        }

        private void submit_Click(object sender, RoutedEventArgs e)
        {
            var vm = DataContext as AnagramViewModel;
            Task.Run(() => vm.CalculateAnagrams());            
        }
    }
}
