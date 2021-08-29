using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Notes.ViewModels
{
   public class ObservableBase:INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string prop = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
