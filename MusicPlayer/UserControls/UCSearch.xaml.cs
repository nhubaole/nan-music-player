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
    /// Interaction logic for UCSearch.xaml
    /// </summary>
    public partial class UCSearch : UserControl
    {
        public static ObservableCollection<Song> listSong = new ObservableCollection<Song>();
        public static int init = 0;
        public UCSearch()
        {
            InitializeComponent();
            if (init == 0)
            {
                loadSong();
                init++;
            }
            lbSongs.ItemsSource = listSong;
        }

        public void loadSong()
        {
            this.DataContext = this;
            HttpRequest httpRequest = new HttpRequest();
            string htmlListSong = httpRequest.Get(@"https://www.nhaccuatui.com/top100/top-100-nhac-tre.m3liaiy6vVsF.html").ToString();
            string Pattern = @"<ul class=""list_show_chart"">(.*?)</ul>";
            var Songs = Regex.Matches(htmlListSong, Pattern, RegexOptions.Singleline);
            string Song = Songs[0].ToString();

            var listSongHTML = Regex.Matches(Song, @"<li>(.*?)</li>", RegexOptions.Singleline);
            int count = 100;
            var theA = Regex.Matches(listSongHTML[0].ToString(), @"<a href=""(.*?)</a>", RegexOptions.Singleline);
            string songString = theA[0].ToString();
            int indexSong = songString.IndexOf("title=\"");
            int indexURL = songString.IndexOf("href=\"");
            string URL = songString.Substring(indexURL, indexSong - indexURL - 2).Replace("href=\"", "");

            HttpRequest httpSong = new HttpRequest();
            string htmlSong = httpSong.Get(URL).ToString();
            string xmlURL = Regex.Match(htmlSong, @"xmlURL = ""(.*?)""").ToString();
            xmlURL = xmlURL.Replace(@"xmlURL = """, "").Replace(@"""", "");

            for (int i = 0; i < count; i++)
            {
                HttpRequest httpXML = new HttpRequest();
                string htmlXML = httpRequest.Get(xmlURL).ToString();
                string songName = getData("title", htmlXML, i);
                string singerName = getData("creator", htmlXML, i);
                string songURL = getData("info", htmlXML, i);
                string downloadURL = getData("location", htmlXML, i);
                string imageURL = getData("avatar", htmlXML, i);
                string savePath = AppDomain.CurrentDomain.BaseDirectory + "Song\\" + songName + ".mp3";

                listSong.Add(new Song() { SongName = songName, SingerName = singerName, DownloadURL = downloadURL, SongURL = songURL, ImageURL = imageURL, SavePath = savePath });
            }
        }

        public string getData(string tag, string htmlXML, int i)
        {
            var result = Regex.Matches(htmlXML, @"<" + tag + ">(.*?)</" + tag + "", RegexOptions.Singleline);
            string res = result[i].ToString();
            int indexCDATA = res.IndexOf("<![CDATA[");
            int indexEnd = res.IndexOf("]]>");
            res = res.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");
            return res;
        }

        private void lbSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UCHome.addListLastestSong(sender);
        }
    }
}
