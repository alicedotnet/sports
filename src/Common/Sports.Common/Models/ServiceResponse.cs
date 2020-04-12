using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Common.Models
{
    public class ServiceResponse<T> : ServiceResponse
    {
        public T Content { get; }

        private ServiceResponse(T content, bool success = true, string errorMessage = null)
           : base(success, errorMessage)
        {
            Content = content;
        }

        public static ServiceResponse<T> Success(T content)
        {
            return new ServiceResponse<T>(content);
        }

        public static ServiceResponse<T> Error(string errorMessage)
        {
            return new ServiceResponse<T>(default, false, errorMessage);
        }
    }

    public class ServiceResponse
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        protected ServiceResponse(bool success = true, string errorMessage = null)
        {
            IsSuccess = success;
            ErrorMessage = errorMessage;
        }
    }
}
