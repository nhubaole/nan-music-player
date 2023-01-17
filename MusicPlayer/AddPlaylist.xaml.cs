using MusicPlayer.Model;
using MusicPlayer.ViewModel;
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
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for AddPlaylist.xaml
    /// </summary>
    public partial class AddPlaylist : Window
    {
        public AddPlaylist()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(tbPlaylistName.Text == "")
            {
                MessageBox.Show("Vui lòng nhập tên Playlist nha");
                return;
            }
            else
            {
                var pl = new PLAYLIST()
                {
                    PLAYLISTNAME = tbPlaylistName.Text
                };
                LoginViewModel.currUser.PLAYLISTs.Add(pl);
                DataProvider.Ins.DB.SaveChanges();
                MessageBox.Show("Tạo Playlist thành công!", "Thông báo", MessageBoxButton.OK);
                this.Close();
            }
        }
    }
}
