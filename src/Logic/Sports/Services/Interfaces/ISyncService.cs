using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sports.Services.Interfaces
{
    public interface ISyncService
    {
        Task SyncAllAsync();
        void DeleteOldData(DateTimeOffset oldestDateToKeep);
    }
}
