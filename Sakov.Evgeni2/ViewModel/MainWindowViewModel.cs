using Sakov_Evgeni2.Commands.CommandForMainWindow;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Sakov_Evgeni2.ViewModel
{
    class MainWindowViewModel : INotifyPropertyChanged
    {
        public static MainWindowViewModel mainwindow;
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            mainwindow = this;
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

        private InputLinkToVKCommand Input_Click_Command;
        public InputLinkToVKCommand Input_Click
        {
            get
            {
                return Input_Click_Command ??
                  (Input_Click_Command = new InputLinkToVKCommand(obj => { }));
            }
        }
        public static string LinkTovk;
        public static string InfoForLink;
        public string LinkToVK
        {
            get { return LinkTovk; }
            set
            {
                LinkTovk = value;
                OnPropertyChanged("LinkToVK");
            }
        }
        public string Info
        {
            get { return InfoForLink; }
            set
            {
                InfoForLink = value;
                OnPropertyChanged("Info");
            }
        }
    }
}
