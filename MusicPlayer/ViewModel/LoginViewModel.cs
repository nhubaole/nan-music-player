using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace MusicPlayer.ViewModel
{
    public class LoginViewModel:BaseViewModel
    {
        public ICommand handleLoginCommand_1 { get; set; }//dang nhap thi vao MainWindow
        public ICommand handleLoginCommand_2 { get; set; }//dang ki thi vao form dang ki 
        public LoginViewModel()
        {
            handleLoginCommand_1 = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = p as Window;
                if (w != null)
                {
                    MainWindow mainWindow = new MainWindow();
                    mainWindow.Show();
                    w.Close();
                }
            });
            handleLoginCommand_2 = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = p as Window;
                if (w != null)
                {
                    SignUp signUp = new SignUp();
                    signUp.Show();
                    w.Close();
                }
            });
        }
    }
}
