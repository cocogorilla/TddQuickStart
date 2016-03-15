using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace TddQuickStart.Tests
{
    public class NonceGenTests
    {
        #region setup autofixture xunit moq customization to enable declaritive moqed tests
        public class AutoMoqAttribute : AutoDataAttribute
        {
            public AutoMoqAttribute() :
                base(new Fixture().Customize(
                    new AutoMoqCustomization()))
            { }
        }
        #endregion

        [Fact]
        public void XunitIsFunctional()
        {
            Assert.True(true);
        }

        [Fact]
        public void AutoMoqIntegrationIsFunctional()
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            var sut = fixture.Create<INonceMethod>();
            Assert.IsAssignableFrom<INonceMethod>(sut);
        }

        [Theory, AutoMoq]
        public void AutoMoqAttributeIntegrationIsFunctional(
            DefaultNonceMethod sut)
        {
            Assert.IsAssignableFrom<INonceMethod>(sut);
        }
    }
}
