using MusicPlayer.Model;
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
    public class LoginViewModel:BaseViewModel
    {
        public bool IsLogin { get; set; } //dang nhap hay chua
        private string _username;
        public string Username { get { return _username; } set { _username = value; OnPropertyChanged(); } }
        private string _password;
        public string Password { get { return _password; } set { _password = value; OnPropertyChanged(); } }
        public ICommand LoginCommand { get; set; } //coi nhu ca dang nhap tha nh cong
        public ICommand ToSignUpCommand { get; set; }//dang ki thi vao form dang ki 
        public ICommand PasswordChangedCommand { get; set; }   
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
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return  true; }, (p) =>
            {
                Password  = p.Password;
            });
            void Login( Window p)
            {
                if (p == null) return;//chekc null
                else
                {
                    var count = DataProvider.Ins.DB.USERS.Where(u => u.USERNAME == Username && u.PASS == Password).Count();

                    if (count > 0)
                    {
                        IsLogin = true;
                        p.Close();
                    }
                    else
                    {
                        IsLogin = false;
                        MessageBox.Show("Tên đăng nhập hoặc mật khẩu bị sai!", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }       
                }
            }
        }
    }
}
