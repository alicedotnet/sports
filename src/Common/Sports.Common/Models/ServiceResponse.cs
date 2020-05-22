using System;
using System.Collections.Generic;
using System.Text;

namespace Sports.Common.Models
{
    public class ServiceResponse<T> : ServiceResponse
    {
        public T Content { get; set; }

        internal ServiceResponse(T content, bool
            success = true, string errorMessage = null)
           : base(success, errorMessage)
        {
            Content = content;
        }
    }

    public class ServiceResponse
    {
        public bool IsSuccess { get; }
        public string ErrorMessage { get; }

        internal ServiceResponse(bool success = true, string errorMessage = null)
        {
            IsSuccess = success;
            ErrorMessage = errorMessage;
        }
    }
}
