using AutoFixture;
using Flurl.Http.Configuration;
using Flurl.Http.Testing;
using Moq;
using MovieReview.Model;
using MovieReview.Services;
using MovieReview.Services.Interfaces;
using Plugin.Connectivity.Abstractions;
using System.Collections.Generic;
using Xunit;

namespace MoviewReview.Test.ServiceTests
{
    public class ContentServiceTest
    {
        public Mock<ILocalStorageService> _localStorageService;
        public Mock<IConnectivity> _conntectivity;
        public ContentServiceTest()
        {
            _localStorageService = new Mock<ILocalStorageService>();
            _conntectivity = new Mock<IConnectivity>();
        }

        [Fact]
        public async void SearchContentsAsync_VerifyResponse_ReturnResult()
        {
            //Arrange
            var content = new Fixture().Create<List<Content>>();
            var contentResponse = new Fixture().Create<ContentResponse>();
            contentResponse.Search = content;
            _conntectivity.Setup(i => i.IsConnected).Returns(true);
            IContentService sut = new ContentService(new PerBaseUrlFlurlClientFactory(), _localStorageService.Object, _conntectivity.Object);
            //Act
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWithJson(contentResponse);
                var actualContentResponse = await sut.SearchContentsAsync("Avengers");
                //Assert
                Assert.NotNull(actualContentResponse);
                Assert.Equal(contentResponse.Search.Count, actualContentResponse.Count);
                Assert.Equal(contentResponse.Search[0].Title, actualContentResponse[0].Title);
                Assert.Equal(contentResponse.Search[0].Poster, actualContentResponse[0].Poster);
                Assert.Equal(contentResponse.Search[0].Year, actualContentResponse[0].Year);
                Assert.Equal(contentResponse.Search[0].Type, actualContentResponse[0].Type);
            }
        }

        [Fact]
        public async void GetFullContentAsync_VerifyResponse_ReturnResult()
        {
            //Arrange
            var fullContent = new Fixture().Create<FullContent>();
            _conntectivity.Setup(i => i.IsConnected).Returns(true);
            IContentService sut = new ContentService(new PerBaseUrlFlurlClientFactory(), _localStorageService.Object, _conntectivity.Object);
            //Act
            using (var httpTest = new HttpTest())
            {
                httpTest.RespondWithJson(fullContent);
                var actualFullContentResponse = await sut.GetFullContentAsync(fullContent.ImdbID);
                //Assert
                Assert.NotNull(fullContent);
                Assert.Equal(fullContent.Title, actualFullContentResponse.Title);
                Assert.Equal(fullContent.Poster, actualFullContentResponse.Poster);
                Assert.Equal(fullContent.Year, actualFullContentResponse.Year);
                Assert.Equal(fullContent.Genre, actualFullContentResponse.Genre);
            }
        }
    }
}
