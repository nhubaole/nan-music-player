using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    internal class SignUpVM: BaseViewModel
    {
        private string _uFullNameSU;
        public string uFullNameSU { get => _uFullNameSU; set { _uFullNameSU = value; OnPropertyChanged(); } }

        private string _uNameSU;
        public string uNameSU { get => _uNameSU; set { _uNameSU = value; OnPropertyChanged(); } }

        private string _uPasswordSU;
        public string uPasswordSU { get => _uPasswordSU; set { _uPasswordSU = value; OnPropertyChanged(); } }

        private string _uRePasswordSU;
        public string uRePasswordSU { get => _uRePasswordSU; set { _uRePasswordSU = value; OnPropertyChanged(); } }
        
        private string _uEmailSU;
        public string uEmailSU { get => _uEmailSU; set { _uEmailSU = value; OnPropertyChanged(); } }

        private string _uPhoneSU;
        public string uPhoneSU { get => _uPhoneSU; set { _uPhoneSU = value; OnPropertyChanged(); } }

        private string _uDoBSU;
        public string uDoBSU { get => _uDoBSU; set { _uDoBSU = value; OnPropertyChanged(); } }

        private string _sex;
        public string Sex { get => _sex; set { _sex = value; OnPropertyChanged();} }

        public string ErroMessage = null;
        public string tempPass = null;



        public ICommand handleSignUpCommand { get; set; }
        public ICommand closeSignUpCommand { get; set; }
        public ICommand PasswordChangedCommand { get; set; }
        public ICommand RePasswordChangedCommand { get; set; }
        public ICommand SelectedDateChangedCommand { get; set; }
        public ICommand SSelectionChangedCommand { get; set; }




        public SignUpVM()
        {
            PasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {

                try
                {
                    tempPass = p.Password;
                    string pass = MD5Hash(Base64Encode(p.Password));
                    uPasswordSU = pass;

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
                    if (p.Password.Length >=6)
                    {
                        string repass = MD5Hash(Base64Encode(p.Password));
                        uRePasswordSU = repass;
                    }    

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            bool flag = true;
            SelectedDateChangedCommand = new RelayCommand<DatePicker>((p) => { return true; }, (p) =>
            {
                
               try
                {
                    DateTime? selectedDate = p.SelectedDate;
                    if (selectedDate.HasValue && flag )
                    {
                        flag = false;
                        uDoBSU = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            SSelectionChangedCommand = new RelayCommand<ComboBox>((p) => { return true; }, (p) =>
            {
                try
                {
                    if (p.SelectedValue.ToString() == "System.Windows.Controls.ComboBoxItem: Nam")
                    {
                        Sex = "Nam";
                    }
                    else
                    {
                        Sex = "Nữ";
                    }
                }catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
            handleSignUpCommand = new RelayCommand<Window>((p) => {return p == null ? false : true;  }, (p) => {
                try
                {
                    var w = p as Window;

                    if (CheckAllInfor())
                    {
                        var user = new USER()
                        {
                            USERNAME = uNameSU,
                            FULLNAME = uFullNameSU,
                            PASS = uPasswordSU,
                            DOB = DateTime.Parse(uDoBSU),
                            PHONE = uPhoneSU,
                            EMAIL = uEmailSU,
                            SEX = Sex
                        };
                        DataProvider.Ins.DB.USERS.Add(user);
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButton.OK);
                        MainWindow main = new MainWindow();
                        w.Close();
                        main.Show();
                        //Login login = new Login();
                        //login.Show();
                    }
                    else
                    {
                        MessageBox.Show(ErroMessage + "Hãy chắc chắn bạn đã nhập đủ các thông tin có đánh dấu *. Kiểm tra lại các thông tin nhé.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);//
                        ErroMessage = null;
                    }
                }catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });
            closeSignUpCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                try
                {
                    if (MessageBox.Show("Bạn có chắc chắn muốn thoát?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        var w = p as Window;
                        if (w != null)
                        {
                            MainWindow main = new MainWindow();
                            w.Close();
                            main.Show();
                            //Login login = new Login();
                            //login.Show();
                            //w.Close();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        public bool CheckAllInfor ()
        {
            if (CheckNewUser(uNameSU) && AllIsChar(uFullNameSU) && CheckLengthOPass(tempPass) && CheckPass(uPasswordSU, uRePasswordSU) && CheckDateOfBirth(DateTime.Parse(uDoBSU)) && Sex != null)
            {
                 int erro = 0;
                if (uEmailSU != null && uEmailSU !="")
                {
                    if (!CheckMail(uEmailSU))
                    {
                        erro++;
                        ErroMessage += "Hãy kiểm tra lại Email cả bạn.\n";
                    }

                }
                if (uPhoneSU != null && uEmailSU !="")
                {
                    if (!CheckPhoneNum(uPhoneSU))
                    {
                        ErroMessage += "Số điện thoại của bạn k hợp lệ.\n";
                        erro++;
                    }

                }
                if (erro != 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (!CheckNewUser(uNameSU))
                {
                    ErroMessage += "Hãy kiểm tra lại tên đăng nhập. Có thể nó bị trùng.\n";
                }
                if (!AllIsChar(uFullNameSU))
                {
                    ErroMessage += "Hãy kiểm tra lại họ tên của bạn.\n";

                }
                if (!CheckLengthOPass(tempPass))
                {
                    ErroMessage += "Hãy nhập lại mật khẩu.\n";
                }
                else
                {
                    if (!CheckPass(uPasswordSU, uRePasswordSU))
                    {
                        ErroMessage += "Hãy xác nhận lại mật khẩu\n";

                    }
                } 
                if ( uDoBSU == null ||  (!CheckDateOfBirth(DateTime.Parse(uDoBSU))))
                {
                    ErroMessage += "Hãy kiểm tra lại ngày sinh của bạn.\n";

                }
                if (Sex == null)
                {
                    ErroMessage += "Hãy kiểm tra lại mục giới tính.\n";

                }

                return false;
            }
        }
        public bool AllIsChar (string s)// note
        {
            //nt Erro = 0;
            if (!string.IsNullOrEmpty(s))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    //La so, la control, la cham cau
                    if (char.IsDigit(c) || char.IsControl(c)|| IsSpecialChar (c) )
                    {
                        return false;
                    }
                    
                }
                return true;
            }
            else
            {
                return false;
            }    
          
        }
        public bool IsSpecialChar (char s)
        {
            string specialchar = "|!#$%^&*()+_-?<>@~{}:',;.=+[]\"" + @"\";
            if (specialchar.Contains(s))
            {
                return true;
            }
            else return false;
        }
        public bool CheckLengthOPass(string tempPass)
        {
            if (tempPass.Length <6)
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

        public bool AllIsDigit (string s)
        {
            //int Erro = 0;
            if (!string.IsNullOrEmpty(s))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    //khong la so thi false
                    if (!char.IsDigit(s[i]) )
                    {
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }

        }
        public bool CheckNewUser (string uName)// xac dinh xem ten dang nhap co bi trung hay k

        {
            if (!string.IsNullOrEmpty(uName) && !uName.Contains(" "))//check k co dau cach
            {
                var count = DataProvider.Ins.DB.USERS.Where(u => u.USERNAME == uName).Count();
                if (count > 0)
                {
                   
                    return false;
                }  
                else
                {
                    return true;
                }     
            }
            else
            {
               
                return false;
            }    
        }
        public bool CheckPhoneNum(string number)
        {
            if ((number == null) || (number.Length != 10) || number[0]!= '0')
            {

                return false;

            }
            else
            {
                return (AllIsDigit(number));
            }
        }
        public bool CheckMail (string mail)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(mail);
        }

        public bool CheckDateOfBirth (DateTime date)
        {
            if (date == null)
            {
                return false ;
            }    
            DateTime now = DateTime.Now;
            if (DateTime.Compare(date, now) >= 0)
            {
        
                return false;
            }
            else
                return true;
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
