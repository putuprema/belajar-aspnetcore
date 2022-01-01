using System;

namespace AspNetAuth.Shared.Classes.Response
{
    public class BaseResponse
    {
        public BaseResponse()
        {
        }

        public BaseResponse(string message)
        {
            Message = message;
        }

        public BaseResponse(Exception ex, string message)
        {
            StackTrace = ex.StackTrace;
            Message = message;
        }

        public string Message { get; set; }
        public string StackTrace { get; set; }
    }
}