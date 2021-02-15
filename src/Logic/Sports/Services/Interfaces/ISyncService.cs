using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Services.Interfaces
{
    public interface ISyncService
    {
        Task SyncNewsAsync();
        Task SyncPopularNewsCommentsAsync(DateTimeOffset fromDate, int newsCount);
        Task SyncPopularNewsCommentsAsync();
        void DeleteOldData(DateTimeOffset oldestDateToKeep);
    }
}
