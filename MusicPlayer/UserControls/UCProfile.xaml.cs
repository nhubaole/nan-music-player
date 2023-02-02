using MusicPlayer.Model;
using MusicPlayer.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MusicPlayer.UserControls
{
    /// <summary>
    /// Interaction logic for UCProfile.xaml
    /// </summary>
    public partial class UCProfile : UserControl
    {
        public UCProfile()
        {
            InitializeComponent();
            USER current = LoginViewModel.currUser;
            if(current != null)
            {
                txtUserName.Text = current.USERNAME;
                txtFullName.Text = current.FULLNAME;
                txtPhone.Text = current.PHONE;
                txtEmail.Text = current.EMAIL;
                dpDOB.SelectedDate = Convert.ToDateTime(current.DOB.ToString().Substring(0, 10));
                cmbSex.Text = current.SEX == "Nam" ? "Nam" : "Nữ";

            }
        }

    }
}
