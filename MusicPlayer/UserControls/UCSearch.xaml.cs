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
    /// Interaction logic for UCSearch.xaml
    /// </summary>
    public partial class UCSearch : UserControl
    {
        public static ObservableCollection<SONG> listSong = new ObservableCollection<SONG>(DataProvider.Ins.DB.SONGs);
        public static ObservableCollection<SONG> searchResult = new ObservableCollection<SONG>();
        public static ObservableCollection<string> listChoices = new ObservableCollection<string>() { "Tên Bài Hát", "Tên Ca Sĩ" };
        public UCSearch()
        {
            InitializeComponent();
            lbSongs.ItemsSource = listSong;
            cbSearchType.ItemsSource = listChoices;
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
            else
            {
                MessageBox.Show("Bạn muốn tìm kiếm theo Tên Bài Hát hay Tên Ca Sĩ? Vui lòng lựa chọn");
                txtSearch.Clear();
                return;
            }
            lbSongs.ItemsSource = searchResult;
        }
    }
}
