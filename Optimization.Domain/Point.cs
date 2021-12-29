using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Optimus.Domain
{
    // Implementing INotifyPropertyChanged - does a better way exist?
    // https://stackoverflow.com/questions/1315621/implementing-inotifypropertychanged-does-a-better-way-exist

    internal class Point : INotifyPropertyChanged
    {
        private double _value;

        public Point(double value)
        {
            this.Value = value;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public double Value
        {
            get => _value;
            set => SetField(ref _value, value);
        }
    }
}