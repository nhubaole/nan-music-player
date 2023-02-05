using MusicPlayer.UserControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MusicPlayer.ViewModel
{
    public class MainViewModel : BaseViewModel
    {
        private object _currentView;
        private Visibility _isVisible = Visibility.Visible;
        public Visibility IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
                OnPropertyChanged("IsVisible");
            }
        }
        public MainViewModel(){
            HomeCommand = new RelayCommand<object>((p) => { return true; }, Home);
            LibraryCommand = new RelayCommand<object>((p) => { return true; }, Library);
            ProfileCommand = new RelayCommand<object>((p) => { return true; }, Profile);
            SearchCommand = new RelayCommand<object>((p) => { return true; }, Search);
            VideoCommand = new RelayCommand<object>((p) => { return true; }, Video);
            

            LoadedWindowCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                try
                {
                    IsLoaded = true;
                    if (p == null) return;
                    p.Hide();

                    Login login = new Login();
                    login.ShowDialog();

                    if (login.DataContext == null) return;
                    var loginVM = login.DataContext as LoginViewModel;
                    if (loginVM.IsLogin == true)
                    {
                        CurrentView = new HomeVM();
                        UCHome.reset = 1;
                        UCPlayMusic.init = 0;
                        p.Show();
                    }
                    else
                    {
                        p.Close();
                    }

                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show(ex.Message, MessageBoxImage.Error);
                }
            });

             handleLogOutCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                if (CustomMessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                     var w = p as Window;
                     if (w != null)
                     {
                         MainWindow main = new MainWindow();
                         LoginViewModel.currUser = null;
                         w.Close();
                         main.Show();

                     }
                 }
            });
   
        }

        public object CurrentView
        {
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }
        public bool IsLoaded  = false;
        public ICommand LoadedWindowCommand { get; set; }

        public ICommand HomeCommand { get; set; }
        public ICommand LibraryCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand SearchCommand { get; set; }
        public ICommand VideoCommand { get; set; }
        public ICommand handleLogOutCommand { get; set; }


        private void Home(object obj)
        {
            CurrentView = new HomeVM();
            IsVisible = Visibility.Visible;
        }
        private void Library(object obj)
        {
            CurrentView = new LibraryVM();
            IsVisible = Visibility.Visible;
        }
        private void Profile(object obj)
        {
            CurrentView = new ProfileVM();
            IsVisible = Visibility.Visible;
        }
        private void Search(object obj)
        {
            CurrentView = new SearchVM();
            IsVisible = Visibility.Visible;
        }
        private void Video(object obj)
        {
            CurrentView = new VideoVM();
            IsVisible = Visibility.Hidden;
        }

    }
}
