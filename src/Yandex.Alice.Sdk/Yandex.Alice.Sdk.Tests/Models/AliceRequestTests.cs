using Sports.Common.Tests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;
using Yandex.Alice.Sdk.Models;
using Yandex.Alice.Sdk.Resources;
using Yandex.Alice.Sdk.Tests.TestsInfrastructure;

namespace Yandex.Alice.Sdk.Tests.Models
{
    public class AliceRequestTests : BaseTests
    {
        public AliceRequestTests(ITestOutputHelper testOutputHelper) : base(testOutputHelper)
        {
        }

        [Fact]
        public void Deserialization_Ok()
        {
            string requestJson = File.ReadAllText(TestsConstants.Assets.AliceRequestFilePath);
            var aliceRequest = JsonSerializer.Deserialize<AliceRequest>(requestJson);
            Assert.NotNull(aliceRequest);
            WritePrettyJson(aliceRequest);
        }

        [Fact]
        public void UnknownRequestType_Error()
        {
            const string unknownTypeValue = "iamunknown";
            string json = $"{{\"request\": {{ \"type\": \"{unknownTypeValue}\" }}}}";
            var exception = Assert.Throws<Exception>(() => JsonSerializer.Deserialize<AliceRequest>(json));
            string message = string.Format(CultureInfo.CurrentCulture, Yandex_Alice_Sdk_Resources.Error_Unknown_Request_Type, unknownTypeValue);
            Assert.Equal(message, exception.Message);
        }
    }
}
