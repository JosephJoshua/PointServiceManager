using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PSMDesktopUI.Library.Api
{
    public class ApiException : Exception
    {
        public string ErrorDescription { get; }

        public ApiException() { }

        public ApiException(string message) : base(message) { }

        public ApiException(string message, Exception inner) : base(message, inner) { }

        public ApiException(string message, string errorDescription) : this(message) => ErrorDescription = errorDescription;

        public ApiException(string message, Exception inner, string errorDescription) : this(message, inner) => ErrorDescription = errorDescription;

        public static async Task<Exception> FromHttpResponse(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode) return new Exception("Attempting to create an ApiException from a successful request");

            if (response.Content.Headers.ContentType.MediaType == "application/json")
            {
                var content = await response.Content.ReadAsAsync<HttpErrorContent>();
                string message = $"{ response.ReasonPhrase }{ (content.Error != null ? ": " : "") }{ content.Error ?? "" }";

                if (content.ErrorDescription == null)
                {
                    return new ApiException(message);
                }

                return new ApiException(message, content.ErrorDescription);
            }
            else if (response.Content.Headers.ContentType.MediaType == "text/plain")
            {
                string message = await response.Content.ReadAsStringAsync();
                return new ApiException(message);
            }

            return new ApiException(response.ReasonPhrase);
        }

        private class HttpErrorContent
        {
            [JsonProperty(PropertyName = "error")]
            public string Error { get; set; }

            [JsonProperty(PropertyName = "error_description")]
            public string ErrorDescription { get; set; }
        }
    }
}
