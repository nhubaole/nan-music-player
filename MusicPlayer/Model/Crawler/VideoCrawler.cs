using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using xNet;

namespace MusicPlayer.Model.Crawler
{
    internal class VideoCrawler
    {
        [STAThread]
        static void Main(string[] args)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text files (*.txt)|*.txt";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName, false, Encoding.UTF8);

                HttpRequest httpRequest = new HttpRequest();
                string htmlListVideo = httpRequest.Get(@"https://www.nhaccuatui.com/video.html").ToString();
                string Pattern = @"<div class=""list_video"">(.*?)</ul>";
                var Videos = Regex.Matches(htmlListVideo, Pattern, RegexOptions.Singleline);

                //change here to crawl video by genre
                int genreNum = 0;


                string genre = "";
                switch (genreNum)
                {
                    case 0:
                        {
                            genre = "Việt Nam";
                            break;
                        }
                    case 1:
                        {
                            genre = "Âu Mỹ";
                            break;
                        }
                    case 2:
                        {
                            genre = "Nhạc Hàn";
                            break;
                        }
                    case 3:
                        {
                            genre = "Nhạc Hoa";
                            break;
                        }
                    case 4:
                        {
                            genre = "Thiếu Nhi";
                            break;
                        }
                    case 5:
                        {
                            genre = "Nhạc Nhật";
                            break;
                        }
                    case 6:
                        {
                            genre = "Nhạc Thái";
                            break;
                        }

                }


                string Video = Videos[genreNum].ToString();

                var listVideoHTML = Regex.Matches(Video, @"<li>(.*?)</li>", RegexOptions.Singleline);
                int count = 8;


                for (int i = 0; i < count; i++)
                {
                    var theA = Regex.Matches(listVideoHTML[i].ToString(), @"<a href=""(.*?)</a>", RegexOptions.Singleline);
                    string VideoString = theA[0].ToString();
                    int indexVideo = VideoString.IndexOf("class=\"");
                    int indexURL = VideoString.IndexOf("href=\"");
                    string URL = VideoString.Substring(indexURL, indexVideo - indexURL - 2).Replace("href=\"", "");

                    HttpRequest httpVideo = new HttpRequest();
                    string htmlVideo = httpVideo.Get(URL).ToString();
                    string xmlURL = Regex.Match(htmlVideo, @"xmlURL = ""(.*?)""").ToString();
                    xmlURL = xmlURL.Replace(@"xmlURL = """, "").Replace(@"""", "");

                    HttpRequest httpXML = new HttpRequest();
                    string htmlXML = httpRequest.Get(xmlURL).ToString();
                    string VideoName = getData("title", htmlXML);
                    string singerName = getData("singer", htmlXML);
                    string VideoURL = getData("info", htmlXML);
                    string downloadURL = getData("location", htmlXML);
                    string imageURL = getData("thumb", htmlXML);
                    sw.WriteLine("INSERT INTO VIDEO (VIDEONAME, SINGERNAME, VIDEOURL, DOWNLOADURL, IMAGEURL, GENRE) VALUES (N'" + VideoName + "', N'" + singerName + "', '" + VideoURL + "', '" + downloadURL + "', '" + imageURL + "', N'" + genre + "')");
                }

                sw.Close();
            }
            MessageBox.Show("Xong");
        }

        public static string getData(string tag, string htmlXML)
        {
            var result = Regex.Matches(htmlXML, @"<" + tag + ">(.*?)</" + tag + "", RegexOptions.Singleline);
            string res = result[0].ToString();
            int indexCDATA = res.IndexOf("<![CDATA[");
            int indexEnd = res.IndexOf("]]>");
            res = res.Substring(indexCDATA, indexEnd - indexCDATA).Replace("<![CDATA[", "");
            return res;
        }
    }
}
