using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    public class PlayMusicVM
    {
        public ICommand ToInforCommand { get; set; }

        public PlayMusicVM()
        {
            ToInforCommand = new RelayCommand<Window>((p) => { return true; }, (p) =>
            {
                Infor infor = new Infor();
                infor.ShowDialog();
            });
        }
    }
}
