using Microsoft.Win32.SafeHandles;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace clientlib.mylayers
{
    public class GenericClient : IDisposable
    {
        private readonly HttpClient _client;
        private readonly string _baseUrl;
        private bool disposed;
        private readonly SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);
        private bool isAuthenticated;
        public GenericClient(IHttpClientFactory factory, string baseUrl)
        {
            this._client = factory.CreateClient();
            this._baseUrl = baseUrl;
        }

        public void ConfigureHeaders()
        {
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept","text/html,application/xhtml+xml,application/xml");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Encoding", "gzip,deflate");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 6.2; WOW64; rv:19.0) Gecko/20100101 Firefox/19.0");
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Accept-Charset", "ISO-8859-1");
        }

        public void AuthenticateWithToken(string jwt)
        {
            _client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", "Bearer "+jwt);
            isAuthenticated = true;
        }
        public void AuthenticateBypass(string jwt)
        {
            isAuthenticated = true;
        }

        public void HandleAuthentication(string route)
        {
            if (!isAuthenticated)
            {
                throw new Exception("Not Authenticated to access ->" + route);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style","IDE0034:Simplify 'Default' expression",Justification ="<Compatibility>")]
        public async Task<ClientResponse<T>> Get<T>(string method)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var route = $"{_baseUrl}{method}";
            HandleAuthentication(route);
            ConfigureHeaders();
            HttpResponseMessage response = await _client.GetAsync(route).ConfigureAwait(false);
            var responseContet = await response.Content.ReadAsStringAsync();
            var headers = response.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            int statusCode = (int)response.StatusCode;
            string elapsedTime = FormatTime(watch);

            if(statusCode == (int)HttpStatusCode.OK)
            {
                return new ClientResponse<T>(statusCode,headers,JsonConvert.DeserializeObject<T>(responseContet),elapsedTime,route);
            }
            string errorContent = response.Content.ReadAsStringAsync().Result;
            string title = ExtractFromBody(errorContent, "<div class=\"titleerror\">", "</div>");

            return new ClientResponse<T>(statusCode, headers, default(T), elapsedTime, route, response.ReasonPhrase, title);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'Default' expression", Justification = "<Compatibility>")]
        public async Task<ClientResponse<T>> Post<T>(string method,string payload)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var route = $"{_baseUrl}{method}";
            HandleAuthentication(route);
            ConfigureHeaders();

            StringContent serializedPayload = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PostAsync(route,serializedPayload).ConfigureAwait(false);
            var responseContet = await response.Content.ReadAsStringAsync();
            var headers = response.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            int statusCode = (int)response.StatusCode;
            string elapsedTime = FormatTime(watch);

            if (statusCode == (int)HttpStatusCode.OK)
            {
                return new ClientResponse<T>(statusCode, headers, JsonConvert.DeserializeObject<T>(responseContet), elapsedTime, route);
            }
            string errorContent = response.Content.ReadAsStringAsync().Result;
            string title = ExtractFromBody(errorContent, "<div class=\"titleerror\">", "</div>");

            return new ClientResponse<T>(statusCode, headers, default(T), elapsedTime, route, response.ReasonPhrase, title);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'Default' expression", Justification = "<Compatibility>")]
        public async Task<ClientResponse<T>> Put<T>(string method, string payload)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var route = $"{_baseUrl}{method}";
            HandleAuthentication(route);
            ConfigureHeaders();

            StringContent serializedPayload = new StringContent(payload, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await _client.PutAsync(route, serializedPayload).ConfigureAwait(false);
            var responseContet = await response.Content.ReadAsStringAsync();
            var headers = response.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            int statusCode = (int)response.StatusCode;
            string elapsedTime = FormatTime(watch);

            if (statusCode == (int)HttpStatusCode.OK)
            {
                return new ClientResponse<T>(statusCode, headers, JsonConvert.DeserializeObject<T>(responseContet), elapsedTime, route);
            }
            string errorContent = response.Content.ReadAsStringAsync().Result;
            string title = ExtractFromBody(errorContent, "<div class=\"titleerror\">", "</div>");

            return new ClientResponse<T>(statusCode, headers, default(T), elapsedTime, route, response.ReasonPhrase, title);
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0034:Simplify 'Default' expression", Justification = "<Compatibility>")]
        public async Task<ClientResponse<T>> Delete<T>(string method)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            var route = $"{_baseUrl}{method}";
            HandleAuthentication(route);
            ConfigureHeaders();

            HttpResponseMessage response = await _client.DeleteAsync(route).ConfigureAwait(false);
            var responseContet = await response.Content.ReadAsStringAsync();
            var headers = response.Headers.ToDictionary(x => x.Key, x => x.Value.ToString());
            int statusCode = (int)response.StatusCode;
            string elapsedTime = FormatTime(watch);

            if (statusCode == (int)HttpStatusCode.OK)
            {
                return new ClientResponse<T>(statusCode, headers, JsonConvert.DeserializeObject<T>(responseContet), elapsedTime, route);
            }
            string errorContent = response.Content.ReadAsStringAsync().Result;
            string title = ExtractFromBody(errorContent, "<div class=\"titleerror\">", "</div>");

            return new ClientResponse<T>(statusCode, headers, default(T), elapsedTime, route, response.ReasonPhrase, title);
        }
        private string ExtractFromBody(string errorContent, string start, string end)
        {
            var matched = string.Empty;
            var indexStart = 0;
            var indexEnd = 0;
            bool exit = false;
            while (!exit)
            {
                indexStart = errorContent.IndexOf(start);
                if (indexStart != -1)
                {
                    indexEnd = indexStart + errorContent.Substring(indexStart).IndexOf(end);
                    matched = errorContent.Substring(indexStart + start.Length, indexEnd - indexStart - start.Length);
                    errorContent = errorContent.Substring(indexEnd + end.Length);
                }
                else
                {
                    exit = true;
                }
            }
            if (string.IsNullOrEmpty(matched)) { matched = errorContent; };
            return matched;
        }

        public static string FormatTime(System.Diagnostics.Stopwatch watch)
        {
            TimeSpan ts = watch.Elapsed;
            string elapsedMs = string.Empty;

            if (ts.Minutes > 0)
                elapsedMs = $"{ts.Minutes}m {ts.Seconds}s {ts.Milliseconds}ms";
            else if(ts.Seconds>0)
                elapsedMs = $"{ts.Seconds}s {ts.Milliseconds}ms";
            else 
                elapsedMs = $"{ts.Milliseconds}ms";

            return elapsedMs;

        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
                handle.Dispose();

            disposed = true;
        }
        ~GenericClient(){
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            GC.Collect();
        }
    }
}
