using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace MusicPlayer
{
    /// <summary>
    /// Interaction logic for CustomMessageBoxWindow.xaml
    /// </summary>
    public partial class CustomMessageBoxWindow : Window
    {
        public MessageBoxResult Result { get; set; }

        public string Message
        {
            get
            {
                return DescriptionText.Text;
            }
            set
            {
                DescriptionText.Text = value;
            }
        }

        public CustomMessageBoxWindow(string message, MessageBoxImage image)
        {
            InitializeComponent();

            Message = message;
            Display(image);
        }

        public void Display(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Exclamation:
                    {
                        SuccessIcon.Visibility = Visibility.Collapsed;
                        ErrorIcon.Visibility = Visibility.Collapsed;
                        InfoIcon.Visibility = Visibility.Collapsed;
                        ConfirmIcon.Visibility = Visibility.Visible;

                        SuccessText.Visibility = Visibility.Collapsed;
                        ErrorText.Visibility = Visibility.Collapsed;
                        InfoText.Visibility = Visibility.Collapsed;
                        ConfirmText.Visibility = Visibility.Visible;

                        ContinueBtn.Visibility = Visibility.Collapsed;
                        AgainBtn.Visibility = Visibility.Collapsed;
                        YesBtn.Visibility = Visibility.Visible;
                        NoBtn.Visibility = Visibility.Visible;
                        OKBtn.Visibility = Visibility.Collapsed;
                        break;
                    }       
                case MessageBoxImage.Error:
                    {
                        SuccessIcon.Visibility = Visibility.Collapsed;
                        ErrorIcon.Visibility = Visibility.Visible;
                        InfoIcon.Visibility = Visibility.Collapsed;
                        ConfirmIcon.Visibility = Visibility.Collapsed;

                        SuccessText.Visibility = Visibility.Collapsed;
                        ErrorText.Visibility = Visibility.Visible;
                        InfoText.Visibility = Visibility.Collapsed;
                        ConfirmText.Visibility = Visibility.Collapsed;

                        ContinueBtn.Visibility = Visibility.Collapsed;
                        AgainBtn.Visibility = Visibility.Visible;
                        YesBtn.Visibility = Visibility.Collapsed;
                        NoBtn.Visibility = Visibility.Collapsed;
                        OKBtn.Visibility = Visibility.Collapsed;
                        break;
                    }             
                case MessageBoxImage.Information: //Info
                    {
                        SuccessIcon.Visibility = Visibility.Collapsed;
                        ErrorIcon.Visibility = Visibility.Collapsed;
                        InfoIcon.Visibility = Visibility.Visible;
                        ConfirmIcon.Visibility = Visibility.Collapsed;

                        SuccessText.Visibility = Visibility.Collapsed;
                        ErrorText.Visibility = Visibility.Collapsed;
                        InfoText.Visibility = Visibility.Visible;
                        ConfirmText.Visibility = Visibility.Collapsed;

                        ContinueBtn.Visibility = Visibility.Collapsed;
                        AgainBtn.Visibility = Visibility.Collapsed;
                        YesBtn.Visibility = Visibility.Collapsed;
                        NoBtn.Visibility = Visibility.Collapsed;
                        OKBtn.Visibility = Visibility.Visible;
                        break;
                    }
                default: //success
                    {
                        SuccessIcon.Visibility = Visibility.Visible;
                        ErrorIcon.Visibility = Visibility.Collapsed;
                        InfoIcon.Visibility = Visibility.Collapsed;
                        ConfirmIcon.Visibility = Visibility.Collapsed;

                        SuccessText.Visibility = Visibility.Visible;
                        ErrorText.Visibility = Visibility.Collapsed;
                        InfoText.Visibility = Visibility.Collapsed;
                        ConfirmText.Visibility = Visibility.Collapsed;

                        ContinueBtn.Visibility = Visibility.Visible;
                        AgainBtn.Visibility = Visibility.Collapsed;
                        YesBtn.Visibility = Visibility.Collapsed;
                        NoBtn.Visibility = Visibility.Collapsed;
                        OKBtn.Visibility = Visibility.Collapsed;
                        break;
                    }
            }
        }

        private void ContinueBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void AgainBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        private void OKBtn_Click(object sender, RoutedEventArgs e)
        {
            Result = MessageBoxResult.OK;
            Close();
        }
    }
}
