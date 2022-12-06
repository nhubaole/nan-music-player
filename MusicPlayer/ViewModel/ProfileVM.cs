using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    internal class ProfileVM : BaseViewModel
    {
        public ICommand handleChangePasswordCommand { get; set; }
        public ICommand handleProfileCommand { get; set; }
        public ProfileVM()
        {
            handleChangePasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                PasswordChange passwordChange = new PasswordChange();
                passwordChange.ShowDialog();
            });
            handleProfileCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {


            });
        }
    }
}
