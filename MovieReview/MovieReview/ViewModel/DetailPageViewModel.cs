using Acr.UserDialogs;
using MovieReview.Model;
using MovieReview.Services.Interfaces;
using Prism.Navigation;
using System;

namespace MovieReview.ViewModel
{
    public class DetailPageViewModel : ViewModelBase
    {
        #region private_fields
        private readonly IContentService _contentService;
        private readonly IUserDialogs _userDialogsService;
        private FullContent _fullContent;
        #endregion

        #region properties
        public FullContent FullContent { get => _fullContent; set => SetProperty(ref _fullContent, value); }

        #endregion

        #region constructor
        public DetailPageViewModel(IContentService contentService, INavigationService navigationService, IUserDialogs userDialogsSerivce) : base(navigationService)
        {
            _contentService = contentService;
            _userDialogsService = userDialogsSerivce;
        }
        #endregion

        #region public_methods
        public bool ValidataKey(INavigationParameters parameters)
        {
            if(parameters?.ContainsKey("MovieID") == true)
            {
                return true;
            }

            return false;
        }
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            try
            {
                if (ValidataKey(parameters))
                {
                    var imdbID = parameters.GetValue<string>("MovieID");
                    var result = await _contentService.GetFullContentAsync(imdbID);
                    if (result != null)
                    {
                        FullContent = new FullContent()
                        {
                            Poster = result.Poster,
                            Title = result.Title,
                            Year = result.Year,
                            Runtime = result.Runtime,
                            Genre = result.Genre,
                            Plot = result.Plot
                        };
                    }
                }
            }
            catch (Exception ex)
            {
                _userDialogsService.Toast(ex.Message, new TimeSpan(100));
            }
            finally
            {
                _userDialogsService.HideLoading();
            }
        }
        #endregion
    }
}
