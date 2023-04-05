using MovieReview.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieReview.Services.Interfaces
{
    public interface IContentService
    {
        Task<FullContent> GetFullContentAsync(string id);
        Task<List<Content>> SearchContentsAsync(string searchText);
    }
}
