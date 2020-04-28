using Sports.Alice.Tests.TestsInfrastructure.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Sports.Alice.Tests.TestsInfrastructure.Collections
{
    [CollectionDefinition(TestsConstants.ServerCollectionName)]
    public class ServerCollection : 
        ICollectionFixture<ServerFixture>
    {
    }
}
