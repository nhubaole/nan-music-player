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
        public bool IsLogin { get; set; } //dang nhap hay chua
        public ICommand LoginCommand { get; set; } //coi nhu ca dang nhap tha nh cong
        public ICommand ToSignUpCommand { get; set; }//dang ki thi vao form dang ki 
        public LoginViewModel()
        {
            IsLogin = false;//coi nhu chua dang nhap
            LoginCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                Login(p);
            });

            ToSignUpCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = p as Window;
                if (w != null)
                {
                    SignUp signUp = new SignUp();
                    signUp.Show();
                    w.Close();
                }
            });
            void Login( Window p)
            {
                if (p == null) return;//chekc null
                else
                {
                    IsLogin = true;
                    p.Close();
                }
            }
        }
    }
}
