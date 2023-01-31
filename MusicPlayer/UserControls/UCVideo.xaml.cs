using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    /// Interaction logic for UCVideo.xaml
    /// </summary>
    public partial class UCVideo : UserControl
    {
        public static ObservableCollection<VIDEO> listVid;
        public static List<string> listGenre = new List<string>() { "Việt Nam", "Âu Mỹ", "Nhạc Hàn", "Nhạc Hoa", "Thiếu Nhi", "Nhạc Nhật", "Nhạc Thái" };
        public UCVideo()
        {
            InitializeComponent();
            cbGenre.ItemsSource = listGenre;
            
            
        }

        private void cbGenre_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string item = comboBox.SelectedItem.ToString();
            listVid = new ObservableCollection<VIDEO>(DataProvider.Ins.DB.VIDEOs.Where(vid => vid.GENRE == item));
            lbVideos.ItemsSource = listVid;
        }
    }
}
