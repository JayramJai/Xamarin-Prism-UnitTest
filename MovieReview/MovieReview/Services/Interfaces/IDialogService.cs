namespace MovieReview.Services.Interfaces
{
    public interface IDialogService
    {
        void ShowLoading(string message = null);
        void HideLoading();
        void Toast(string message = null);
    }
}
