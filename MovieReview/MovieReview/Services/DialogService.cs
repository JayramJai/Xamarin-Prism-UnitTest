using System.Diagnostics.CodeAnalysis;
using Acr.UserDialogs;
using MovieReview.Localization;
using MovieReview.Services.Interfaces;
using Prism.Services;

namespace MovieReview.Services
{
    [ExcludeFromCodeCoverage]
    public class DialogService : IDialogService
    {
        private readonly IUserDialogs _userDialogs;
        private readonly IDeviceService _deviceService;

        public DialogService(IUserDialogs userDialogs, IDeviceService deviceService)
        {
            _userDialogs = userDialogs;
            _deviceService = deviceService;
        }

        public void ShowLoading(string message)
        {
            _deviceService.BeginInvokeOnMainThread(()=> _userDialogs.ShowLoading(string.IsNullOrWhiteSpace(message) ? AppResources.Loading : message));
        }

        public void HideLoading()
        {
            _deviceService.BeginInvokeOnMainThread(_userDialogs.HideLoading);
        }

        public void Toast(string message) => _userDialogs.Toast(message);
    }
}
