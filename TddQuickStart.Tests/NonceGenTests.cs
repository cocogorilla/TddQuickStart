﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
