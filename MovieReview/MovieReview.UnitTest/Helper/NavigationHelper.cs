using System;
using System.Collections.Generic;
using System.Text;
using AutoFixture;
using AutoFixture.AutoMoq;
using Prism.Navigation;

namespace MoviewReview.Test.Helper
{
    public static class NavigationHelper
    {
        private const string NavigationModeKey = "__NavigationMode";
        public static NavigationParameters GetNavigationParameters(NavigationMode navigationMode = NavigationMode.New)
        {
            var navigationParameters = new NavigationParameters();
            INavigationParametersInternal navigationParametersInternal = navigationParameters;
            navigationParametersInternal.Add(NavigationModeKey, navigationMode);
            return navigationParameters;
        }

        public static INavigationResult SuccessNavigation()
        {
            var fixture = new Fixture();
            fixture.Customize(new AutoMoqCustomization());
            return new ReadOnlyFixtureCustomization<INavigationResult>(fixture)
                .With(x => x.Success, true)
                .Create();
        }
    }
}
