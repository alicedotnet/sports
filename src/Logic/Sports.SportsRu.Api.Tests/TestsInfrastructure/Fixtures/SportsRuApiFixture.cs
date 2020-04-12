using Sports.SportsRu.Api.Services;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.SportsRu.Api.Tests.TestsInfrastructure.Fixtures
{
    public class SportsRuApiFixture
    {
        public ISportsRuApiService SportsRuApiService { get; }
        public SportsRuApiFixture()
        {
            SportsRuApiService = new SportsRuApiService();
        }
    }
}
