using Sports.Data.Context;
using Sports.Data.Entities;
using Sports.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sports.Services
{
    public class NewsService : INewsService
    {
        private readonly SportsContext _sportsContext;

        public NewsService(SportsContext sportsContext)
        {
            _sportsContext = sportsContext;
        }

        
    }
}
