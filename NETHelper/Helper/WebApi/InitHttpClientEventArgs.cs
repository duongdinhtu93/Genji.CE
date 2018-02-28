using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GenjiCore.Helper.WebApi
{
    public delegate void InitHttpClientEventHandler(InitHttpClientEventArgs e);

    public class InitHttpClientEventArgs
    {
        public HttpClient Client { get; private set; }

        public InitHttpClientEventArgs(HttpClient client)
        {
            this.Client = client;
        }
    }
}
