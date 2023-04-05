using Prism.Mvvm;

namespace MovieReview.Model
{
    public class FullContent : BindableBase
    {
        private string _title;
        public string Title
        {
            get { return _title; }
            set
            {
                _title = value;
                RaisePropertyChanged(nameof(Title));
            }
        }
        private string _year;
        public string Year
        {
            get { return _year; }
            set
            {
                _year = value;
                RaisePropertyChanged(nameof(Year));
            }
        }
        private string _plot;
        public string Plot
        {
            get { return _plot; }
            set
            {
                _plot = value;
                RaisePropertyChanged(nameof(Plot));
            }
        }
        private string _genre;
        public string Genre
        {
            get { return _genre; }
            set
            {
                _genre = value;
                RaisePropertyChanged(nameof(Genre));
            }
        }
        private string _runtime;
        public string Runtime
        {
            get { return _runtime; }
            set
            {
                _runtime = value;
                RaisePropertyChanged(nameof(Runtime));
            }
        }
        private string _poster;
        public string Poster
        {
            get { return _poster; }
            set
            {
                _poster = value;
                RaisePropertyChanged(nameof(Poster));
            }
        }
        private string _imdbID;
        public string ImdbID
        {
            get { return _imdbID; }
            set
            {
                _imdbID = value;
                RaisePropertyChanged(nameof(ImdbID));
            }
        }
    }
}
