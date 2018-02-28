using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Helper.WebApi
{
    public class WebApiRestClient
    {
        private Dictionary<string, string[]> defaultRequestHeaders;

        public Dictionary<string, string[]> DefaultRequestHeaders
        {
            get
            {
                if (this.defaultRequestHeaders == null)
                    this.defaultRequestHeaders = new Dictionary<string, string[]>();
                return this.defaultRequestHeaders;
            }
        }

        public string ApiAddress { get; private set; }

        public string ApiUserName { get; private set; }

        public string ApiPassword { get; private set; }

        public static bool IsDebug
        {
            get
            {
                return true;
            }
        }

        public WebApiRestClient(string apiAddress, string apiUserName, string apiPassword)
        {
            this.ApiAddress = apiAddress;
            this.ApiUserName = apiUserName;
            this.ApiPassword = apiPassword;
        }

        public event InitHttpClientEventHandler InitHttpClient;

        private HttpClient CreateHttpClient(HttpClientHandler handler)
        {
            HttpClient client = new HttpClient((HttpMessageHandler)handler);
            if (!string.IsNullOrWhiteSpace(this.ApiUserName) && !client.DefaultRequestHeaders.Contains("ApiUserName"))
                client.DefaultRequestHeaders.Add("ApiUserName", this.ApiUserName);
            if (!string.IsNullOrWhiteSpace(this.ApiPassword) && !client.DefaultRequestHeaders.Contains("ApiPassword"))
                client.DefaultRequestHeaders.Add("ApiPassword", this.ApiPassword);
            if (this.DefaultRequestHeaders.Count > 0)
            {
                foreach (KeyValuePair<string, string[]> defaultRequestHeader in this.DefaultRequestHeaders)
                {
                    if (!client.DefaultRequestHeaders.Contains(defaultRequestHeader.Key))
                        client.DefaultRequestHeaders.Add(defaultRequestHeader.Key, (IEnumerable<string>)defaultRequestHeader.Value);
                }
            }
            if (this.InitHttpClient != null)
            {
                this.InitHttpClient(new InitHttpClientEventArgs(client));
            }

            return client;
        }

        public HttpResponseMessage GetAsync(string requestUri)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient httpClient = this.CreateHttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.ApiAddress);
                    return httpClient.GetAsync(requestUri).Result;
                }
            }
        }

        public HttpResponseMessage PostAsJsonAsync<T>(string requestUri, T model)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient httpClient = this.CreateHttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.ApiAddress);
                    return httpClient.PostAsJsonAsync<T>(requestUri, model).Result;
                }
            }
        }

        public HttpResponseMessage PostAsXmlAsync<T>(string requestUri, T model)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient httpClient = this.CreateHttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.ApiAddress);
                    return httpClient.PostAsXmlAsync<T>(requestUri, model).Result;
                }
            }
        }

        public HttpResponseMessage PutAsJsonAsync<T>(string requestUri, T model)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient httpClient = this.CreateHttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.ApiAddress);
                    return httpClient.PutAsJsonAsync<T>(requestUri, model).Result;
                }
            }
        }

        public HttpResponseMessage PutAsXmlAsync<T>(string requestUri, T model)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient httpClient = this.CreateHttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.ApiAddress);
                    return httpClient.PutAsXmlAsync<T>(requestUri, model).Result;
                }
            }
        }

        public HttpResponseMessage DeleteAsync(string requestUri)
        {
            using (HttpClientHandler handler = new HttpClientHandler())
            {
                using (HttpClient httpClient = this.CreateHttpClient(handler))
                {
                    httpClient.BaseAddress = new Uri(this.ApiAddress);
                    return httpClient.DeleteAsync(requestUri).Result;
                }
            }
        }
    }
}
