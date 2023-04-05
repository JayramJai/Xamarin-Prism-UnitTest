using AutoFixture;
using FluentAssertions;
using Moq;
using MovieReview.Constants;
using MovieReview.Model;
using MovieReview.Services.Interfaces;
using MovieReview.ViewModel;
using MovieReview.Views;
using MoviewReview.Test.Helper;
using Prism.Navigation;
using Prism.Navigation.Xaml;
using System;
using System.Linq;
using Xunit;

namespace MoviewReview.Test.ViewModelTests
{
    public class HomePageViewModelTest
    {
        public readonly Fixture _fixture;
        private readonly MockRepository _mockRepository;
        private readonly Mock<INavigationService> _navigationServiceMock;
        private readonly Mock<IContentService> _contentServiceMock;
        private readonly Mock<IDialogService> _dialogServiceMock;
        private readonly Random _random;

        public HomePageViewModelTest()
        {
            _fixture = new Fixture();
            _mockRepository = new MockRepository(MockBehavior.Strict);
            _navigationServiceMock = _mockRepository.Create<INavigationService>();
            _contentServiceMock = _mockRepository.Create<IContentService>();
            _dialogServiceMock = _mockRepository.Create<IDialogService>();
            _random = new Random();
        }

        [Fact]
        public void ShouldNotThrowAnyException_WhenHomepageViewModel_Constructor_IsCalled()
        {
            Action action = () => CreateViewModel();
            action.Should().NotThrow<Exception>();
        }

        private HomepageViewModel CreateViewModel()
        {
            return new HomepageViewModel(_navigationServiceMock.Object, _contentServiceMock.Object, _dialogServiceMock.Object);
        }

        [Fact]
        public void ShouldReturnMovieList_When_OnNavigated_IsCalled()
        {
            //Arrange
            var homePageViewModel = CreateViewModel();
            var contentList = _fixture.CreateMany<Content>().ToList();
            _contentServiceMock.Setup(x => x.SearchContentsAsync("Avengers")).ReturnsAsync(contentList);
            var navigationParameter = NavigationHelper.GetNavigationParameters();
            _dialogServiceMock.Setup(x => x.ShowLoading(null));
            _dialogServiceMock.Setup(x => x.HideLoading());

            //Act
            homePageViewModel.OnNavigatedTo(navigationParameter);

            //Assert
            _dialogServiceMock.Verify(x => x.ShowLoading(null), Times.Once);
            _dialogServiceMock.Verify(x => x.HideLoading(), Times.Once);
            Assert.Equal(contentList, homePageViewModel.Result);
            _mockRepository.VerifyEverything<MockRepository>();
        }

        [Fact]
        public void ShouldReturnMovieList_WhenExecute_SearchCommand()
        {
            //Arrange
            var homePageViewModel = CreateViewModel();
            int randomItem = _random.Next(2);
            var contentList = _fixture.CreateMany<Content>().ToList();
            _contentServiceMock.Setup(x => x.SearchContentsAsync("Avengers")).ReturnsAsync(contentList);
            var navigationParameter = NavigationHelper.GetNavigationParameters();

            homePageViewModel.SearchText = contentList[randomItem].Title;
            _contentServiceMock.Setup(x => x.SearchContentsAsync(homePageViewModel.SearchText)).ReturnsAsync(contentList);

            _dialogServiceMock.Setup(x => x.ShowLoading(null));
            _dialogServiceMock.Setup(x => x.HideLoading());

            //Act
            homePageViewModel.OnNavigatedTo(navigationParameter);
            homePageViewModel.SearchCommand.Execute();

            //Assert
            _dialogServiceMock.Verify(x => x.ShowLoading(null), Times.Exactly(2));
            _dialogServiceMock.Verify(x => x.HideLoading(), Times.Exactly(2));
            Assert.Equal(contentList, homePageViewModel.Result);
            _mockRepository.VerifyEverything<MockRepository>();
        }

        [Fact]
        public void ShouldThrowException_WhenExecute_SearchCommand_WithInvalidText()
        {
            //Arrange
            var homePageViewModel = CreateViewModel();
            var exception = _fixture.Create<Exception>();
            string searchText = _fixture.Create<string>();
            _contentServiceMock.Setup(x => x.SearchContentsAsync(searchText)).ThrowsAsync(exception);
            var navigationParameter = NavigationHelper.GetNavigationParameters();
            _dialogServiceMock.Setup(x => x.ShowLoading(null));
            _dialogServiceMock.Setup(x => x.HideLoading());
            _dialogServiceMock.Setup(x => x.Toast(exception.Message));
            homePageViewModel.SearchText = searchText;
            _contentServiceMock.Setup(x => x.SearchContentsAsync("Avengers")).ReturnsAsync(_fixture.CreateMany<Content>().ToList());

            //Act
            homePageViewModel.OnNavigatedTo(navigationParameter);
            homePageViewModel.SearchCommand.Execute();

            //Assert
            _dialogServiceMock.Verify(x => x.ShowLoading(null), Times.Exactly(2));
            _dialogServiceMock.Verify(x => x.Toast(exception.Message), Times.Once);
            _contentServiceMock.Verify(x=>x.SearchContentsAsync(searchText), Times.Once);
            _contentServiceMock.Verify(x => x.SearchContentsAsync("Avengers"), Times.Once);
            _dialogServiceMock.Verify(x => x.HideLoading(), Times.Exactly(2));
            _mockRepository.VerifyEverything<MockRepository>();
        }

        [Fact]
        public void ShouldNavigateToContentDetailPage_WhenExecute_DetailPageCommand()
        {
            //Arrange
            var homePageViewModel = CreateViewModel();
            int randomItem = _random.Next(2);
            var contentList = _fixture.CreateMany<Content>().ToList();
            var fullContent = _fixture.Create<FullContent>();
            _dialogServiceMock.Setup(x => x.ShowLoading(null));
            _dialogServiceMock.Setup(x => x.HideLoading());
            var navigationParameter = NavigationHelper.GetNavigationParameters();

            _contentServiceMock.Setup(x => x.SearchContentsAsync("Avengers")).ReturnsAsync(contentList);
            _navigationServiceMock.Setup(x => x.NavigateAsync(nameof(DetailPage), new Prism.Navigation.NavigationParameters
                        {
                            {NavigationKey.MovieID, contentList[randomItem].ImdbID}
                        })).ReturnsAsync(_fixture.Create<NavigationResult>());

            //Act
            homePageViewModel.OnNavigatedTo(navigationParameter);
            homePageViewModel.DetailPageCommand.Execute(contentList[randomItem]);

            //Assert
            _dialogServiceMock.Verify(x => x.ShowLoading(null), Times.Exactly(2));
            _navigationServiceMock.Verify(e => e.NavigateAsync(nameof(DetailPage), It.Is<Prism.Navigation.NavigationParameters>(x => x.ContainsKey(NavigationKey.MovieID))));
            _dialogServiceMock.Verify(x => x.HideLoading(), Times.Once);
            _mockRepository.VerifyEverything<MockRepository>();
        }

        [Fact]
        public void ShouldThrowException_WhenExecute_DetailPageCommand()
        {
            //Arrange
            var homePageViewModel = CreateViewModel();
            int randomItem = _random.Next(2);
            var exception = _fixture.Create<Exception>();
            var contentList = _fixture.CreateMany<Content>().ToList();
            var fullContent = _fixture.Create<FullContent>();
            _dialogServiceMock.Setup(x => x.ShowLoading(null));
            _dialogServiceMock.Setup(x => x.HideLoading());
            var navigationParameter = NavigationHelper.GetNavigationParameters();
            _dialogServiceMock.Setup(x => x.Toast(exception.Message));

            _contentServiceMock.Setup(x => x.SearchContentsAsync("Avengers")).ReturnsAsync(contentList);
            _navigationServiceMock.Setup(x => x.NavigateAsync(nameof(DetailPage), new Prism.Navigation.NavigationParameters
                        {
                            {NavigationKey.MovieID, contentList[randomItem].ImdbID}
                        })).ThrowsAsync(exception);

            //Act
            homePageViewModel.OnNavigatedTo(navigationParameter);
            homePageViewModel.DetailPageCommand.Execute(contentList[randomItem]);

            //Assert
            _dialogServiceMock.Verify(x => x.ShowLoading(null), Times.Exactly(2));
            _dialogServiceMock.Verify(x => x.Toast(exception.Message), Times.Once);
            _dialogServiceMock.Verify(x => x.HideLoading(), Times.Once);
            _mockRepository.VerifyEverything<MockRepository>();
        }
    }
}
