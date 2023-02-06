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
        public static int edit = 0;
        public static PLAYLIST playlistEditing;
        public AddPlaylist()
        {
            InitializeComponent();
            if (edit == 1)
            {
                txtTitle.Text = "CHỈNH SỬA PLAYLIST";
                txtBtn.Text = "CHỈNH SỬA";
                tbPlaylistName.Text = playlistEditing.PLAYLISTNAME;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if(tbPlaylistName.Text == "")
            {
                CustomMessageBox.Show("Vui lòng nhập tên Playlist nha", MessageBoxImage.Information);
                return;
            }
            else
            {
                
                if (edit == 1)
                {
                    playlistEditing.PLAYLISTNAME = tbPlaylistName.Text;
                    DataProvider.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Chỉnh sửa Playlist thành công!", MessageBoxImage.None);
                    this.Close();
                }
                else
                {
                    var pl = new PLAYLIST()
                    {
                        PLAYLISTNAME = tbPlaylistName.Text
                    };
                    LoginViewModel.currUser.PLAYLISTs.Add(pl);
                    DataProvider.Ins.DB.SaveChanges();
                    CustomMessageBox.Show("Tạo Playlist thành công!", MessageBoxImage.None);
                    this.Close();
                }
            }
        }
    }
}
