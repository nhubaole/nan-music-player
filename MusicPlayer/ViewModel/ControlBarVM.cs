using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    internal class ControlBarVM : BaseViewModel
    {
        public ICommand CloseCommand { get; set; }
        public ICommand MinimizedCommand { get; set; }

        public ControlBarVM()
        {
            CloseCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true;  }, (p) => {
                FrameworkElement window = getWindow(p);
                var w = window as Window;
                if(w != null)
                {
                    w.Close();
                }
            });

            MinimizedCommand = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = getWindow(p);
                var w = window as Window;
                if (w != null)
                {
                    if(w.WindowState != WindowState.Minimized)
                    {
                        w.WindowState = WindowState.Minimized;
                    }
                }
            });
        }

        FrameworkElement getWindow(UserControl p)
        {
            FrameworkElement parent = p;
            while(parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
