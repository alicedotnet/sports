using Sports.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Common.Factories
{
    public static class ServiceResponseFactory
    {
        public static ServiceResponse<T> Success<T>(T content)
        {
            return new ServiceResponse<T>(content);
        }

        public static ServiceResponse<T> Error<T>(string errorMessage)
        {
            return new ServiceResponse<T>(default, false, errorMessage);
        }
    }
}
