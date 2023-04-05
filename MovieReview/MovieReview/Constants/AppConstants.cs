using System.Diagnostics.CodeAnalysis;

namespace MovieReview.Constants
{
    [ExcludeFromCodeCoverage]
    public static class AppConstants
    {
        public static string BaseUrl = "https://www.omdbapi.com";
        public static string FindByName = "?apikey={0}&s={1}&r=json&page=1";
        public static string FindByID = "?apikey={0}&i={1}&plot=full";
        public static string ApiKey = "bc78f62c";
    }
}
