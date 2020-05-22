using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sports.SportsRu.Api.Services.Interfaces
{
    public interface IHttpService : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(Uri requestUri);
    }
}
