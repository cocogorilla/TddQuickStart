using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Idioms;
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

        #region verify testing framework and integrations is running
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
        #endregion

        [Theory, AutoMoq]
        public void NonceMethodIsCorrect(
            [Frozen] INonceMethod expected,
            NonceGen sut)
        {
            Assert.Same(expected, sut.Method);
        }

        [Theory, AutoMoq]
        public void ConstructorsAreNullGuarded(
            [Frozen] IFixture fixture,
            GuardClauseAssertion assertion)
        {
            assertion.Verify(
                typeof(NonceGen).GetConstructors());
        }

        [Theory, AutoMoq]
        public void NonceStoreIsCorrect(
            [Frozen] INonceStore expected,
            NonceGen sut)
        {
            Assert.Same(expected, sut.Store);
        }

        [Theory, AutoMoq]
        public void NonceReturnedIsGeneratedByMethod(
            Nonce expected,
            [Frozen] Mock<INonceMethod> method,
            NonceGen sut)
        {
            method
                .Setup(x => x.GenerateNonce())
                .Returns(expected);
            var actual = sut.CreateNonce();
            Assert.Equal(expected, actual);
        }

        [Theory, AutoMoq]
        public void NonceGeneratedIsNonceStored(
            Nonce expected,
            [Frozen] Mock<INonceMethod> method,
            [Frozen] Mock<INonceStore> store,
            NonceGen sut)
        {
            method
                .Setup(x => x.GenerateNonce())
                .Returns(expected);
            sut.CreateNonce();
            store.Verify(
                x => x.SaveNonce(expected),
                Times.Once());
        }

        [Theory, AutoMoq]
        public void ValidNonceFoundIsCorrect(
            Nonce storedNonce,
            [Frozen] Mock<ITimeSource> timesource,
            [Frozen] Mock<INonceStore> store,
            NonceGen sut)
        {
            // nonce has not expired
            timesource
                .Setup(x => x.GetNowEpoch())
                .Returns(Int64.MinValue);
            store
                .Setup(x => x.RetrieveNonce(storedNonce.NonceKey))
                .Returns(storedNonce);
            var actual = sut.ValidateNonce(
                storedNonce.NonceKey,
                // match on correct nonce value
                storedNonce.NonceValue);
            Assert.True(actual);
        }

        [Theory, AutoMoq]
        public void InvalidNonceFoundIsCorrect(
            Nonce storedNonce,
            int invalidNonce,
            [Frozen] Mock<INonceStore> store,
            NonceGen sut)
        {
            store
                .Setup(x => x.RetrieveNonce(storedNonce.NonceKey))
                .Returns(storedNonce);
            var actual = sut.ValidateNonce(
                storedNonce.NonceKey,
                // match on invalid nonce value
                invalidNonce);
            Assert.False(actual);
        }

        [Theory, AutoMoq]
        public void NonceExpiredIsCorrect(
            Nonce storedNonce,
            int expiredBy,
            [Frozen] Mock<ITimeSource> timesource,
            [Frozen] Mock<INonceStore> store,
            NonceGen sut)
        {
            // nonce has expired
            Assert.True(expiredBy > 0);
            timesource
                .Setup(x => x.GetNowEpoch())
                .Returns(storedNonce.NonceExpiration + expiredBy);
            store
                .Setup(x => x.RetrieveNonce(storedNonce.NonceKey))
                .Returns(storedNonce);
            var actual = sut.ValidateNonce(
                storedNonce.NonceKey,
                // nonce value is match
                storedNonce.NonceValue);
            Assert.False(actual);
        }
    }
}
