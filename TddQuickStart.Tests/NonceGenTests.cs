﻿using System;
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
    }
}
