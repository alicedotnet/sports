using Sports.Common.Models;
using Sports.SportsRu.Api.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sports.SportsRu.Api.Services.Interfaces
{
    public interface ISportsRuApiService
    {
        Task<ServiceResponse<NewsResponse>> GetNewsAsync(NewsType newsType, NewsPriority newsPriority, NewsContentOrigin newsContentOrigin, int count);
        Task<ServiceResponse<CommentIdsResponse>> GetCommentsIdsAsync(int messageId, MessageClass messageClass, Sort sort);
    }
}
