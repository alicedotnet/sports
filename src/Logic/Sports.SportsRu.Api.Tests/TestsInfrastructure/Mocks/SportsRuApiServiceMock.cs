using Microsoft.Extensions.Logging;
using Sports.SportsRu.Api.Services;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.SportsRu.Api.Tests.TestsInfrastructure.Mocks
{
    public class SportsRuApiServiceMock : SportsRuApiService
    {
        public SportsRuApiServiceMock(IHttpService statHttpService, ILogger<SportsRuApiService> logger) : base(logger)
        {
            StatHttpClient = statHttpService;
        }
    }
}
