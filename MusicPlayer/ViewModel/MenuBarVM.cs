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
    public class MenuBarVM : BaseViewModel
    {
        public ICommand handleLogOut { get; set; }

        public MenuBarVM()
        {
            handleLogOut = new RelayCommand<UserControl>((p) => { return p == null ? false : true; }, (p) => {
                FrameworkElement window = getWindow(p);
                Login login = new Login();
                login.Show();
                var w = window as Window;
                if (w != null)
                {
                    w.Close();
                }
            });
        }

        FrameworkElement getWindow(UserControl p)
        {
            FrameworkElement parent = p;
            while (parent.Parent != null)
            {
                parent = parent.Parent as FrameworkElement;
            }
            return parent;
        }
    }
}
