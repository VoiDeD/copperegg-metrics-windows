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
using System.Web;

namespace CopperEggLib
{
    class APIClient : IDisposable
    {
        const string API_BASE = "https://api.copperegg.com/v2/";


        string apiKey;
        Dictionary<string, string> arguments;

        HttpClient httpClient;


        public TimeSpan Timeout
        {
            get { return httpClient.Timeout; }
            set { httpClient.Timeout = value; }
        }


        public APIClient( string apiKey )
        {
            this.apiKey = apiKey;
            arguments = new Dictionary<string, string>();

            HttpClientHandler handler = new HttpClientHandler
            {
                UseProxy = false,
                AllowAutoRedirect = false,
                PreAuthenticate = true,
                Credentials = new NetworkCredential( apiKey, "U" ),
            };

            httpClient = new HttpClient( handler );

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
            };
        }


        public void AddArgument( string name, string value )
        {
            arguments[ name ] = value;
        }
        public void AddArgument( string name, IEnumerable<string> values )
        {
            arguments[ name ] = string.Join( ",", values );
        }

        public async Task<T> Request<T>( string command, HttpMethod method = null, object postData = null )
        {
            var respMsg = await RequestInternal( command, method, postData );

            var streamReader = new StreamReader( await respMsg.Content.ReadAsStreamAsync() );

            JsonSerializer js = new JsonSerializer();
            return js.Deserialize<T>( new JsonTextReader( streamReader ) );
        }
        public Task Request( string command, HttpMethod method = null, object postData = null )
        {
            return RequestInternal( command, method, postData );
        }


        async Task<HttpResponseMessage> RequestInternal( string command, HttpMethod method, object postData )
        {
            if ( method == null )
                method = HttpMethod.Get;

            string url = string.Format( "{0}{1}", API_BASE, command );

            var reqMsg = new HttpRequestMessage();

            reqMsg.Method = method;
            //reqMsg.Headers.Add( "Authorization", Convert.ToBase64String( Encoding.UTF8.GetBytes( apiKey + ":U" ) ) );

            var uriBuilder = new UriBuilder( url );

            if ( postData != null )
            {
                // if we have any post data, this takes priority over http method
                // because the copperegg api apparently includes GET APIs with POST data
                // see: http://dev.copperegg.com/revealmetrics/samples.html

                string json = JsonConvert.SerializeObject( postData );
                reqMsg.Content = new StringContent( json, Encoding.ASCII, "application/json" );
            }
            else
            {
                // otherwise, defer to the http method

                if ( reqMsg.Method == HttpMethod.Get )
                {
                    var queryArguments = HttpUtility.ParseQueryString( string.Empty );

                    foreach ( var arg in arguments )
                        queryArguments[ arg.Key ] = arg.Value;

                    uriBuilder.Query = queryArguments.ToString();
                }
                else
                {
                    reqMsg.Content = new FormUrlEncodedContent( arguments );
                }
            }

            reqMsg.RequestUri = uriBuilder.Uri;

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
