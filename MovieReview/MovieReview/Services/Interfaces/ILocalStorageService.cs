using MovieReview.Model;
using System.Collections.Generic;

namespace MovieReview.Services.Interfaces
{
    public interface ILocalStorageService
    {
        int InsertList<T>(List<T> items);
        List<T> GetItemList<T>();
        int InsertItem<T>(T item);
        FullContent GetItem(string id);
    }
}
