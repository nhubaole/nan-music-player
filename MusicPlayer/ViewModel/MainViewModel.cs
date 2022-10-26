using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        }

        public object CurrentView { 
            get => _currentView;
            set { _currentView = value; OnPropertyChanged(); }
        }

        public ICommand HomeCommand { get; set; }
        public ICommand LibraryCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand SearchCommand { get; set; }

        private void Home(object obj) => CurrentView = new HomeVM();
        private void Library(object obj) => CurrentView = new LibraryVM();
        private void Profile(object obj) => CurrentView = new ProfileVM();
        private void Search(object obj) => CurrentView = new SearchVM();

    }
}
