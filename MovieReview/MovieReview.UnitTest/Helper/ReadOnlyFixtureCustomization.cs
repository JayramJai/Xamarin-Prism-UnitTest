using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using AutoFixture;

namespace MoviewReview.Test.Helper
{
    public class ReadOnlyFixtureCustomization<T>
    {
        private readonly Fixture _fixture;

        public ReadOnlyFixtureCustomization(Fixture fixture)
        {
            _fixture = fixture;
        }

        public ReadOnlyFixtureCustomization<T> With<TProp>(Expression<Func<T, TProp>> expr, TProp value)
        {
            _fixture.Customizations.Add(new OverridePropertyBuilder<T, TProp>(expr, value));
            return this;
        }

        public T Create() => _fixture.Create<T>();
    }
}
