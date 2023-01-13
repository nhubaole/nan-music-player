﻿using System;
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
        public MainViewModel(){
            HomeCommand = new RelayCommand<object>((p) => { return true; }, Home);
            LibraryCommand = new RelayCommand<object>((p) => { return true; }, Library);
            ProfileCommand = new RelayCommand<object>((p) => { return true; }, Profile);
            SearchCommand = new RelayCommand<object>((p) => { return true; }, Search);
            CurrentView = new HomeVM();

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
                        p.Show();
                    }
                    else
                    {
                        p.Close();
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            });

             handleLogOutCommand = new RelayCommand<Window>((p) => { return p == null ? false : true; }, (p) => {
                if (MessageBox.Show("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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
        public ICommand handleLogOutCommand { get; set; }


        private void Home(object obj) => CurrentView = new HomeVM();
        private void Library(object obj) => CurrentView = new LibraryVM();
        private void Profile(object obj) => CurrentView = new ProfileVM();
        private void Search(object obj) => CurrentView = new SearchVM();

    }
}
