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
using xNet;

namespace MusicPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCHome.xaml
    /// </summary>
    public partial class UCHome : UserControl
    {
        public static ObservableCollection<Song> listFeaturedSong = new ObservableCollection<Song>();
        public static ObservableCollection<Song> listNewSong = new ObservableCollection<Song>();
        public static int init = 0;

        public UCHome()
        {
            InitializeComponent();
            if(init == 0)
            {
                loadFeaturedSong();
                loadNewSong();
                UCPlayMusic.SelectedSong = listFeaturedSong.First();
                init++;
            }
            lbNewSongs.ItemsSource = listNewSong;
            lbFeaturedSongs.ItemsSource = listFeaturedSong;
        }
        
        public void loadNewSong()
        {
            this.DataContext = this;
            HttpRequest httpRequest = new HttpRequest();
            string htmlNewSong = httpRequest.Get(@"https://www.nhaccuatui.com/homepage").ToString();
            string newPattern = @"MỚI PHÁT HÀNH</a>(.*?)</ul>";

            var newSongs = Regex.Matches(htmlNewSong, newPattern, RegexOptions.Singleline);
            string newSong = newSongs[0].ToString();

            var listSongHTML = Regex.Matches(newSong, @"<li>(.*?)</li>", RegexOptions.Singleline);
            int count = 5;
            for (int i = 0; i < count; i++)
            {
                var theA = Regex.Matches(listSongHTML[i].ToString(), @"<a href=""(.*?)</a>", RegexOptions.Singleline);
                string songString = theA[0].ToString();
                int indexSong = songString.IndexOf("class=\"");
                int indexURL = songString.IndexOf("href=\"");
                string URL = songString.Substring(indexURL, indexSong - indexURL - 2).Replace("href=\"", "");

                string htmlSong = httpRequest.Get(URL).ToString();
                string xmlURL = Regex.Match(htmlSong, @"xmlURL = ""(.*?)""").ToString();
                xmlURL = xmlURL.Replace(@"xmlURL = """, "").Replace(@"""", "");
                if (xmlURL == "")
                {
                    count++;
                    continue;
                }
                string htmlXML = httpRequest.Get(xmlURL).ToString();
                string songName = getData("title", htmlXML);
                string singerName = getData("creator", htmlXML);
                string songURL = getData("info", htmlXML);
                string downloadURL = getData("location", htmlXML);
                string imageURL = getData("coverimage", htmlXML);
                string savePath = AppDomain.CurrentDomain.BaseDirectory + "Song\\" + songName + ".mp3";

                listNewSong.Add(new Song() { SongName = songName, SingerName = singerName, DownloadURL = downloadURL, SongURL = songURL, ImageURL = imageURL,  SavePath = savePath});
            }
        }

        public void loadFeaturedSong()
        {
            listFeaturedSong = new ObservableCollection<Song>();
            this.DataContext = this;
            HttpRequest httpRequest = new HttpRequest();
            string htmlFeaturedSong = httpRequest.Get(@"https://www.nhaccuatui.com/bai-hat/top-20.nhac-viet.tuan--2.0.html").ToString();
            string featuredPattern = @"<ul class=""list_show_chart"">(.*?)</ul>";
            var featuredSongs = Regex.Matches(htmlFeaturedSong, featuredPattern, RegexOptions.Singleline);
            string featuredSong = featuredSongs[0].ToString();

            var listSongHTML = Regex.Matches(featuredSong, @"<li>(.*?)</li>", RegexOptions.Singleline);
            int count = 5;
            for (int i = 0; i < count; i++)
            {
                var theA = Regex.Matches(listSongHTML[i].ToString(), @"<a href=""(.*?)</a>", RegexOptions.Singleline);
                string songString = theA[0].ToString();
                int indexSong = songString.IndexOf("title=\"");
                int indexURL = songString.IndexOf("href=\"");
                string URL = songString.Substring(indexURL, indexSong - indexURL - 2).Replace("href=\"", "");

                HttpRequest httpSong = new HttpRequest();
                string htmlSong = httpSong.Get(URL).ToString();
                string xmlURL = Regex.Match(htmlSong, @"xmlURL = ""(.*?)""").ToString();
                xmlURL = xmlURL.Replace(@"xmlURL = """, "").Replace(@"""", "");
                if (xmlURL == "")
                {
                    count++;
                    continue;
                }

                HttpRequest httpXML = new HttpRequest();
                string htmlXML = httpRequest.Get(xmlURL).ToString();
                string songName = getData("title", htmlXML);
                string singerName = getData("creator", htmlXML);
                string songURL = getData("info", htmlXML);
                string downloadURL = getData("location", htmlXML);
                string imageURL = getData("coverimage", htmlXML);
                string savePath = AppDomain.CurrentDomain.BaseDirectory + "Song\\" + songName + ".mp3";

                listFeaturedSong.Add(new Song() { SongName = songName, SingerName = singerName, DownloadURL = downloadURL, SongURL = songURL, ImageURL = imageURL, SavePath = savePath });
            }
        }

        public string getData(string tag, string htmlXML)
        {
            string result = Regex.Match(htmlXML, @"<" + tag +">(.*?)</" + tag +"", RegexOptions.Singleline).ToString();
            int indexCDATA = result.IndexOf("<![CDATA[");
            int indexEnd = result.IndexOf("]]>");
            result = result.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");
            return result;
        }

        private void lbFeaturedSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addListLastestSong(sender);
        }

        public static void addListLastestSong(object sender)
        {
            ListBox ls = sender as ListBox;
            Song song = ls.SelectedItem as Song;
            UCLibrary.listLastestSong.Insert(0, song);
            if (UCLibrary.listLastestSong.Count() > 5)
            {
                UCLibrary.listLastestSong.RemoveAt(5);
            }
            UCPlayMusic.SelectedSong = song;
        }

        private void lbNewSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            addListLastestSong(sender);
        }
    }
}
