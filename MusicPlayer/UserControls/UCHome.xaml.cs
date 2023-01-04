using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Interaction logic for UCHome.xaml
    /// </summary>
    public partial class UCHome : UserControl
    {
        public static StreamReader sr;
        public static ObservableCollection<SONG> listFeaturedSong = new ObservableCollection<SONG>();
        public static ObservableCollection<SONG> listNewSong = new ObservableCollection<SONG>();
        public static ObservableCollection<SONG> listSong = new ObservableCollection<SONG>(DataProvider.Ins.DB.SONGs);
        public static int init = 0;


        public UCHome()
        {
            InitializeComponent();
            if (init == 0)
            {
                loadFeaturedSong();
                loadNewSong();
                UCPlayMusic.SelectedSong = listFeaturedSong.First();
                init++;
            }
            lbNewSongs.ItemsSource = listNewSong;
            lbFeaturedSongs.ItemsSource = listFeaturedSong;
        }

        public void loadNewSong()
        {
            List<NEWSONG> newSong = new List<NEWSONG>(DataProvider.Ins.DB.NEWSONGs);
            foreach (NEWSONG n in newSong)
            {
                foreach (SONG s in listSong)
                {
                    if (s.SONGID == n.SONGID)
                    {
                        listNewSong.Add(s);
                    }
                }
            }
        }

        public void loadFeaturedSong()
        {
            List<FEATUREDSONG> featuredSong = new List<FEATUREDSONG>(DataProvider.Ins.DB.FEATUREDSONGs);
            foreach(FEATUREDSONG f in featuredSong)
            {
                foreach (SONG s in listSong)
                {
                    if(s.SONGID == f.SONGID)
                    {
                        listFeaturedSong.Add(s);
                    }
                }
            }
        }

        private void lbFeaturedSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addListLastestSong(sender);
            UCPlayMusic.CurrentList = sender as ListBox;
        }

        public static void addListLastestSong(object sender)
        {
            ListBox ls = sender as ListBox;
            SONG song = ls.SelectedItem as SONG;
            UCLibrary.listLastestSong.Insert(0, song);
            if (UCLibrary.listLastestSong.Count() > 5)
            {
                UCLibrary.listLastestSong.RemoveAt(5);
            }
            UCPlayMusic.SelectedSong = song;

            if (ls.SelectedIndex + 1 < ls.Items.Count)
                UCPlayMusic.NextSong = ls.Items[ls.SelectedIndex + 1] as SONG;
            else
                UCPlayMusic.NextSong = null;

            if (ls.SelectedIndex - 1 >= 0)
                UCPlayMusic.PrevSong = ls.Items[ls.SelectedIndex - 1] as SONG;
            else
                UCPlayMusic.PrevSong = null;
        }

        private void lbNewSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addListLastestSong(sender);
            UCPlayMusic.CurrentList = sender as ListBox;
        }
    }
}
