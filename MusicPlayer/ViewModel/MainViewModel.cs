using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        public MainViewModel()
        {
           Upload up = new Upload();    
            up.ShowDialog();    
        }
    }
}
