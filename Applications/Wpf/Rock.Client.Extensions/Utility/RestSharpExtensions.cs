using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using RestSharp;

namespace Rock.Client
{
    /// <summary>
    /// 
    /// </summary>
    public static class RestSharpExtensions
    {
        /// <summary>
        /// Logins the specified rest client.
        /// </summary>
        /// <param name="restClient">The rest client.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="rockLoginUrl">The rock login URL.</param>
        public static void Login( this RestClient restClient, string username, string password, string rockLoginUrl = "api/auth/login" )
        {
            restClient.CookieContainer = new CookieContainer();

            RestRequest restRequest = new RestRequest( Method.POST );
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Resource = rockLoginUrl;
            var loginParameters = new
            {
                UserName = username,
                Password = password
            };

            restRequest.AddBody( loginParameters );

            var response = restClient.Post( restRequest );
        }

        /// <summary>
        /// Gets the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restClient">The rest client.</param>
        /// <param name="getPath">The get path.</param>
        /// <param name="odataFilter">The odata filter.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No Response Data</exception>
        public static T GetData<T>( this RestClient restClient, string getPath, string odataFilter = null ) where T : new()
        {
            string requestUri;

            if ( !string.IsNullOrWhiteSpace( odataFilter ) )
            {
                string queryParam = "?$filter=" + odataFilter;
                requestUri = getPath + queryParam;
            }
            else
            {
                requestUri = getPath;
            }

            RestRequest restRequest = new RestRequest( requestUri );
            restRequest.RequestFormat = DataFormat.Json;

            var response = restClient.Get<T>( restRequest );
            if ( response.Data != null )
            {
                return response.Data;
            }
            else
            {
                throw response.ErrorException ?? new Exception( "No Response Data" );
            }
        }

        /// <summary>
        /// Gets the data by unique identifier.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restClient">The rest client.</param>
        /// <param name="getPath">The get path.</param>
        /// <param name="guid">The unique identifier.</param>
        /// <returns></returns>
        public static T GetDataByGuid<T>( this RestClient restClient, string getPath, Guid guid ) where T : new()
        {
            return restClient.GetData<List<T>>( getPath, string.Format( "Guid eq guid'{0}'", guid ) ).FirstOrDefault();
        }

        /// <summary>
        /// Gets the data by enum.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restClient">The rest client.</param>
        /// <param name="getPath">The get path.</param>
        /// <param name="enumFieldName">Name of the enum field.</param>
        /// <param name="enumVal">The enum value.</param>
        /// <returns></returns>
        public static T GetDataByEnum<T>( this RestClient restClient, string getPath, string enumFieldName, int enumVal ) where T : new()
        {
            return restClient.GetData<T>( getPath, string.Format( "{0} eq '{1}'", enumFieldName, enumVal ) );
        }

        /// <summary>
        /// Posts the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restClient">The rest client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        public static void PostData<T>( this RestClient restClient, string url, T data )
        {
            var restRequest = new RestRequest( url );
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody( data );
            var response = restClient.Post( restRequest );
        }

        /// <summary>
        /// Puts the data.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="restClient">The rest client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        public static void PutData<T>( this RestClient restClient, string url, T data )
        {
            var restRequest = new RestRequest( url );
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.AddBody( data );
            var response = restClient.Put( restRequest );
        }

        /// <summary>
        /// Posts the data with result.
        /// </summary>
        /// <typeparam name="D"></typeparam>
        /// <typeparam name="T"></typeparam>
        /// <param name="restClient">The rest client.</param>
        /// <param name="url">The URL.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">No Response Data</exception>
        public static T PostDataWithResult<D, T>( this RestClient restClient, string url, D data ) where T : new()
        {
            RestRequest restRequest = new RestRequest( Method.POST );
            restRequest.RequestFormat = DataFormat.Json;
            restRequest.Resource = url;

            restRequest.AddBody( data );

            var response = restClient.Post<T>( restRequest );

            if ( response.Data != null )
            {
                return response.Data;
            }
            else
            {
                throw response.ErrorException ?? new Exception( "No Response Data" );
            }
        }
    }
}
