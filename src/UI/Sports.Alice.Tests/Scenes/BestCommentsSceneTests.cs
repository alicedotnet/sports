using Sports.Alice.Scenes;
using Sports.Alice.Tests.TestsInfrastructure;
using Sports.Alice.Tests.TestsInfrastructure.Fixtures;
using System;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Sports.Alice.Models;
using Yandex.Alice.Sdk.Models;
using Sports.Alice.Resources;
using Sports.SportsRu.Api.Services.Interfaces;
using System.Threading.Tasks;
using Sports.SportsRu.Api.Models;
using Sports.Common.Tests;
using Xunit.Abstractions;

namespace Sports.Alice.Tests.Scenes
{
    [Collection(TestsConstants.ServerCollectionName)]

    public class BestCommentsSceneTests : BaseTests
    {
        private readonly IServiceProvider _serviceProvider;

        public BestCommentsSceneTests(ServerFixture serverFixture, ITestOutputHelper testOutputHelper)
            : base(testOutputHelper)
        {
            _serviceProvider = serverFixture.Services;
        }

        [Fact]
        public void BestCommentsScene_Reply_GetComments()
        {
            var bestCommentsScene = _serviceProvider.GetService<BestCommentsScene>();
            var sportsRequest = new SportsRequest()
            {
                State = new AliceStateModel<SportsSessionState, object>()
                {
                    Session = new SportsSessionState()
                }
            };
            var response = bestCommentsScene.Reply(sportsRequest);
            Assert.IsType<SportsResponse>(response);
            var sportsResponse = response as SportsResponse;
            Assert.NotEqual(Sports_Resources.BestComments_NoComments, sportsResponse.Response.Text);
            TestOutputHelper.WriteLine(sportsResponse.Response.Text);
        }

        [Fact]
        public void BestCommentsScene_Reply_UnknownGuid()
        {
            var bestCommentsScene = _serviceProvider.GetService<BestCommentsScene>();
            Guid unknownNextNewsArticleId = new Guid("8a93a228-fd9f-47fe-b3d5-9c040e0dcc83");
            var sportsRequest = new SportsRequest()
            {
                State = new AliceStateModel<SportsSessionState, object>()
                {
                    Session = new SportsSessionState()
                    {
                        NextNewsArticleId = unknownNextNewsArticleId
                    }
                }
            };
            var response = bestCommentsScene.Reply(sportsRequest);
            Assert.IsType<SportsResponse>(response);
            var sportsResponse = response as SportsResponse;
            Assert.NotEqual(unknownNextNewsArticleId, sportsResponse.SessionState.NextNewsArticleId);
            Assert.NotEqual(Sports_Resources.BestComments_NoComments, sportsResponse.Response.Text);
            TestOutputHelper.WriteLine(sportsResponse.Response.Text);
        }
    }
}
