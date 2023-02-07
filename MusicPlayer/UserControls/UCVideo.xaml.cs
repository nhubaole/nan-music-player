﻿using MusicPlayer.Model;
using MusicPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
    /// Interaction logic for UCVideo.xaml
    /// </summary>
    public partial class UCVideo : UserControl
    {
        public static ObservableCollection<VIDEO> listVid = new ObservableCollection<VIDEO>(DataProvider.Ins.DB.VIDEOs);
        public static ObservableCollection<UPLOADVIDEO> listVidUp;
        public static List<string> listGenre = new List<string>() { "Việt Nam", "Âu Mỹ", "Nhạc Hàn", "Nhạc Hoa", "Thiếu Nhi", "Nhạc Nhật", "Nhạc Thái" };

        public static int init = 0;
        public static int reset = 1;
        public static BackgroundWorker bw;
        private VIDEO selectedVideo;
        static VIDEO nextVideo;
        static VIDEO prevVideo;
        static VIDEO nowPlaying;
        static ListBox currentList;
        public UCVideo()
        {
            InitializeComponent();
            init = 0;
            cbGenre.ItemsSource = listGenre;
            cbGenre.SelectedItem = cbGenre.Items[0];
            if (reset == 1)
            {
                SelectedVideo = listVid.First();
                reset = 0;
            }
            else
            {
                SelectedVideo = nowPlaying;
                foreach(var item in cbGenre.Items)
                {
                    if(item.ToString() == SelectedVideo.GENRE)
                    {
                        cbGenre.SelectedItem = item;
                    }
                }
                
            }
            UpdateUploadVideo();
            slVolume.Maximum = 1;
            slVolume.Value = 0.5;
            UCPlayMusic.audio.Pause();
            UCPlayMusic.btnPlay.IsChecked = false;
        }

        public VIDEO SelectedVideo { 
            get => selectedVideo; 
            set
            {
                selectedVideo = value;
                nowPlaying = value;

                txtName.Text = selectedVideo.VIDEONAME;
                txtSingerName.Text = selectedVideo.SINGERNAME;


                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                try
                {
                    bitmapImage.UriSource = new Uri(selectedVideo.IMAGEURL);
                }
                catch
                {
                    bitmapImage.UriSource = new Uri("../" + selectedVideo.IMAGEURL, UriKind.RelativeOrAbsolute);
                }

                bitmapImage.EndInit();
                imgURL.ImageSource = bitmapImage;


                if (selectedVideo.SAVEPATH == null)
                {
                    DownloadFileToPlay(selectedVideo);
                }

                selectedVideo.POSITION = video.Position.TotalSeconds;
                slPlay.Value = (double)selectedVideo.POSITION;
                txtPosition.Text = new TimeSpan(0, (int)(selectedVideo.POSITION / 60), (int)(selectedVideo.POSITION % 60)).ToString(@"mm\:ss");

                video.Source = new Uri(selectedVideo.SAVEPATH);
                video.MediaOpened += Video_MediaOpened;
                if (init == 0)
                {
                    video.Pause();
                    btnPlay.IsChecked = false;
                    init++;
                }
                else
                {
                    video.Play();
                    btnPlay.IsChecked = true;
                }
            }
        }

        public static VIDEO NextVideo { get => nextVideo; set => nextVideo = value; }
        public static VIDEO PrevVideo { get => prevVideo; set => prevVideo = value; }
        public static ListBox CurrentList { get => currentList; set => currentList = value; }

        private void Video_MediaOpened(object sender, RoutedEventArgs e)
        {
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.DoWork += Bw_DoWork;
            selectedVideo.DURATION = video.NaturalDuration.TimeSpan.TotalSeconds;
            txtDuration.Text = new TimeSpan(0, (int)(selectedVideo.DURATION / 60), (int)(selectedVideo.DURATION % 60)).ToString(@"mm\:ss");
            slPlay.Maximum = (double)selectedVideo.DURATION;
            bw.RunWorkerAsync();
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(1000);
                bw.ReportProgress(1);
            }
        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            selectedVideo.POSITION = video.Position.TotalSeconds;
            slPlay.Value = (double)selectedVideo.POSITION;
            txtPosition.Text = new TimeSpan(0, (int)(selectedVideo.POSITION / 60), (int)(selectedVideo.POSITION % 60)).ToString(@"mm\:ss");
        }

        public static void DownloadFileToPlay(VIDEO selectedVideo)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Video\\" + selectedVideo.VIDEONAME + ".mp4";
            selectedVideo.SAVEPATH = path;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Video"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Video");
            if (!File.Exists(path))
            {
                WebClient wb = new WebClient();
                try
                {
                    wb.DownloadFile(selectedVideo.DOWNLOADURL, path);
                }
                catch
                {
                    CustomMessageBox.Show("Video không còn tồn tại", MessageBoxImage.Error);
                    return;
                }

            }
        }

        private void cbGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = comboBox.SelectedItem.ToString();
            listVid = new ObservableCollection<VIDEO>(DataProvider.Ins.DB.VIDEOs.Where(vid => vid.GENRE == item));
            lbVideos.ItemsSource = listVid;
        }

        private void lbVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if(listBox.SelectedItem == null)
            {
                return;
            }
            SelectedVideo = listBox.SelectedItem as VIDEO;
            if (listBox.SelectedIndex + 1 < listBox.Items.Count)
                NextVideo = listBox.Items[listBox.SelectedIndex + 1] as VIDEO;
            else
                NextVideo = null;

            if (listBox.SelectedIndex - 1 >= 0)
                PrevVideo = listBox.Items[listBox.SelectedIndex - 1] as VIDEO;
            else
                PrevVideo = null;
            currentList = listBox;
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (btnPlay.IsChecked == true)
            {
                video.Play();
            }
            else
            {
                video.Pause();
            }
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (nextVideo != null)
            {
                if(currentList.Name == "lbUploadVideos")
                {
                    UPLOADVIDEO temp = LoginViewModel.currUser.UPLOADVIDEOs.Where(s => s.VIDEOID.ToString() == nextVideo.VIDEOURL).FirstOrDefault();
                    currentList.SelectedItem = temp;
                }
                else
                {
                    currentList.SelectedItem = nextVideo;
                }
                
            }
        }

        private void btnPrev_Click(object sender, RoutedEventArgs e)
        {
            if (prevVideo != null)
            {
                if (currentList.Name == "lbUploadVideos")
                {
                    UPLOADVIDEO temp = LoginViewModel.currUser.UPLOADVIDEOs.Where(s => s.VIDEOID.ToString() == prevVideo.VIDEOURL).FirstOrDefault();
                    currentList.SelectedItem = temp;
                }
                else
                {
                    currentList.SelectedItem = prevVideo;
                }
            }
        }

        private void slPlay_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            selectedVideo.POSITION = slPlay.Value;
            video.Position = new TimeSpan(0, 0, (int)selectedVideo.POSITION);
            txtPosition.Text = new TimeSpan(0, (int)(selectedVideo.POSITION / 60), (int)(selectedVideo.POSITION % 60)).ToString(@"mm\:ss");
        }

        private void btnMute_Click(object sender, RoutedEventArgs e)
        {
            if (btnMute.IsChecked == true)
            {
                video.Volume = 0;
            }
            else
            {
                video.Volume = slVolume.Value;
            }
        }

        private void btnUp_Click(object sender, RoutedEventArgs e)
        {
            Upload.uploadVideo = 1;
            Upload upload = new Upload();
            upload.ShowDialog();
            UpdateUploadVideo();
            Upload.uploadVideo = 0;
        }

        private void lbUploadVideos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            UPLOADVIDEO select = listBox.SelectedItem as UPLOADVIDEO;
            if (select == null)
            {
                return;
            }
            else
            {
                VIDEO temp = new VIDEO() { VIDEONAME = select.VIDEONAME, SINGERNAME = select.SINGERNAME, IMAGEURL = select.IMAGEPATH, SAVEPATH = select.SAVEPATH, VIDEOURL = select.VIDEOID.ToString() };
                SelectedVideo = temp;
                if (listBox.SelectedIndex + 1 < listBox.Items.Count)
                {
                    UPLOADVIDEO tmp = listBox.Items[listBox.SelectedIndex + 1] as UPLOADVIDEO;
                    NextVideo = new VIDEO() { VIDEONAME = tmp.VIDEONAME, SINGERNAME = tmp.SINGERNAME, IMAGEURL = tmp.IMAGEPATH, SAVEPATH = tmp.SAVEPATH, VIDEOURL = tmp.VIDEOID.ToString() };
                }
                else
                    NextVideo = null;

                if (listBox.SelectedIndex - 1 >= 0)
                {
                    UPLOADVIDEO tmp = listBox.Items[listBox.SelectedIndex - 1] as UPLOADVIDEO;
                    PrevVideo = new VIDEO() { VIDEONAME = tmp.VIDEONAME, SINGERNAME = tmp.SINGERNAME, IMAGEURL = tmp.IMAGEPATH, SAVEPATH = tmp.SAVEPATH, VIDEOURL = tmp.VIDEOID.ToString() };
                }
                else
                    PrevVideo = null;
                currentList = listBox;
            }
        }
        public void UpdateUploadVideo()
        {
            listVidUp = new ObservableCollection<UPLOADVIDEO>(LoginViewModel.currUser.UPLOADVIDEOs);
            lbUploadVideos.ItemsSource = listVidUp;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Upload.updateVid = 1;

            Button btn = (Button)sender;
            UPLOADVIDEO s = btn.DataContext as UPLOADVIDEO;
            Upload.updateVideo = s;
            Upload upload = new Upload();

            upload.ShowDialog();
            UpdateUploadVideo();
            Upload.updateVid = 0;
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            if (CustomMessageBox.Show("Bạn có chắc chắn muốn xóa video này?", MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
            {
                Button btn = (Button)sender;
                UPLOADVIDEO s = btn.DataContext as UPLOADVIDEO;
                LoginViewModel.currUser.UPLOADVIDEOs.Remove(s);
                DataProvider.Ins.DB.SaveChanges();
                UpdateUploadVideo();
                CustomMessageBox.Show("Đã xóa video thành công", MessageBoxImage.None);
            }
            else
            {
                return;
            }
        }

        private void slVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (slVolume.Value == 0)
                btnMute.IsChecked = true;
            else
                btnMute.IsChecked = false;
            video.Volume = slVolume.Value;
        }
    }
}
