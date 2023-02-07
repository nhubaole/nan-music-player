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
        public static int uploadVideo = 0;
        public static int updateVid = 0;
        public static UPLOADSONG updateSong;
        public static UPLOADVIDEO updateVideo;
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
            if (uploadVideo == 1)
            {
                txtTitle.Text = "TẢI VIDEO LÊN";
            }
            if (updateVid == 1)
            {
                txtTitle.Text = "CHỈNH SỬA VIDEO";
                txtBtnUp.Text = "CẬP NHẬT";
                tbSongName.Text = updateVideo.VIDEONAME;
                tbSingerName.Text = updateVideo.SINGERNAME;
                txtPath.Text = updateVideo.SAVEPATH;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                try
                {
                    bitmapImage.UriSource = new Uri(updateVideo.IMAGEPATH);
                    btnImage.ToolTip = updateVideo.IMAGEPATH;
                }
                catch
                {
                    bitmapImage.UriSource = new Uri("../" + updateVideo.IMAGEPATH, UriKind.RelativeOrAbsolute);
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
                CustomMessageBox.Show("Không thể chọn file bài hát", MessageBoxImage.Error);
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
                CustomMessageBox.Show("Không thể chọn file hình ảnh", MessageBoxImage.Error);
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
                CustomMessageBox.Show("Vui lòng nhập tên bài hát và chọn file bài hát đầy đủ", MessageBoxImage.Information);
                return;
            }
            else
            {
                if(uploadVideo == 0 && updateVid == 0)
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
                        CustomMessageBox.Show("Cập nhật bài hát thành công!", MessageBoxImage.None);
                        this.Close();
                    }
                    else
                    {
                        song.USERS.Add(LoginViewModel.currUser);
                        DataProvider.Ins.DB.UPLOADSONGs.Add(song);
                        DataProvider.Ins.DB.SaveChanges();
                        CustomMessageBox.Show("Tải bài hát lên thành công!", MessageBoxImage.None);
                        this.Close();
                    }
                }
                else
                {
                    var video = new UPLOADVIDEO()
                    {
                        VIDEONAME = songName,
                        SINGERNAME = singerName,
                        IMAGEPATH = imagePath,
                        SAVEPATH = savePath
                    };

                    if (updateVid == 1)
                    {
                        LoginViewModel.currUser.UPLOADVIDEOs.Remove(updateVideo);
                        LoginViewModel.currUser.UPLOADVIDEOs.Add(video);
                        DataProvider.Ins.DB.SaveChanges();
                        CustomMessageBox.Show("Cập nhật video thành công!", MessageBoxImage.None);
                        this.Close();
                    }
                    else
                    {
                        video.USERS.Add(LoginViewModel.currUser);
                        DataProvider.Ins.DB.UPLOADVIDEOs.Add(video);
                        DataProvider.Ins.DB.SaveChanges();
                        CustomMessageBox.Show("Tải video lên thành công!", MessageBoxImage.None);
                        this.Close();
                    }
                    
                }
            }
        }
    }
}
