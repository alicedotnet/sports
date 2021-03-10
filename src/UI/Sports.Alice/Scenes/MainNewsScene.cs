using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Resources;
using Sports.Data.Entities;
using Sports.Data.Models;
using Sports.Models;
using Sports.Services.Interfaces;
using System;

namespace Sports.Alice.Scenes
{
    public class MainNewsScene : NewsScene
    {
        public override SceneType SceneType => SceneType.MainNews;
        protected override string HeaderTts => Sports_Resources.MainNews_Header_Tts;
        protected override string HeaderText => Sports_Resources.MainNews_Header;
        protected override string NoNewsText => Sports_Resources.MainNews_NoNews;
        protected override SportsButtonModel[] Buttons => new SportsButtonModel[]
            {
                new SportsButtonModel(Sports_Resources.Command_LatestNews),
                new SportsButtonModel(Sports_Resources.Command_BestComments)
            };

        public MainNewsScene(INewsService newsService, SportsSettings sportsSettings)
            : base(newsService, sportsSettings)
        {
        }

        protected override PagedResponse<NewsArticleModel> GetNews(int pageIndex, SportKind sportKind)
        {
            return NewsService
                .GetPopularNews(DateTimeOffset.Now.AddDays(-1),
                    new PagedRequest(NewsPerPage, pageIndex),
                    sportKind);
        }
    }
}
