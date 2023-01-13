using Microsoft.Win32;
using MusicPlayer.Model;
using MusicPlayer.UserControls;
using MusicPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for Upload.xaml
    /// </summary>
    public partial class Upload : Window
    {
        public static int update = 0;
        public static UPLOADSONG updateSong;
        public Upload()
        {
            InitializeComponent();
            if(update == 1)
            {
                txtTitle.Text = "CHỈNH SỬA BÀI HÁT";
                txtBtnUp.Text = "CẬP NHẬT";
                tbSongName.Text = updateSong.SONGNAME;
                tbSingerName.Text = updateSong.SINGERNAME;
                txtPath.Text = updateSong.SAVEPATH;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                try
                {
                    bitmapImage.UriSource = new Uri(updateSong.IMAGEPATH);
                    btnImage.ToolTip = updateSong.IMAGEPATH;
                }
                catch
                {
                    bitmapImage.UriSource = new Uri("../" + updateSong.IMAGEPATH, UriKind.RelativeOrAbsolute);
                    btnImage.ToolTip = "";
                }
                bitmapImage.EndInit();
                img.ImageSource = bitmapImage;
            }
        }

        private void btnChooseFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Media Files(*.avi; *.mpeg; *.wav; *.midi; *.mp4; *.mp3)|*.avi; *.mpeg; *.wav; *.midi; *.mp4; *.mp3";
                if (openFileDialog.ShowDialog() == true)
                {
                    string path = openFileDialog.FileName;
                    txtPath.Text = path;
                }
            }
            catch
            {
                MessageBox.Show("Đã xảy ra lỗi");
            }
        }

        private void btnImage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (openFileDialog.ShowDialog() == true)
                {
                    string path = openFileDialog.FileName;
                    var bitmapImage = new BitmapImage();
                    bitmapImage.BeginInit();
                    bitmapImage.UriSource = new Uri(path);
                    bitmapImage.EndInit();
                    img.ImageSource = bitmapImage;
                    btnImage.ToolTip = path;
                }
            }
            catch
            {
                MessageBox.Show("Đã xảy ra lỗi");
            }
        }

        private void btnUpload_Click(object sender, RoutedEventArgs e)
        {
            string songName = tbSongName.Text;
            string singerName = string.IsNullOrEmpty(tbSingerName.Text) ? "Unknown" : tbSingerName.Text;
            string savePath = txtPath.Text;
            string imagePath = btnImage.ToolTip.ToString() == "" ? "../Resource/Images/ImgUp.png" : btnImage.ToolTip.ToString();
            if (string.IsNullOrEmpty(songName) || string.IsNullOrEmpty(savePath))
            {
                MessageBox.Show("Vui lòng nhập tên bài hát và chọn file bài hát đầy đủ");
                return;
            }
            else
            {
                var song = new UPLOADSONG()
                {
                    SONGNAME = songName,
                    SINGERNAME = singerName,
                    IMAGEPATH = imagePath,
                    SAVEPATH = savePath
                };
                if (update == 1)
                {
                    LoginViewModel.currUser.UPLOADSONGs.Remove(updateSong);
                    LoginViewModel.currUser.UPLOADSONGs.Add(song);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Cập nhật bài hát thành công!", "Thông báo", MessageBoxButton.OK);
                    this.Close();
                }
                else
                {
                    song.USERS.Add(LoginViewModel.currUser);
                    DataProvider.Ins.DB.UPLOADSONGs.Add(song);
                    DataProvider.Ins.DB.SaveChanges();
                    MessageBox.Show("Tải bài hát lên thành công!", "Thông báo", MessageBoxButton.OK);
                    this.Close();
                }
            }
        }
    }
}
