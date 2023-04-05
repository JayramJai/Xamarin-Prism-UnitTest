using Acr.UserDialogs;
using AutoFixture;
using Moq;
using MovieReview.Model;
using MovieReview.Services.Interfaces;
using MovieReview.ViewModel;
using Prism.Navigation;
using System.Collections.Generic;
using Xunit;

namespace MoviewReview.Test.ViewModelTests
{
    public class DetailPageViewModelTest
    {
        public Mock<INavigationService> _navigationService;
        public Mock<IContentService> _contentService;
        public Mock<IUserDialogs> _IUserDialogsService;
        public DetailPageViewModelTest()
        {
            _contentService = new Mock<IContentService>();
            _navigationService = new Mock<INavigationService>();
            _IUserDialogsService = new Mock<IUserDialogs>();
        }

        [Fact]
        public void ValidateKey_ValidKey_ReturnTrue()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var contentList = new List<Content>();
            var content1 = fixture.Create<Content>();
            contentList.Add(content1);
            var imdbID = fixture.Create<string>();
            var contentDetPageVM = new DetailPageViewModel(_contentService.Object, _navigationService.Object, _IUserDialogsService.Object);
            //Act
            bool result = contentDetPageVM.ValidataKey(new NavigationParameters
                        {
                            {"MovieID", imdbID}
                        });
            //Assert
            Assert.True(result);
        }
        [Fact]
        public void ValidateKey_InValidKey_ReturnFalse()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var contentList = new List<Content>();
            var content1 = fixture.Create<Content>();
            contentList.Add(content1);
            var imdbID = fixture.Create<string>();
            var contentDetPageVM = new DetailPageViewModel(_contentService.Object, _navigationService.Object, _IUserDialogsService.Object);
            //Act
            bool result = contentDetPageVM.ValidataKey(new NavigationParameters
                        {
                            {"ImdbID", imdbID}
                        });
            //Assert
            Assert.False(result);
        }
        [Fact]
        public void OnNavigatedTo_PassingParameter_CheckResult()
        {
            //Arrange
            Fixture fixture = new Fixture();
            var contentList = new List<Content>();
            var content1 = fixture.Create<Content>();
            contentList.Add(content1);
            var imdbID = fixture.Create<string>();
            var fullContent = fixture.Create<FullContent>();
            var contentDetPageVM = new DetailPageViewModel(_contentService.Object, _navigationService.Object, _IUserDialogsService.Object);
            _contentService.Setup(x => x.GetFullContentAsync(imdbID)).ReturnsAsync(fullContent);
            //Act
            contentDetPageVM.OnNavigatedTo(new NavigationParameters
                        {
                            {"MovieID", imdbID}
                        });
            //Assert
            Assert.Equal(fullContent.Poster, contentDetPageVM.FullContent.Poster);
            Assert.Equal(fullContent.Title, contentDetPageVM.FullContent.Title);
            Assert.Equal(fullContent.Year, contentDetPageVM.FullContent.Year);
            Assert.Equal(fullContent.Runtime, contentDetPageVM.FullContent.Runtime);
            Assert.Equal(fullContent.Genre, contentDetPageVM.FullContent.Genre);
            Assert.Equal(fullContent.Plot, contentDetPageVM.FullContent.Plot);
        }
    }
}
