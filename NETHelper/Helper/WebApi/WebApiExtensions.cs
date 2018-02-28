using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Helper.WebApi
{
    public static class WebApiExtensions
    {
        public static string GetHeaderValue(this HttpHeaders headers, string headerName)
        {
            string str = string.Empty;
            if (headers != null && headers.Contains(headerName))
                str = headers.GetValues(headerName).FirstOrDefault<string>();
            return str;
        }

        public static T ReadAsAsync<T>(this HttpResponseMessage message)
        {
            T obj = default(T);
            if (message != null && message.IsSuccessStatusCode)
                obj = message.Content.ReadAsAsync<T>().Result;
            return obj;
        }

        public static object ReadAsAsync(this HttpResponseMessage message, Type objectType)
        {
            object obj = (object)null;
            if (message != null && message.IsSuccessStatusCode)
                obj = message.Content.ReadAsAsync(objectType).Result;
            return obj;
        }
    }
}
