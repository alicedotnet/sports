using Sports.Common.Tests;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Helpers;
using Yandex.Alice.Sdk.Models;

namespace Yandex.Alice.Sdk.Tests.Models
{
    public class AliceGalleryCardItemTests : BaseTests
    {
        public AliceGalleryCardItemTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void TitleLenghtOverflow()
        {
            var cardItem = new AliceGalleryCardItem();
            string tooLongString = "В Нидерландах досрочно завершили сезон, Соловьев угрожает Уткину, Оздоев отказал «Брайтону», турнир UFC 9 мая и другие новости утра";
            var exception = Assert.Throws<ArgumentException>(() => cardItem.Title = tooLongString);
            Assert.Equal(nameof(cardItem.Title), exception.ParamName);
            TestOutputHelper.WriteLine($"Error message: {exception.Message}");

            cardItem.Title = AliceHelper.PrepareGalleryCardItemTitle(tooLongString);
            Assert.True(cardItem.Title.Length < AliceGalleryCardItem.MaxTitleLength);
            Assert.EndsWith(AliceHelper.DefaultReducedStringEnding, cardItem.Title, StringComparison.OrdinalIgnoreCase);
            TestOutputHelper.WriteLine(cardItem.Title);
        }
    }
}
