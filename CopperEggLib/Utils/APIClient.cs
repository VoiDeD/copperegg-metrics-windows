using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace CopperEggLib
{
    class APIClient : IDisposable
    {
        const string API_BASE = "https://api.copperegg.com/v2/";


        string apiKey;
        Dictionary<string, string> arguments;

        HttpClient httpClient;


        public TimeSpan Timeout { get; set; }


        public APIClient( string apiKey )
        {
            Timeout = TimeSpan.FromSeconds( 10 );

            this.apiKey = apiKey;
            arguments = new Dictionary<string, string>();

            HttpClientHandler handler = new HttpClientHandler
            {
                UseProxy = false,
                Credentials = new NetworkCredential( apiKey, "U" ),
            };

            httpClient = new HttpClient( handler );
            httpClient.Timeout = TimeSpan.FromSeconds( 10 );
        }


        public void AddArgument( string name, string value )
        {
            arguments[ name ] = value;
        }

        public async Task<T> Request<T>( string command, HttpMethod method = null )
            where T : new()
        {
            var respMsg = await RequestInternal( command, method );

            var streamReader = new StreamReader( await respMsg.Content.ReadAsStreamAsync() );

            JsonSerializer js = new JsonSerializer();
            return js.Deserialize<T>( new JsonTextReader( streamReader ) );
        }
        public Task Request( string command, HttpMethod method = null )
        {
            return RequestInternal( command, method );
        }


        async Task<HttpResponseMessage> RequestInternal( string command, HttpMethod method )
        {
            if ( method == null )
                method = HttpMethod.Get;

            string url = string.Format( "{0}{1}", API_BASE, command );

            var reqMsg = new HttpRequestMessage();

            reqMsg.Method = method;
            reqMsg.RequestUri = new Uri( url );

            var respMsg = await httpClient.SendAsync( reqMsg );
            respMsg.EnsureSuccessStatusCode();

            return respMsg;
        }


        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}
