using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MusicPlayer
{
    internal class CustomMessageBox
    {
        public static MessageBoxResult Show(string message, MessageBoxImage image)
        {
            CustomMessageBoxWindow msg = new CustomMessageBoxWindow(message, image);
            msg.ShowDialog();

            return msg.Result;
        }
    }
}
