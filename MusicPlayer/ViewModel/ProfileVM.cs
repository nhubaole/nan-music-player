using MusicPlayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    internal class ProfileVM : BaseViewModel
    {

        private string _uFullName;
        public string uFullName { get => _uFullName; set { _uFullName = value; OnPropertyChanged(); } }


        private string _uEmail;
        public string uEmail { get => _uEmail; set { _uEmail = value; OnPropertyChanged(); } }

        private string _uPhone;
        public string uPhone { get => _uPhone; set { _uPhone = value; OnPropertyChanged(); } }

        private string _uDoB;
        public string uDoB { get => _uDoB; set { _uDoB = value; OnPropertyChanged(); } }

        private string _sex;
        public string Sex { get => _sex; set { _sex = value; OnPropertyChanged(); } }

        public int Change = 0;
        public string ErroMessage = null;

        public ICommand handleChangePasswordCommand { get; set; }
        public ICommand handleProfileCommand { get; set; }
        public ICommand SelectedDateChangedCommand { get; set; }
        public ICommand SSelectionChangedCommand { get; set; }

        public ProfileVM()
        {
            var user = LoginViewModel.currUser;
            uFullName = user.FULLNAME;
            uDoB = user.DOB.ToString();
            uPhone = user.PHONE;
            uEmail = user.EMAIL;
            Sex = user.SEX;

            handleChangePasswordCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                PasswordChange passwordChange = new PasswordChange();
                passwordChange.ShowDialog();
            });
            handleProfileCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                try 
                {


                    
                    if (uFullName == user.FULLNAME && DateTime.Parse(uDoB) == user.DOB && uPhone == user.PHONE && uEmail == user.EMAIL && Sex == user.SEX)
                    {
                        MessageBox.Show("Hãy nhập các thông tin bạn cần thay đổi!", "Thông báo", MessageBoxButton.OK);

                    }
                    else
                    if (CheckAllInfor())
                    {
                        {
                            user.FULLNAME = uFullName;
                            user.DOB = DateTime.Parse(uDoB);
                            user.PHONE = uPhone;
                            user.EMAIL = uEmail;
                            user.SEX = Sex;
                        };
                        DataProvider.Ins.DB.SaveChanges();
                        MessageBox.Show("Thay đổi thông tin thành công!", "Thông báo", MessageBoxButton.OK);

                    }
                    else
                    {
                        MessageBox.Show(ErroMessage, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);//
                        ErroMessage = null;
                    }
                }
            
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });

            //bool flag = true;
            SelectedDateChangedCommand = new RelayCommand<DatePicker>((p) => { return true; }, (p) =>
            {

                try
                {
                    Change++;
                    DateTime? selectedDate = p.SelectedDate;
                    if (selectedDate.HasValue )
                    {
                       
                        uDoB = selectedDate.Value.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
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
                    Change++;
                    if (p.SelectedValue.ToString() == "System.Windows.Controls.ComboBoxItem: Nam")
                    {
                        Sex = "Nam";
                    }
                    else
                    {
                        Sex = "Nữ";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });
        }
        public bool CheckChange (USER u)
        {
            if ( uFullName == u.FULLNAME && DateTime.Parse(uDoB) == u.DOB && uEmail == u.EMAIL && Sex == u.SEX)
            {
                return false;
            }    
            return true;
        }
        public bool CheckAllInfor()
        {
            if (  AllIsChar(uFullName) && CheckDateOfBirth(DateTime.Parse(uDoB)) && Sex != null)
            {
                int erro = 0;
                if (uEmail != null && uEmail != "")
                {
                    if (!CheckMail(uEmail))
                    {
                        erro++;
                        ErroMessage += "Hãy kiểm tra lại Email của bạn.\n";
                    }

                }
                if (uPhone != null && uPhone != "")
                {
                    if (!CheckPhoneNum(uPhone))
                    {
                        ErroMessage += "Số điện thoại của bạn không hợp lệ.\n";
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
                
                if (!AllIsChar(uFullName))
                {
                    ErroMessage += "Hãy kiểm tra lại họ tên của bạn.\n";

                }


                if (uDoB == null || (!CheckDateOfBirth(DateTime.Parse(uDoB))))
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
        public bool AllIsChar(string s)// note
        {
            //nt Erro = 0;
            if (!string.IsNullOrEmpty(s))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    char c = s[i];
                    //La so, la control, la cham cau
                    if (char.IsDigit(c) || char.IsControl(c) || IsSpecialChar(c))
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
        public bool IsSpecialChar(char s)
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

        public bool AllIsDigit(string s)
        {
            //int Erro = 0;
            if (!string.IsNullOrEmpty(s))
            {
                for (int i = 0; i < s.Length; i++)
                {
                    //khong la so thi false
                    if (!char.IsDigit(s[i]))
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
       
        public bool CheckPhoneNum(string number)
        {
            if ((number == null) || (number.Length != 10) || number[0] != '0')
            {

                return false;

            }
            else
            {
                return (AllIsDigit(number));
            }
        }
        public bool CheckMail(string mail)
        {
            Regex regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
            return regex.IsMatch(mail);
        }

        public bool CheckDateOfBirth(DateTime date)
        {
            if (date == null)
            {
                return false;
            }
            DateTime now = DateTime.Now;
            if (DateTime.Compare(now, date) < 0)
            {

                return false;
            }
            else
                return true;
        }
    }
}
