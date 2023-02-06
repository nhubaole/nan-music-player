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
        private string _uFullName;
        public string uFullName { get => _uFullName; set { _uFullName = value; OnPropertyChanged(); } }

        private string _uName;
        public string uName { get => _uName; set { _uName = value; OnPropertyChanged(); } }

        private string _uPassword;
        public string uPassword { get => _uPassword; set { _uPassword = value; OnPropertyChanged(); } }

        private string _uRePassword;
        public string uRePassword { get => _uRePassword; set { _uRePassword = value; OnPropertyChanged(); } }
        
        private string _uEmail;
        public string uEmail { get => _uEmail; set { _uEmail = value; OnPropertyChanged(); } }

        private string _uPhone;
        public string uPhone { get => _uPhone; set { _uPhone = value; OnPropertyChanged(); } }

        private string _uDoB;
        public string uDoB { get => _uDoB; set { _uDoB = value; OnPropertyChanged(); } }

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
                    uPassword = pass;

                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, MessageBoxImage.Error);
                }
            });
            RePasswordChangedCommand = new RelayCommand<PasswordBox>((p) => { return true; }, (p) =>
            {
                try
                {
                    if (p.Password.Length >=6)
                    {
                        string repass = MD5Hash(Base64Encode(p.Password));
                        uRePassword = repass;
                    }    

                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, MessageBoxImage.Error);
                }
            });
   
            SelectedDateChangedCommand = new RelayCommand<DatePicker>((p) => { return true; }, (p) =>
            {
               try
                {
                    DateTime? selectedDate = p.SelectedDate;
                    if (selectedDate.HasValue )
                    {

                        uDoB = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, MessageBoxImage.Error);
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
                    CustomMessageBox.Show(ex.Message, MessageBoxImage.Error);
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
                            USERNAME = uName,
                            FULLNAME = uFullName,
                            PASS = uPassword,
                            DOB = DateTime.Parse(uDoB),
                            PHONE = uPhone,
                            EMAIL = uEmail,
                            SEX = Sex
                        };
                        DataProvider.Ins.DB.USERS.Add(user);
                        DataProvider.Ins.DB.SaveChanges();
                        CustomMessageBox.Show("Đăng ký thành công!", MessageBoxImage.None);
                        MainWindow main = new MainWindow();
                        w.Close();
                        main.Show();

                    }
                    else
                    {
                        CustomMessageBox.Show(ErroMessage + "Hãy chắc chắn bạn đã nhập đủ các thông tin có đánh dấu *. Kiểm tra lại các thông tin nhé.", MessageBoxImage.Error);//
                        ErroMessage = null;
                    }
                }catch(Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, MessageBoxImage.Error);
                }

            });
            closeSignUpCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                try
                {
                    if (CustomMessageBox.Show("Bạn có chắc chắn muốn thoát?", MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                    {
                        var w = p as Window;
                        if (w != null)
                        {
                            MainWindow main = new MainWindow();
                            w.Close();
                            main.Show();
                        }
                    }

                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, MessageBoxImage.Error);
                }
            });
        }
        public bool CheckAllInfor ()
        {
            if (CheckNewUser(uName) && AllIsChar(uFullName) && CheckLengthOPass(tempPass) && CheckPass(uPassword, uRePassword) && CheckDateOfBirth(DateTime.Parse(uDoB)) && Sex != null)
            {
                 int erro = 0;
                if (uEmail != null && uEmail !="")
                {
                    if (!CheckMail(uEmail))
                    {
                        erro++;
                        ErroMessage += "Hãy kiểm tra lại Email cả bạn.\n";
                    }

                }
                if (uPhone != null && uPhone !="")
                {
                    if (!CheckPhoneNum(uPhone))
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
                if (!CheckNewUser(uName))
                {
                    ErroMessage += "Hãy kiểm tra lại tên đăng nhập. Có thể nó bị trùng.\n";
                }
                if (!AllIsChar(uFullName))
                {
                    ErroMessage += "Hãy kiểm tra lại họ tên của bạn.\n";

                }
                if (!CheckLengthOPass(tempPass))
                {
                    ErroMessage += "Hãy nhập lại mật khẩu.\n";
                }
                else
                {
                    if (!CheckPass(uPassword, uRePassword))
                    {
                        ErroMessage += "Hãy xác nhận lại mật khẩu\n";

                    }
                } 
                if ( uDoB == null ||  (!CheckDateOfBirth(DateTime.Parse(uDoB))))
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
            if (DateTime.Compare(now, date) < 0)
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
