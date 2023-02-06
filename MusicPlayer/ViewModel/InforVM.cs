using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer.ViewModel 
{
    public class InforVM: BaseViewModel
    {
        public ICommand ToMainCommand { get; set; }
        public InforVM()
        {
            ToMainCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
           {
               if (CustomMessageBox.Show("Bạn có chắc chắn muốn thoát?", MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
               {
                   var w = p as Window;
                   if (w != null)
                   {     
                       w.Close();
                   }
               }
           });
        }
    }
}
