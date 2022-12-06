using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
namespace MusicPlayer.ViewModel
{
     public class LibraryVM : BaseViewModel
    {
        public ICommand ToUploadCommand { get; set; }
        public LibraryVM()
        {
            ToUploadCommand = new RelayCommand<object>((p) => { return true; }, (p) =>
            {
                Upload upload = new Upload();
                upload.ShowDialog();
            });
        }
    }
}
