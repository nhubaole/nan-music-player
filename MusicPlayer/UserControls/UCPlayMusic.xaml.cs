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

namespace MusicPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCPlayMusic.xaml
    /// </summary>
    public partial class UCPlayMusic : UserControl
    {
        public static MediaElement audio;
        public UCPlayMusic()
        {
            InitializeComponent();
            this.DataContext = this;
            
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

                audio = Application.Current.TryFindResource("audio") as MediaElement;
                audio.Source = new Uri(selectedSong.SAVEPATH);
                
                audio.MediaOpened += Audio_MediaOpened;
                audio.Play();

                selectedSong.POSITION = 0;
                var position = Application.Current.TryFindResource("position") as TextBlock;
                position.Text = new TimeSpan(0, (int)(selectedSong.POSITION / 60), (int)(selectedSong.POSITION % 60)).ToString(@"mm\:ss"); 
            }
        }

        private static void Audio_MediaOpened(object sender, RoutedEventArgs e)
        {
            selectedSong.DURATION = audio.NaturalDuration.TimeSpan.TotalSeconds;
            var duration = Application.Current.TryFindResource("duration") as TextBlock;
            duration.Text = new TimeSpan(0, (int)(selectedSong.DURATION / 60), (int)(selectedSong.DURATION % 60)).ToString(@"mm\:ss");
        }

        public static void DownloadFileToPlay(SONG selectedSong)
        {
            string path = selectedSong.SAVEPATH;
            if (!File.Exists(path))
            {
                WebClient wb = new WebClient();
                wb.DownloadFile(selectedSong.DOWNLOADURL, path);
            }
        }
    }
}
