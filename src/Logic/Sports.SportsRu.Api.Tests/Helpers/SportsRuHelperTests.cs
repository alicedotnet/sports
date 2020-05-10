using Sports.SportsRu.Api.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sports.SportsRu.Api.Tests.Helpers
{
    public class SportsRuHelperTests
    {
        [Fact]
        public void InternalUrl_Valid()
        {
            var internalUrl = new Uri("https://www.sports.ru/football/1085421239.html");
            Assert.True(SportsRuHelper.IsInternalUrl(internalUrl));
        }

        [Fact]
        public void InternalUrl_Invalid()
        {
            var internalUrl = new Uri("https://www.google.com/");
            Assert.False(SportsRuHelper.IsInternalUrl(internalUrl));
        }
    }
}
