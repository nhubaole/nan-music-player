using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    internal class PasswordChangeVM : BaseViewModel
    {
        private string _uCurPassword;
        public string uCurPassword { get => _uCurPassword; set { _uCurPassword = value; OnPropertyChanged(); } }

        private string _uNePassword;
        public string uNePassword { get => _uNePassword; set { _uNePassword = value; OnPropertyChanged(); } }

        private string _uRePassword;
        public string uRePassword { get => _uRePassword; set { _uRePassword = value; OnPropertyChanged(); } }
        public ICommand savePasswordCommand { get; set; }
        public ICommand closePasswordChangeCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }
        public ICommand CurPasswordCommand { get; set; }
        public string tempPass = null;
        public string erro = null;
        public PasswordChangeVM()
        {
            CurPasswordCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                try
                {
                    string pass = MD5Hash(Base64Encode(p.Password));
                    uCurPassword = pass;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                
                try
                {
                    
                    uNePassword = p.Password;
                 

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                try
                {
                    //string repass = MD5Hash(Base64Encode(p.Password));
                    uRePassword = p.Password;
                    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            savePasswordCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
            {
                var w = p as Window;
                if (w != null)
                {
                   if (uCurPassword != null && uNePassword != null && uRePassword !=null)
                    {
                        if (CheckCurPass(uCurPassword))//mat khau hien tai khop
                        {

                            if (CheckLengthOPass(uNePassword) && CheckLengthOPass(uRePassword) && CheckPass(uNePassword, uRePassword))//mk mới khớp mk re
                            {
                                string NewPass = MD5Hash(Base64Encode(uNePassword));

                                var user = LoginViewModel.currUser;
                                {
                                    user.PASS = NewPass;
                                };

                                DataProvider.Ins.DB.SaveChanges();
                                MessageBox.Show("Đổi mật khẩu thành công!","Thông báo");
                                w.Close();
                            }
                            else
                            {
                                MessageBox.Show("Hãy kiểm tra lại mật khẩu mới và xác nhận lại mật khẩu!", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                            }
                        }
                        else
                        {
                            MessageBox.Show("Mật khẩu hiện tại sai!", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }   
                   else
                    {
                        MessageBox.Show("Hãy nhập đủ các thông tin có dấu (*)!", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
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
        public bool CheckCurPass(string pass)//kiem tra mat khau hien tai
        {


            if (uCurPassword != null)
            {
               
                if (pass == LoginViewModel.currUser.PASS)
                {

                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Hãy nhập mật khẩu hiện tại của bạn để xác nhận!", "Đã xảy ra lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;

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

        public bool CheckLengthOPass(string tempPass)
        {
            if (tempPass.Length < 6)
            {
                return false;
            }
            return true;
        }

        public bool CheckPass(string Pass, string Repass)
        {
            if (string.IsNullOrEmpty(Pass) || string.IsNullOrEmpty(Repass))
            {
                return false;
            }
            else
            {
                if (string.Compare(Pass, Repass) == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
