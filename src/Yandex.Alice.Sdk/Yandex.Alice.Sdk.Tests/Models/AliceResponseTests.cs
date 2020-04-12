using Sports.Common.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Models;
using Yandex.Alice.Sdk.Tests.TestsInfrastructure;

namespace Yandex.Alice.Sdk.Tests.Models
{
    public class AliceResponseTests : BaseTests
    {
        public AliceResponseTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Deserialize_Ok()
        {
            string json = File.ReadAllText(TestsConstants.Assets.AliceResponseFilePath);
            var aliceResponse = JsonSerializer.Deserialize<AliceResponse>(json);
            Assert.NotNull(aliceResponse);
            Assert.NotNull(aliceResponse.Response);
            Assert.NotNull(aliceResponse.Response.Buttons);
            Assert.NotEmpty(aliceResponse.Response.Buttons);
            foreach (var button in aliceResponse.Response.Buttons)
            {
                Assert.NotNull(button.Url);
            }
            WritePrettyJson(aliceResponse);
        }
    }
}
