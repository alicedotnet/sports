using Sports.Alice.Models;
using Sports.Alice.Models.Settings;
using Sports.Alice.Resources;
using Sports.Data.Entities;
using Sports.Data.Models;
using Sports.Models;
using Sports.Services.Interfaces;

namespace Sports.Alice.Scenes
{
    public class LatestNewsScene : NewsScene
    {
        public override SceneType SceneType => SceneType.LatestNews;
        protected override string HeaderTts => Sports_Resources.LatestNews_Header_Tts;
        protected override string HeaderText => Sports_Resources.LatestNews_Header;
        protected override string NoNewsText => Sports_Resources.LatestNews_NoNews;
        protected override SportsButtonModel[] Buttons => new SportsButtonModel[]
            {
                new SportsButtonModel(Sports_Resources.Command_MainNews),
                new SportsButtonModel(Sports_Resources.Command_BestComments)
            };

        public LatestNewsScene(INewsService newsService, SportsSettings sportsSettings)
            : base(newsService, sportsSettings)
        {
        }

        protected override PagedResponse<NewsArticleModel> GetNews(int pageIndex, SportKind sportKind)
        {
            return NewsService
                .GetLatestNews(new PagedRequest(NewsPerPage, pageIndex), sportKind);
        }
    }
}
