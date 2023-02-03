//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;
//using System.Windows.Forms;
//using xNet;

//namespace MusicPlayer.Model.Crawler
//{
//    internal class SongCrawler
//    {
//        static void test()
//        {
//            SaveFileDialog saveFileDialog = new SaveFileDialog();
//            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
//            if (saveFileDialog.ShowDialog() == DialogResult.OK)
//            {
//                StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8);

//                HttpRequest httpRequest = new HttpRequest();
//                string htmlListSong = httpRequest.Get(@"https://www.nhaccuatui.com/top100/top-100-nhac-han.iciV0mD8L9Ed.html").ToString();
//                string Pattern = @"<ul class=""list_show_chart"">(.*?)</ul>";
//                var Songs = Regex.Matches(htmlListSong, Pattern, RegexOptions.Singleline);
//                string Song = Songs[0].ToString();

//                var listSongHTML = Regex.Matches(Song, @"<li>(.*?)</li>", RegexOptions.Singleline);
//                int count = 100;
//                var theA = Regex.Matches(listSongHTML[0].ToString(), @"<a href=""(.*?)</a>", RegexOptions.Singleline);
//                string songString = theA[0].ToString();
//                int indexSong = songString.IndexOf("title=\"");
//                int indexURL = songString.IndexOf("href=\"");
//                string URL = songString.Substring(indexURL, indexSong - indexURL - 2).Replace("href=\"", "");

//                HttpRequest httpSong = new HttpRequest();
//                string htmlSong = httpSong.Get(URL).ToString();
//                string xmlURL = Regex.Match(htmlSong, @"xmlURL = ""(.*?)""").ToString();
//                xmlURL = xmlURL.Replace(@"xmlURL = """, "").Replace(@"""", "");

//                for (int i = 0; i < count; i++)
//                {
//                    HttpRequest httpXML = new HttpRequest();
//                    string htmlXML = httpRequest.Get(xmlURL).ToString();
//                    string songName = getData("title", htmlXML, i);
//                    string singerName = getData("creator", htmlXML, i);
//                    string songURL = getData("info", htmlXML, i);
//                    string downloadURL = getData("location", htmlXML, i);
//                    string imageURL = getData("avatar", htmlXML, i);
//                    sw.WriteLine("INSERT INTO SONG (SONGNAME, SINGERNAME, SONGURL, DOWNLOADURL, IMAGEURL, GENRE) VALUES (N'" + songName + "', N'" + singerName + "', '" + songURL + "', '" + downloadURL + "', '" + imageURL + "', N'Hàn Quốc')");
//                }
                
//                sw.Close();
//            }
//            MessageBox.Show("Xong");
//        }

//        public static string getData(string tag, string htmlXML, int i)
//        {
//            var result = Regex.Matches(htmlXML, @"<" + tag + ">(.*?)</" + tag + "", RegexOptions.Singleline);
//            string res = result[i].ToString();
//            int indexCDATA = res.IndexOf("<![CDATA[");
//            int indexEnd = res.IndexOf("]]>");
//            res = res.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");
//            return res;
//        }
//    }
//}
