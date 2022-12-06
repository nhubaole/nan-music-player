using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


namespace MusicPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCLibrary.xaml
    /// </summary>
    public partial class UCLibrary : UserControl
    {
        public static ObservableCollection<SONG> listLastestSong = new ObservableCollection<SONG>();
        public UCLibrary()
        {
            InitializeComponent();
            lbLastestSongs.ItemsSource = listLastestSong;
        }

        private void lbLastestSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UCHome.addListLastestSong(sender);
        }
    }
}
