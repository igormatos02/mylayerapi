using System;
using System.Collections.Generic;
using System.Text;

namespace clientlib.mylayers
{
    public class ClientResponse<T> : IDisposable
    {
        public int StatusCode { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public string ErrorMessage { get; set; }
        public string Route { get; set; }
        public string ErrorContent { get; set; }
        public string ElapsedTime { get; set; }
        public T Data { get; set; }

        public ClientResponse(int statusCode, IDictionary<string, string> headers, T data, string elapsedTime, string route)
        {
            StatusCode = statusCode;
            Headers = headers;
            Data = data;
            ElapsedTime = elapsedTime;
            Route = route;
        }

        public ClientResponse(int statusCode, IDictionary<string, string> headers, T data, string elapsedTime, string route, string message, string content)
        {
            StatusCode = statusCode;
            Headers = headers;
            Data = data;
            ElapsedTime = elapsedTime;
            Route = route;
            ErrorMessage = message;
            ErrorContent = content;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
