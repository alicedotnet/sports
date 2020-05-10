using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.SportsRu.Api.Helpers
{
    public static class SportsRuHelper
    {
        private const string _sportsRuHost = "sports.ru";

        public static bool IsInternalUrl(Uri url)
        {
            if(url == null)
            {
                throw new ArgumentNullException(nameof(url));
            }
            return url.Host.Contains(_sportsRuHost, StringComparison.OrdinalIgnoreCase);
        }
    }
}
