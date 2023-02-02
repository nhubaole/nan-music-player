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
using System.Windows.Controls.Primitives;

namespace MusicPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCPlayMusic.xaml
    /// </summary>
    public partial class UCPlayMusic : UserControl
    {
        public static int init = 0;
        public static MediaElement audio = Application.Current.TryFindResource("audio") as MediaElement;
        public static BackgroundWorker bw;
        public static Slider sliderPlay = Application.Current.TryFindResource("slPlay") as Slider;
        public static Slider sliderVolume = Application.Current.TryFindResource("slVolume") as Slider;
        public static TextBlock position = Application.Current.TryFindResource("position") as TextBlock;
        public static TextBlock duration = Application.Current.TryFindResource("duration") as TextBlock;
        public static ToggleButton btnPlay = Application.Current.TryFindResource("btnPlay") as ToggleButton;
        public static Button btnPrev = Application.Current.TryFindResource("btnPrev") as Button;
        public static Button btnNext = Application.Current.TryFindResource("btnNext") as Button;
        public static ToggleButton btnRepeat = Application.Current.TryFindResource("btnRepeat") as ToggleButton;
        public static ToggleButton btnRandom = Application.Current.TryFindResource("btnRandom") as ToggleButton;
        public static ToggleButton btnMute = Application.Current.TryFindResource("btnMute") as ToggleButton;
        public UCPlayMusic()
        {
            InitializeComponent();
            this.DataContext = this;
            sliderVolume.Maximum = 1;
            sliderVolume.Value = 0.5;
            sliderVolume.PreviewMouseUp += SliderVolume_PreviewMouseUp;
            sliderPlay.PreviewMouseUp += SliderPlay_PreviewMouseUp;
            btnPlay.Click += BtnPlay_Click;
            btnPrev.Click += BtnPrev_Click;
            btnNext.Click += BtnNext_Click;
            btnMute.Click += BtnMute_Click;
        }

        private void SliderVolume_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sliderVolume.Value == 0)
                btnMute.IsChecked = true;
            else
                btnMute.IsChecked = false;
            audio.Volume = sliderVolume.Value;
        }

        private void BtnMute_Click(object sender, RoutedEventArgs e)
        {
            if (btnMute.IsChecked == true)
            {
                audio.Volume = 0;
            }
            else
            {
                audio.Volume = sliderVolume.Value;
            }
        }

        private void BtnNext_Click(object sender, RoutedEventArgs e)
        {
            if (nextSong != null)
            {
                currentList.SelectedItem = nextSong;
            }
        }

        private void BtnPrev_Click(object sender, RoutedEventArgs e)
        {
            if(prevSong != null)
            {
                currentList.SelectedItem = prevSong;
            }
        }

        private void BtnPlay_Click(object sender, RoutedEventArgs e)
        {
            if (btnPlay.IsChecked == true)
            {
                audio.Play();
            }
            else
            {
                audio.Pause();
            }
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
            if(position.Text == duration.Text)
            {
                if(btnRepeat.IsChecked == true)
                {
                    SelectedSong = selectedSong;
                    return;
                }
                if(btnRandom.IsChecked == true)
                {
                    currentList.SelectedItem = currentList.Items[RandomSong(currentList.Items.Count)];
                    return;
                }
                currentList.SelectedItem = nextSong;
            }
        }

        public static int RandomSong(int length)
        {
            Random random = new Random();
            return random.Next(0, length - 1);
        }

        static SONG selectedSong;
        static SONG nextSong;
        static SONG prevSong;
        static ListBox currentList;

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
                bitmapImage.UriSource = new Uri(selectedSong.IMAGEURL, UriKind.RelativeOrAbsolute);
                bitmapImage.EndInit();
                imgURL.Source = bitmapImage;

                if(selectedSong.SAVEPATH == null)
                {
                    DownloadFileToPlay(selectedSong);
                }
                selectedSong.POSITION = audio.Position.TotalSeconds;
                sliderPlay.Value = (double)selectedSong.POSITION;
                position.Text = new TimeSpan(0, (int)(selectedSong.POSITION / 60), (int)(selectedSong.POSITION % 60)).ToString(@"mm\:ss");

                audio.Source = new Uri(selectedSong.SAVEPATH);
                audio.MediaOpened += Audio_MediaOpened;
                if(init == 0)
                {
                    audio.Pause();
                    btnPlay.IsChecked = false;
                    init++;
                }
                else
                {
                    audio.Play();
                    btnPlay.IsChecked = true;
                }
            }
        }

        public static SONG NextSong { get => nextSong; set => nextSong = value; }
        public static SONG PrevSong { get => prevSong; set => prevSong = value; }
        public static ListBox CurrentList { get => currentList; set => currentList = value; }

        private static void Audio_MediaOpened(object sender, RoutedEventArgs e)
        {
            bw = new BackgroundWorker();
            bw.WorkerSupportsCancellation = true;
            bw.WorkerReportsProgress = true;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.DoWork += Bw_DoWork;
            selectedSong.DURATION = audio.NaturalDuration.TimeSpan.TotalSeconds;
            duration.Text = new TimeSpan(0, (int)(selectedSong.DURATION / 60), (int)(selectedSong.DURATION % 60)).ToString(@"mm\:ss");
            sliderPlay.Maximum = (double)selectedSong.DURATION;
            bw.RunWorkerAsync();
        }

        public static void DownloadFileToPlay(SONG selectedSong)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "Song\\" + selectedSong.SONGNAME + ".mp3";
            selectedSong.SAVEPATH = path;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "Song"))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "Song");
            if (!File.Exists(path))
            {
                WebClient wb = new WebClient();
                try
                {
                    wb.DownloadFile(selectedSong.DOWNLOADURL, path);
                }
                catch
                {
                    MessageBox.Show("Bài hát không còn tồn tại");
                    return;
                }
                
            }
        }

        private void btThreePoint_Click(object sender, RoutedEventArgs e)
        {
            Infor infor = new Infor();
            infor.txtLike.Text = selectedSong.USERS.Count().ToString();
            infor.tblName2.Text = selectedSong.SONGNAME;
            infor.tblName4.Text = selectedSong.SINGERNAME;
            infor.tblGenre2.Text = selectedSong.GENRE;
            infor.tblTime2.Text = new TimeSpan(0, (int)(selectedSong.DURATION / 60), (int)(selectedSong.DURATION % 60)).ToString(@"mm\:ss");
            var bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            try
            {
                bitmapImage.UriSource = new Uri(selectedSong.IMAGEURL);
            }
            catch
            {
                bitmapImage.UriSource = new Uri("../" + selectedSong.IMAGEURL, UriKind.RelativeOrAbsolute);
            }

            bitmapImage.EndInit();
            infor.img.ImageSource = bitmapImage;
            infor.ShowDialog();
        }
    }
}
