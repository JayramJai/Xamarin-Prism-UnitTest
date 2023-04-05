using Flurl.Http;
using Flurl.Http.Configuration;
using MovieReview.Constants;
using MovieReview.Model;
using MovieReview.Services.Interfaces;
using Newtonsoft.Json;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieReview.Services
{
    public class ContentService : IContentService
    {
        private readonly ILocalStorageService _localStorageService;
        private IFlurlClient _flurlClient;
        private readonly IConnectivity _connectivity;

        public ContentService(IFlurlClientFactory flurlClientFac, ILocalStorageService localStorageService, IConnectivity connectivity)
        {
            _flurlClient = flurlClientFac.Get(AppConstants.BaseUrl);
            _localStorageService = localStorageService;
            _connectivity = connectivity;
        }
        public async Task<List<Content>> SearchContentsAsync(string searchText)
        {
            if (!_connectivity.IsConnected)
            {
                var allContents = _localStorageService.GetItemList<Content>();
                
                if (allContents == null || allContents.Count == 0)
                    throw new Exception("Please make sure your device is connected to the Internet!");

                var searchContent = allContents.Find(i => i.Title == searchText);
                if (searchContent != null)
                    return new List<Content>() { searchContent };
                else
                {
                    return allContents.OrderBy(a => a.Title).ToList();
                }
            }

            var url = string.Format( AppConstants.FindByName, AppConstants.ApiKey, searchText);
            var response = await _flurlClient.Request(url).GetAsync();
            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var resultContent = await response.GetStringAsync();
                var resultData = JsonConvert.DeserializeObject<ContentResponse>(resultContent);
                if (resultData.Response.Equals("False"))
                    throw new Exception("Please enter valid input");
                _localStorageService.InsertList(resultData.Search);
                if (resultData.Search == null)
                    throw new Exception("There is no data available");
                else
                    return resultData.Search;
            }
            return null;
        }
        public async Task<FullContent> GetFullContentAsync(string id)
        {
            if (!_connectivity.IsConnected)
            {
                var getDetails = _localStorageService.GetItem(id);

                if (getDetails == null)
                    throw new Exception("Please make sure your device is connected to the Internet!");
                else
                {
                    return getDetails;
                }
            }
            var url = string.Format(AppConstants.FindByID, AppConstants.ApiKey, id);

            var response = await _flurlClient.Request(url).GetAsync();
            if (response.ResponseMessage.IsSuccessStatusCode)
            {
                var resultContent = await response.GetStringAsync(); 
                var resultData = JsonConvert.DeserializeObject<FullContent>(resultContent);
                _localStorageService.InsertItem(resultData);
                return resultData;
            }
            return null;
        }
    }
}
