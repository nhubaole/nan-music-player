using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.Model
{
    public class Song
    {
        string songName;
        string singerName;
        string songURL;
        string downloadURL;
        string imageURL;

        public string SongName { get => songName; set => songName = value; }
        public string SingerName { get => singerName; set => singerName = value; }
        public string SongURL { get => songURL; set => songURL = value; }
        public string DownloadURL { get => downloadURL; set => downloadURL = value; }
        public string ImageURL { get => imageURL; set => imageURL = value; }
    }
}
