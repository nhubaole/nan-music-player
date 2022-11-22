using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    internal class SignUpVM
    {
        public ICommand handleSignUpCommand { get; set; }
        public ICommand closeSignUpCommand { get; set; }
        public SignUpVM()
        {
            handleSignUpCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                var w = p as Window;
                if (w != null)
                {
                    MessageBox.Show("Đăng ký thành công!");
                    Login login = new Login();
                    login.Show();
                    w.Close();
                }
            });

            closeSignUpCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    var w = p as Window;
                    if (w != null)
                    {
                        Login login = new Login();
                        login.Show();
                        w.Close();
                    }
                }
            });
        }
    }
}
