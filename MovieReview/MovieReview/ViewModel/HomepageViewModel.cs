using MovieReview.Model;
using MovieReview.Views;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Prism.Commands;
using MovieReview.Services.Interfaces;
using MovieReview.Localization;
using MovieReview.Constants;

namespace MovieReview.ViewModel
{
    public class HomepageViewModel : ViewModelBase
    {
        #region private_fields
        private string _searchText;
        private ObservableCollection<Content> _result;
        private readonly IContentService _contentService;
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        #endregion

        #region commands
        public DelegateCommand<Content> DetailPageCommand { get; set; }
        public DelegateCommand SearchCommand { get; set; }
        #endregion

        #region constructor
        public HomepageViewModel(INavigationService navigationService, IContentService contentService, IDialogService dialogsSerivce) : base(navigationService)
        {
            _navigationService = navigationService;
            _contentService = contentService;
            _dialogService = dialogsSerivce;
            DetailPageCommand = new DelegateCommand<Content>(DetialPageCommandHandler);
            SearchCommand = new DelegateCommand(SearchCommandHandler);
            Result = new();
        }
        #endregion

        #region public_methods
        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            if (parameters.GetNavigationMode() == NavigationMode.New)
            {
                await Initialize();
            }
        }

        public async Task Initialize()
        {
            try
            {
                _dialogService.ShowLoading();
                var result = await _contentService.SearchContentsAsync(AppResources.DefaultMovieName);
                Result = new ObservableCollection<Content>(result);
            }
            catch (Exception ex)
            {
                _dialogService.Toast(ex.Message);
            }
            finally
            {
                _dialogService.HideLoading();
            }

        }
        #endregion

        #region properties
        public string SearchText { get => _searchText; set => SetProperty(ref _searchText, value); }
        public ObservableCollection<Content> Result { get => _result; set => SetProperty(ref _result, value); }

        #endregion

        #region public_methods
        public bool ValidateSearchText(String Search)
        {
            if (string.IsNullOrEmpty(Search))
                return false;

            return true;
        }
        public async void SearchCommandHandler()
        {
            try
            {
                if (ValidateSearchText(SearchText))
                {
                    _dialogService.ShowLoading();
                    var result = await _contentService.SearchContentsAsync(SearchText);
                    this.Result = new ObservableCollection<Content>(result);
                }
            }
            catch (Exception ex)
            {
                _dialogService.Toast(ex.Message);
            }
            finally
            {
                _dialogService.HideLoading();
            }

        }
        #endregion

        #region private_methods
        private async void DetialPageCommandHandler(Content content)
        {
            try
            {
                _dialogService.ShowLoading();
                await NavigationService.NavigateAsync(nameof(DetailPage), new NavigationParameters
                        {
                            {NavigationKey.MovieID, content.ImdbID}
                        });

            }
            catch (Exception ex)
            {
                _dialogService.Toast(ex.Message);
            }
        }
        #endregion
    }
}
