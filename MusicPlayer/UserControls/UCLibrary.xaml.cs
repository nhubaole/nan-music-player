using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
    /// Interaction logic for UCLibrary.xaml
    /// </summary>
    public partial class UCLibrary : UserControl
    {
        public static int init = -1;
        public static BackgroundWorker bw2;
        public static TimeSpan timer;
        public static ObservableCollection<SONG> listLastestSong = new ObservableCollection<SONG>();
        public static ObservableCollection<string> listTimer = new ObservableCollection<string>() { "15 phút" , "30 phút", "1 giờ", "2 giờ", "Tắt hẹn giờ" };
        public static ComboBox cbTimer = Application.Current.TryFindResource("cbTimer") as ComboBox;
        public static TextBlock txtTimer = Application.Current.TryFindResource("txtTimer") as TextBlock;
        public UCLibrary()
        {
            InitializeComponent();
            if(init == -1)
            {
                bw2 = new BackgroundWorker();
                bw2.WorkerSupportsCancellation = true;
                bw2.WorkerReportsProgress = true;
                bw2.ProgressChanged += Bw2_ProgressChanged;
                bw2.DoWork += Bw2_DoWork;
                bw2.RunWorkerAsync();
                init++;
            }
            lbLastestSongs.ItemsSource = listLastestSong;
            cbTimer.ItemsSource = listTimer;
            cbTimer.SelectionChanged += CbTimer_SelectionChanged;
        }

        private void CbTimer_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(init == 0)
            {
                init++;
            }
            if(cbTimer.SelectedItem != null)
            {
                if(cbTimer.SelectedItem == cbTimer.Items[0])
                {
                    timer = new TimeSpan(0, 15, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[1])
                {
                    timer = new TimeSpan(0, 30, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[2])
                {
                    timer = new TimeSpan(1, 0, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[3])
                {
                    timer = new TimeSpan(2, 0, 0);
                }
                else if (cbTimer.SelectedItem == cbTimer.Items[4])
                {
                    init = 0;
                    txtTimer.Text = cbTimer.SelectedItem.ToString();
                }

            }
        }

        private void Bw2_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                Thread.Sleep(1000);
                bw2.ReportProgress(1);
            }
        }

        private void Bw2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (init != 0 && init != -1)
            {
                if(cbTimer.Text == "00:00:00")
                {
                    Application.Current.Shutdown();
                }
                else
                {
                    timer = timer.Subtract(new TimeSpan(0, 0, 1));
                    this.Dispatcher.Invoke(() =>
                    {
                        cbTimer.Text = timer.ToString();
                        txtTimer.Text = timer.ToString();
                    });
                }
            }
        }

        private void lbLastestSongs_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UCHome.addListLastestSong(sender);
        }
    }
}
