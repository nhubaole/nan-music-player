using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace MusicPlayer.ViewModel
{
    public class LoginViewModel:BaseViewModel
    {
        public static ObservableCollection<USER> listUser = new ObservableCollection<USER>(DataProvider.Ins.DB.USERS);
        public static USER currUser = null;
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
                    w.Close();
                    signUp.Show();
//note
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
                    if (Password != null && Username != null)
                    {
                        string pass = MD5Hash(Base64Encode(Password));
                        var count = DataProvider.Ins.DB.USERS.Where(u => u.USERNAME == Username && u.PASS == pass).Count();

                        if (count > 0)
                        {
                            IsLogin = true;
                            foreach (USER n in listUser)
                            {
                                if (n.USERNAME == Username)
                                {
                                    currUser = n;
                                    break;
                                }
                            }
                            p.Close();//
                        }
                        else
                        {
                            IsLogin = false;
                            currUser = null;
                            MessageBox.Show("Tên đăng nhập hoặc mật khẩu bị sai!", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }    
                    else
                    {
                        MessageBox.Show("Hãy điền đủ thông tin!", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                    }


                }
            }

        }
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
    }
}
