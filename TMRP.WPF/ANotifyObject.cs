using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TMRP.WPF
{
    public abstract class ANotifyObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void Set<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (field == null && value != null || field != null && !field.Equals(value))
            {
                field = value;
                NotifyChanged(propertyName);
            }
        }

        public void RefreshBindings()
        {
            NotifyChanged("");
        }

        protected void NotifyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
