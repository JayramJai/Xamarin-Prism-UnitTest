namespace MovieReview.Model
{
    public class Content
    {
        private string year;

        public string Title { get; set; }
        public string Year { get => year; set => year = value; }
        public string ImdbID { get; set; }
        public string Type { get; set; }
        public string Poster { get; set; }
    }
}
