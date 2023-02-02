using MusicPlayer.Model;
using MusicPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for UCSearch.xaml
    /// </summary>
    public partial class UCSearch : UserControl
    {
        public static ObservableCollection<SONG> listSong;
        public static ObservableCollection<SONG> searchResult = new ObservableCollection<SONG>();
        public static ObservableCollection<string> listChoices = new ObservableCollection<string>() { "Tên Bài Hát", "Tên Ca Sĩ" };
        public static Button btnNow;

        public UCSearch()
        {
            InitializeComponent();
            cbSearchType.ItemsSource = listChoices;
            cbSearchType.SelectedItem = cbSearchType.Items[0];
            if (LoginViewModel.currUser != null)
            {
                UCLibrary.UpdateLikedSong();
            }
        }

        private void lbSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox ls = sender as ListBox;
            if (ls.SelectedItem == null)
            {
                return;
            }
            UCHome.addListLastestSong(sender);
            UCPlayMusic.CurrentList = ls;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            searchSongs();
        }

        private void cbSearchType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            searchSongs();
        }

        void searchSongs()
        {
            if(listSong != null)
            {
                searchResult.Clear();
                if (cbSearchType.SelectedItem == cbSearchType.Items[0])
                {
                    foreach (SONG n in listSong)
                    {
                        if (n.SONGNAME.ToUpper().Contains(txtSearch.Text.ToUpper()))
                        {
                            searchResult.Add(n);
                        }
                    }
                }
                else if (cbSearchType.SelectedItem == cbSearchType.Items[1])
                {
                    foreach (SONG n in listSong)
                    {
                        if (n.SINGERNAME.ToUpper().Contains(txtSearch.Text.ToUpper()))
                        {
                            searchResult.Add(n);
                        }
                    }
                }
                lbSongs.ItemsSource = searchResult;
            }
        }

        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            UCLibrary.LikeSong(sender);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            Button btnAdd = (Button)sender;
            ContextMenu contextMenu = btnAdd.ContextMenu;
            contextMenu.ItemsSource = UCLibrary.listPlaylists;
            contextMenu.PlacementTarget = btnAdd;
            contextMenu.Placement = PlacementMode.MousePoint;
            contextMenu.IsOpen = true;
            btnNow = btnAdd;
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = sender as MenuItem;
            PLAYLIST pl = menu.DataContext as PLAYLIST;
            SONG s = btnNow.DataContext as SONG;
            pl.SONGs.Add(s);
            DataProvider.Ins.DB.SaveChanges();
            MessageBox.Show("Thêm bài hát vào playlist thành công");
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TabControl tab = sender as TabControl;
            if(tab.SelectedIndex == 0)
                listSong = new ObservableCollection<SONG>(DataProvider.Ins.DB.SONGs.Where(s => s.GENRE == "Việt Nam"));
            else if (tab.SelectedIndex == 1)
                listSong = new ObservableCollection<SONG>(DataProvider.Ins.DB.SONGs.Where(s => s.GENRE == "Âu Mỹ"));
            else if (tab.SelectedIndex == 2)
                listSong = new ObservableCollection<SONG>(DataProvider.Ins.DB.SONGs.Where(s => s.GENRE == "Hàn Quốc"));
            lbSongs.ItemsSource = listSong;
        }
    }
}
