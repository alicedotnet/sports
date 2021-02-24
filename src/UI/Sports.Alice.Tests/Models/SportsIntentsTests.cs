using Sports.Alice.Models;
using Sports.Alice.Tests.TestsInfrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Sports.Alice.Tests.Models
{
    public class SportsIntentsTests
    {
        [Fact]
        public void TestIntents()
        {
            string text = File.ReadAllText(TestsConstants.Assets.IntentsFilePath);
            var sportsIntents = JsonSerializer.Deserialize<SportsIntents>(text);
            Assert.NotNull(sportsIntents);
            Assert.NotNull(sportsIntents.Read);
            Assert.NotNull(sportsIntents.Read.Slots.InfoType);
            Assert.Equal(InfoType.News, sportsIntents.Read.Slots.InfoType.Value);
            Assert.NotNull(sportsIntents.Read.Slots.InfoCategory);
            Assert.Equal(InfoCategory.Latest, sportsIntents.Read.Slots.InfoCategory.Value);
            Assert.NotNull(sportsIntents.Read.Slots.Sport);
            Assert.Equal(Sport.Football, sportsIntents.Read.Slots.Sport.Value);
        }
    }
}
