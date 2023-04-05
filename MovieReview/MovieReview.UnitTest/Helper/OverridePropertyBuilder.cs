using System;
using System.Linq.Expressions;
using System.Reflection;
using AutoFixture.Kernel;
using System.Text.RegularExpressions;

namespace MoviewReview.Test.Helper
{
    public class OverridePropertyBuilder<T, TProp> : ISpecimenBuilder
    {
        private readonly PropertyInfo _propertyInfo;
        private readonly TProp _value;

        public OverridePropertyBuilder(Expression<Func<T, TProp>> expr, TProp value)
        {
            _propertyInfo = (expr.Body as MemberExpression)?.Member as PropertyInfo ??
                            throw new InvalidOperationException("Invalid porperty expression.");
            _value = value;
        }

        public object Create(object request, ISpecimenContext context)
        {
            if (!(request is ParameterInfo parameterInfo))
            {
                return new NoSpecimen();
            }

            var camelCase = Regex.Replace(_propertyInfo.Name, @"(\w)(.*)",
                m => m.Groups[1].Value.ToLower() + m.Groups[2]);

            if (parameterInfo.ParameterType != typeof(TProp) || parameterInfo.Name != camelCase)
                return new NoSpecimen();

            return _value;
        }
    }
}
