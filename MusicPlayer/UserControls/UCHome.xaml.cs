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
        public ObservableCollection<Song> listFeaturedSong;
        public ObservableCollection<Song> listNewdSong;
        public UCHome()
        {
            InitializeComponent();
            loadFeaturedSong();

        }

        public void loadFeaturedSong()
        {
            listFeaturedSong = new ObservableCollection<Song>();
            this.DataContext = this;
            lsbTopSongs.ItemsSource = listFeaturedSong;
            HttpRequest httpRequest = new HttpRequest();
            string htmlFeaturedSong = httpRequest.Get(@"https://www.nhaccuatui.com/bai-hat/top-20.nhac-viet.html").ToString();
            string feateredPattern = @"<ul class=""list_show_chart"">(.*?)</ul>";
            var featuredSongs = Regex.Matches(htmlFeaturedSong, feateredPattern, RegexOptions.Singleline);
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
                string htmlXML = httpXML.Get(xmlURL).ToString();

                string songName = Regex.Match(htmlXML, @"<title>(.*?)</title>", RegexOptions.Singleline).ToString();
                int indexCDATA = songName.IndexOf("<![CDATA[");
                int indexEnd = songName.IndexOf("]]>");
                songName = songName.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");

                string singerName = Regex.Match(htmlXML, @"<creator>(.*?)</creator>", RegexOptions.Singleline).ToString();
                indexCDATA = singerName.IndexOf("<![CDATA[");
                indexEnd = singerName.IndexOf("]]>");
                singerName = singerName.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");

                string songURL = Regex.Match(htmlXML, @"<info>(.*?)</info>", RegexOptions.Singleline).ToString();
                indexCDATA = songURL.IndexOf("<![CDATA[");
                indexEnd = songURL.IndexOf("]]>");
                songURL = songURL.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");

                string downloadURL = Regex.Match(htmlXML, @"<location>(.*?)</location>", RegexOptions.Singleline).ToString();
                indexCDATA = downloadURL.IndexOf("<![CDATA[");
                indexEnd = downloadURL.IndexOf("]]>");
                downloadURL = downloadURL.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");

                string imageURL = Regex.Match(htmlXML, @"<coverimage>(.*?)</coverimage>", RegexOptions.Singleline).ToString();
                indexCDATA = imageURL.IndexOf("<![CDATA[");
                indexEnd = imageURL.IndexOf("]]>");
                imageURL = imageURL.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");

                listFeaturedSong.Add(new Song() { SongName = songName, SingerName = singerName, DownloadURL = downloadURL, SongURL = songURL, ImageURL = imageURL });
            }
        }
    }
}
