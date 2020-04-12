using Sports.SportsRu.Api.Tests.TestsInfrastructure.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sports.SportsRu.Api.Tests.TestsInfrastructure.Collections
{
    [CollectionDefinition(TestsConstants.SportsRuApiCollectionName)]
    public class SportsRuApiCollection : ICollectionFixture<SportsRuApiFixture>
    {
    }
}
