using Sports.Common.Tests.Helpers;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit.Abstractions;

namespace Sports.Common.Tests
{
    public abstract class BaseTests
    {
        protected ITestOutputHelper TestOutputHelper { get; }

        protected BaseTests(ITestOutputHelper testOutputHelper)
        {
            TestOutputHelper = testOutputHelper;
        }

        protected void WritePrettyJson<T>(T value)
        {
            string json = JsonSerializerHelper.ToPrettyJson(value);
            TestOutputHelper.WriteLine(json);
        }
    }
}
