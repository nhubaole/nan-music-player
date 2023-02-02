﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    public class UploadVM: BaseViewModel
    {
        public ICommand CloseUploadSongCommand { get; set; }
        public UploadVM()
        {
            CloseUploadSongCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) =>
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
    }
}
