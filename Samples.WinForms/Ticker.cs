namespace Samples.WinForms {
    using System.ComponentModel;
    using System.Runtime.CompilerServices;

    internal sealed class Ticker : INotifyPropertyChanged
    {
        public string Id { get; set; }

        private decimal _price;
        public decimal Price
        {
            get => _price;
            set
            {
                if (value.Equals(_price)) { return; }

                _price = value;
                NotifyPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}