using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
using System.Windows.Threading;
using System.Threading;

namespace MusicPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCPlayMusic.xaml
    /// </summary>
    public partial class UCPlayMusic : UserControl
    {
        public static MediaElement audio = Application.Current.TryFindResource("audio") as MediaElement;
        public static BackgroundWorker bw;
        public static Slider sliderPlay = Application.Current.TryFindResource("slPlay") as Slider;
        public static TextBlock position = Application.Current.TryFindResource("position") as TextBlock;
        public UCPlayMusic()
        {
            InitializeComponent();
            this.DataContext = this;
            sliderPlay.PreviewMouseUp += SliderPlay_PreviewMouseUp;
        }

        private void SliderPlay_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            selectedSong.POSITION = sliderPlay.Value;
            audio.Position = new TimeSpan(0, 0, (int)selectedSong.POSITION);
            position.Text = new TimeSpan(0, (int)(selectedSong.POSITION / 60), (int)(selectedSong.POSITION % 60)).ToString(@"mm\:ss");
        }

        private static void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(1000);
                bw.ReportProgress(1);
            }
        }

        private static void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            selectedSong.POSITION = audio.Position.TotalSeconds;
            sliderPlay.Value = (double)selectedSong.POSITION;
            position.Text = new TimeSpan(0, (int)(selectedSong.POSITION / 60), (int)(selectedSong.POSITION % 60)).ToString(@"mm\:ss");
        }

        static SONG selectedSong;

        public static SONG SelectedSong
        {
            get => selectedSong;
            set
            {
                selectedSong = value;
                
                var singerName = Application.Current.TryFindResource("singerName") as TextBlock;
                singerName.Text = selectedSong.SINGERNAME;

                var songName = Application.Current.TryFindResource("songName") as TextBlock;
                songName.Text = selectedSong.SONGNAME;

                var imgURL = Application.Current.TryFindResource("imgURL") as Image;
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = new Uri(selectedSong.IMAGEURL); 
                bitmapImage.EndInit();
                imgURL.Source = bitmapImage;

                DownloadFileToPlay(selectedSong);
                selectedSong.POSITION = audio.Position.TotalSeconds;
                sliderPlay.Value = (double)selectedSong.POSITION;
                position.Text = new TimeSpan(0, (int)(selectedSong.POSITION / 60), (int)(selectedSong.POSITION % 60)).ToString(@"mm\:ss");

                audio.Source = new Uri(selectedSong.SAVEPATH);
                audio.MediaOpened += Audio_MediaOpened;
                audio.Play();
            }
        }

        private static void Audio_MediaOpened(object sender, RoutedEventArgs e)
        {
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.DoWork += Bw_DoWork;
            selectedSong.DURATION = audio.NaturalDuration.TimeSpan.TotalSeconds;
            var duration = Application.Current.TryFindResource("duration") as TextBlock;
            duration.Text = new TimeSpan(0, (int)(selectedSong.DURATION / 60), (int)(selectedSong.DURATION % 60)).ToString(@"mm\:ss");
            sliderPlay.Maximum = (double)selectedSong.DURATION;

            bw.RunWorkerAsync();
        }

        public static void DownloadFileToPlay(SONG selectedSong)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Song\\" + selectedSong.SONGNAME + ".mp3";
            selectedSong.SAVEPATH = path;
            if (!File.Exists(path))
            {
                WebClient wb = new WebClient();
                wb.DownloadFile(selectedSong.DOWNLOADURL, path);
            }
        }

        private void btThreePoint_Click(object sender, RoutedEventArgs e)
        {
            Infor infor = new Infor();
            infor.tblName2.Text = selectedSong.SONGNAME;
            infor.tblName4.Text = selectedSong.SINGERNAME;
            infor.tblTime2.Text = new TimeSpan(0, (int)(selectedSong.DURATION / 60), (int)(selectedSong.DURATION % 60)).ToString(@"mm\:ss");
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(selectedSong.IMAGEURL);
            bitmapImage.EndInit();
            infor.img.ImageSource = bitmapImage;
            infor.ShowDialog();
        }
    }
}
