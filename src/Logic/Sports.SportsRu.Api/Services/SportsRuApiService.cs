﻿using Sports.Common.Factories;
using Sports.Common.Models;
using Sports.SportsRu.Api.Helpers;
using Sports.SportsRu.Api.Models;
using Sports.SportsRu.Api.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Sports.SportsRu.Api.Services
{
    public class SportsRuApiService : ISportsRuApiService, IDisposable
    {
        private readonly HttpClient _httpClient;

        public SportsRuApiService()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://www.sports.ru")
            };
        }

        public async Task<ServiceResponse<NewsResponseCollection>> GetNewsAsync(NewsType newsType, NewsPriority newsPriority, NewsContentOrigin newsContentOrigin, int count)
        {
            var newsRequest = new NewsRequest()
            {
                Count = count,
                Filter = new NewsRequestFilter()
            };
            switch(newsType)
            {
                case NewsType.HomePage:
                    newsRequest.Filter.Type = "homepage";
                    break;
            }
            switch(newsPriority)
            {
                case NewsPriority.Main:
                    newsRequest.Filter.NewsPriority = "main";
                    break;
            }
            switch(newsContentOrigin)
            {
                case NewsContentOrigin.Mixed:
                    newsRequest.Filter.ContentOrigin = "mixed";
                    break;
            }
            string args = HttpHelper.UrlEncodeJson(newsRequest);
            var response = await _httpClient.GetAsync($"core/news/list?args={args}").ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {                
                var newsResponse = JsonSerializer.Deserialize<NewsResponseCollection>(content);
                return ServiceResponseFactory.Success(newsResponse);
            }
            return ServiceResponseFactory.Error<NewsResponseCollection>(content);
        }

        public async Task<ServiceResponse<CommentIdsResponseCollection>> GetCommentsIdsAsync(int messageId, MessageClass messageClass, Sort sort)
        {
            var commentsIdsRequest = new CommentIdsRequest()
            {
                MessageId = messageId
            };
            switch(messageClass)
            {
                case MessageClass.News:
                    commentsIdsRequest.MessageClass = "Sports::News";
                    break;
            }
            switch(sort)
            {
                case Sort.Top10:
                    commentsIdsRequest.Sort = "top10";
                    break;
            }

            string args = HttpHelper.UrlEncodeJson(commentsIdsRequest);
            var response = await _httpClient.GetAsync($"core/api/comment/get_ids?args={args}").ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var commentIdsResponse = JsonSerializer.Deserialize<CommentIdsResponseCollection>(content);
                return ServiceResponseFactory.Success(commentIdsResponse);
            }
            return ServiceResponseFactory.Error<CommentIdsResponseCollection>(content);
        }

        public async Task<ServiceResponse<CommentByIdsResponse>> GetCommentsByIds(IEnumerable<int> ids)
        {
            string commentsIds = string.Join(",", ids);
            var response = await _httpClient.GetAsync($"api/comment/get/by_ids.json?comment_ids={commentsIds}&style=newjs").ConfigureAwait(false);
            string content = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {
                var commentsByIdsResponse = JsonSerializer.Deserialize<CommentByIdsResponse>(content);
                return ServiceResponseFactory.Success(commentsByIdsResponse);
            }
            return ServiceResponseFactory.Error<CommentByIdsResponse>(content);
        }

        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _httpClient.Dispose();
                }

                _disposedValue = true;
            }
        }

        // ~SportsRuApiService()
        // {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
