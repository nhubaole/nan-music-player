using Microsoft.Win32;
using MusicPlayer.UserControls;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for Infor.xaml
    /// </summary>
    public partial class Infor : Window
    {
        public Infor()
        {
            InitializeComponent();

        }

        private void btnDownload_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Media Files(*.avi; *.mpeg; *.wav; *.midi; *.mp4; *.mp3)|*.avi; *.mpeg; *.wav; *.midi; *.mp4; *.mp3";
                if (saveFileDialog.ShowDialog() == true)
                {
                    string path = saveFileDialog.FileName;
                    if (!File.Exists(path))
                    {
                        File.Copy(UCPlayMusic.SelectedSong.SAVEPATH, path, true);
                    }
                    CustomMessageBox.Show("Tải xuống thành công", MessageBoxImage.None);
                }
            }
            catch
            {
                CustomMessageBox.Show("Không thể tải được bài hát", MessageBoxImage.Error);
            }
        }
    }
}
