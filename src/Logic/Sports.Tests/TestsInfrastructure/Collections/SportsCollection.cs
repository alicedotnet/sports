using Sports.Tests.TestsInfrastructure.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sports.Tests.TestsInfrastructure.Collections
{
    [CollectionDefinition(TestsConstants.SportsCollectionName)]
    public class SportsCollection : ICollectionFixture<SportsFixture>
    {
    }
}
