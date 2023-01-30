using MusicPlayer.Model;
using MusicPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// Interaction logic for UCLibrary.xaml
    /// </summary>
    public partial class UCLibrary : UserControl
    {
        public static int init = -1;
        public static BackgroundWorker bw2;
        public static TimeSpan timer;
        public static ObservableCollection<SONG> listLastestSong;
        public static ObservableCollection<SONG> listLikedSong = new ObservableCollection<SONG>();
        public static ObservableCollection<PLAYLIST> listPlaylists;
        public static ObservableCollection<UPLOADSONG> listUploadSong;
        public static ObservableCollection<UPLOADSONG> listOwnUpload;
        public static ObservableCollection<string> listTimer = new ObservableCollection<string>() { "15 phút" , "30 phút", "1 giờ", "2 giờ", "Tắt hẹn giờ" };
        public static ComboBox cbTimer = Application.Current.TryFindResource("cbTimer") as ComboBox;
        public static TextBlock txtTimer = Application.Current.TryFindResource("txtTimer") as TextBlock;
        public static PLAYLIST playlistDeleted;
        public static Button btnNow;
        public UCLibrary()
        {
            InitializeComponent();
            if(init == -1)
            {
                bw2 = new BackgroundWorker();
                bw2.WorkerSupportsCancellation = true;
                bw2.WorkerReportsProgress = true;
                bw2.ProgressChanged += Bw2_ProgressChanged;
                bw2.DoWork += Bw2_DoWork;
                bw2.RunWorkerAsync();
                init++;
            }
            UpdatePlaylist();

            listLastestSong = new ObservableCollection<SONG>();
            foreach (LASTEST l in LoginViewModel.currUser.LASTESTs.OrderByDescending(l => l.SEQ))
            {
                listLastestSong.Add(l.SONG);
            }
            lbLastestSongs.ItemsSource = listLastestSong;

            UpdateLikedSong();
            lbLikedSongs.ItemsSource = listLikedSong;
            
            UpdateUploadSong();
            cbTimer.ItemsSource = listTimer;
            cbTimer.SelectionChanged += CbTimer_SelectionChanged;
        }

        public void UpdateUploadSong()
        {
            listUploadSong = new ObservableCollection<UPLOADSONG>(DataProvider.Ins.DB.UPLOADSONGs);
            listOwnUpload = new ObservableCollection<UPLOADSONG>();
            foreach (UPLOADSONG u in listUploadSong)
            {
                if(u.USERS.ToList().Count() != 0 && u.USERS.ToList()[0].USERNAME == LoginViewModel.currUser.USERNAME)
                {
                    listOwnUpload.Add(u);
                }
            }
            lbUploadSongs.ItemsSource = listOwnUpload;
        }

        public void UpdatePlaylist()
        {
            listPlaylists = new ObservableCollection<PLAYLIST>(LoginViewModel.currUser.PLAYLISTs);
            lbPlaylists.ItemsSource = listPlaylists;
        }

        private void CbTimer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(init == 0)
            {
                init++;
            }
            if(cbTimer.SelectedItem != null)
            {
                if(cbTimer.SelectedItem == cbTimer.Items[0])
                {
                    timer = new TimeSpan(0, 15, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[1])
                {
                    timer = new TimeSpan(0, 30, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[2])
                {
                    timer = new TimeSpan(1, 0, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[3])
                {
                    timer = new TimeSpan(2, 0, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[4])
                {
                    init = 0;
                    txtTimer.Text = cbTimer.SelectedItem.ToString();
                }

            }
        }

        private void Bw2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(1000);
                bw2.ReportProgress(1);
            }
        }

        private void Bw2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (init != 0 && init != -1)
            {
                if(cbTimer.Text == "00:00:00")
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    timer = timer.Subtract(new TimeSpan(0, 0, 1));
                    this.Dispatcher.Invoke(() =>
                    {
                        cbTimer.Text = timer.ToString();
                        txtTimer.Text = timer.ToString();
                    });
                }
            }
        }

        private void lbUploadSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox ls = sender as ListBox;
            UPLOADSONG select = ls.SelectedItem as UPLOADSONG;
            if(select == null)
            {
                return;
            }
            else
            {
                SONG temp = new SONG() { SONGNAME = select.SONGNAME, SINGERNAME = select.SINGERNAME, IMAGEURL = select.IMAGEPATH, SAVEPATH = select.SAVEPATH };
                UCPlayMusic.SelectedSong = temp;
                if (ls.SelectedIndex + 1 < ls.Items.Count)
                    UCPlayMusic.NextSong = ls.Items[ls.SelectedIndex + 1] as SONG;
                else
                    UCPlayMusic.NextSong = null;

                if (ls.SelectedIndex - 1 >= 0)
                    UCPlayMusic.PrevSong = ls.Items[ls.SelectedIndex - 1] as SONG;
                else
                    UCPlayMusic.PrevSong = null;

                UCPlayMusic.CurrentList = ls;
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            Upload upload = new Upload();
            upload.ShowDialog();
            UpdateUploadSong();
        }

        private void btnLike_Click(object sender, RoutedEventArgs e)
        {
            LikeSong(sender);
        }

        private void btnLike2_Click(object sender, RoutedEventArgs e)
        {
            LikeSong(sender);
        }

        public static void UpdateLikedSong()
        {
            foreach(SONG s in UCHome.listSong)
            {
                s.LIKED = false;
            }
            listLikedSong.Clear();
            foreach(SONG s in LoginViewModel.currUser.SONGs)
            {
                s.LIKED = true;
                listLikedSong.Add(s);
            }
        }

        public static void LikeSong(object sender)
        {
            ToggleButton btn = (ToggleButton)sender;
            SONG s = btn.DataContext as SONG;
            if (btn.IsChecked == true)
            {
                LoginViewModel.currUser.SONGs.Add(s);
                DataProvider.Ins.DB.SaveChanges();
                UpdateLikedSong();
            }
            else
            {
                LoginViewModel.currUser.SONGs.Remove(s);
                DataProvider.Ins.DB.SaveChanges();
                UpdateLikedSong();
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn xóa bài hát này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Button btn = (Button)sender;
                UPLOADSONG s = btn.DataContext as UPLOADSONG;
                LoginViewModel.currUser.UPLOADSONGs.Remove(s);
                DataProvider.Ins.DB.SaveChanges();
                UpdateUploadSong();
            }
            else
            {
                return;
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Upload.update = 1;

            Button btn = (Button)sender;
            UPLOADSONG s = btn.DataContext as UPLOADSONG;
            Upload.updateSong = s;
            Upload upload = new Upload();

            upload.ShowDialog();
            UpdateUploadSong();
            Upload.update = 0;
        }

        private void btnAddPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylist add = new AddPlaylist();
            add.ShowDialog();
            UpdatePlaylist();
        }

        private void btnDelPlaylist_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa playlist này?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Button btn = (Button)sender;
                PLAYLIST pl = btn.DataContext as PLAYLIST;
                LoginViewModel.currUser.PLAYLISTs.Remove(pl);
                DataProvider.Ins.DB.PLAYLISTs.Remove(pl);
                DataProvider.Ins.DB.SaveChanges();
                UpdatePlaylist();
            }
            else
            {
                return;
            }
        }

        private void btnRemove_Click(object sender, RoutedEventArgs e)
        {
            if(MessageBox.Show("Bạn có chắc chắn muốn xóa bài hát khỏi playlist?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Button btn = sender as Button;
                SONG s = btn.DataContext as SONG;

                playlistDeleted.SONGs.Remove(s);
                DataProvider.Ins.DB.SaveChanges();
                UpdatePlaylist();
                MessageBox.Show("Đã xóa bài hát thành công");
            }
        }


        private void ListBox_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            playlistDeleted = listBox.DataContext as PLAYLIST;
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
            UpdatePlaylist();
        }

        private void btnEditPlaylist_Click(object sender, RoutedEventArgs e)
        {
            AddPlaylist.edit = 1;

            Button btn = (Button)sender;
            PLAYLIST pl = btn.DataContext as PLAYLIST;
            AddPlaylist.playlistEditing = pl;
            AddPlaylist add = new AddPlaylist();

            add.ShowDialog();
            UpdatePlaylist();
            AddPlaylist.edit = 0;
        }
    }
}
