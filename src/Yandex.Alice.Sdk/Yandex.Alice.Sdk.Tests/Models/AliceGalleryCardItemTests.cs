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
        private const string _tooLongString = "В Нидерландах досрочно завершили сезон, Соловьев угрожает Уткину, Оздоев отказал «Брайтону», турнир UFC 9 мая и другие новости утра";

        public AliceGalleryCardItemTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void TrimString()
        {
            var cardItem = new AliceGalleryCardItem
            {
                Title = AliceHelper.PrepareGalleryCardItemTitle(_tooLongString)
            };
            Assert.True(cardItem.Title.Length < AliceGalleryCardItem.MaxTitleLength);
            Assert.EndsWith(AliceHelper.DefaultReducedStringEnding, cardItem.Title, StringComparison.OrdinalIgnoreCase);
            TestOutputHelper.WriteLine(cardItem.Title);
        }

        [Fact]
        public void TrimStringMandatoryEnding()
        {
            var fireEmoji = char.ConvertFromUtf32(0x1F525);
            var cardItem = new AliceGalleryCardItem
            {
                Title = AliceHelper.PrepareGalleryCardItemTitle(_tooLongString, fireEmoji, AliceHelper.DefaultReducedStringEnding)
            };
            Assert.True(cardItem.Title.Length < AliceGalleryCardItem.MaxTitleLength);
            Assert.EndsWith(fireEmoji, cardItem.Title, StringComparison.OrdinalIgnoreCase);
            TestOutputHelper.WriteLine(cardItem.Title);
        }

        [Fact]
        public void MandatoryEnding()
        {
            var fireEmoji = char.ConvertFromUtf32(0x1F525);
            var cardItem = new AliceGalleryCardItem
            {
                Title = AliceHelper.PrepareGalleryCardItemTitle("IamShort", fireEmoji, AliceHelper.DefaultReducedStringEnding)
            };
            Assert.True(cardItem.Title.Length < AliceGalleryCardItem.MaxTitleLength);
            Assert.EndsWith(fireEmoji, cardItem.Title, StringComparison.OrdinalIgnoreCase);
            TestOutputHelper.WriteLine(cardItem.Title);
        }

        [Fact]
        public void TitleLenghtOverflow()
        {
            var cardItem = new AliceGalleryCardItem();
            var exception = Assert.Throws<ArgumentException>(() => cardItem.Title = _tooLongString);
            Assert.Equal(nameof(cardItem.Title), exception.ParamName);
            TestOutputHelper.WriteLine($"Error message: {exception.Message}");
        }
    }
}
