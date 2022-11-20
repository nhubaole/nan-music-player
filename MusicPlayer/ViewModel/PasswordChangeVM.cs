using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    internal class PasswordChangeVM : BaseViewModel
    {
        public ICommand savePasswordCommand { get; set; }
        public ICommand closePasswordChangeCommand { get; set; }
        public PasswordChangeVM()
        {
            savePasswordCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = p as Window;
                if (w != null)
                {
                    MessageBox.Show("Đổi mật khẩu thành công!");
                    w.Close();
                }
            });

            closePasswordChangeCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
