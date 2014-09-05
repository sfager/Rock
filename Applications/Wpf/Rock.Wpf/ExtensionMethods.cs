using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RestSharp;

namespace Rock.Client
{
    public static class ExtensionMethods
    {
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

        /// <summary>
        /// Attempts to convert string to Guid.  Returns Guid.Empty if unsuccessful.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static Guid AsGuid( this string str )
        {
            return str.AsGuidOrNull() ?? Guid.Empty;
        }

        /// <summary>
        /// Attempts to convert string to Guid.  Returns null if unsuccessful.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static Guid? AsGuidOrNull( this string str )
        {
            Guid value;
            if ( Guid.TryParse( str, out value ) )
            {
                return value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Splits a Camel or Pascal cased identifier into seperate words.
        /// </summary>
        /// <param name="str">The identifier.</param>
        /// <returns></returns>
        public static string SplitCase( this string str )
        {
            if ( str == null )
                return null;

            return Regex.Replace( Regex.Replace( str, @"(\P{Ll})(\P{Ll}\p{Ll})", "$1 $2" ), @"(\p{Ll})(\P{Ll})", "$1 $2" );
        }

        /// <summary>
        /// To the true false.
        /// </summary>
        /// <param name="value">if set to <c>true</c> [value].</param>
        /// <returns></returns>
        public static string ToTrueFalse( this bool value )
        {
            return value ? "True" : "False";
        }

        /// <summary>
        /// Removes the special characters.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <returns></returns>
        public static string RemoveSpecialCharacters( this string str )
        {
            StringBuilder sb = new StringBuilder();
            foreach ( char c in str )
            {
                if ( ( c >= '0' && c <= '9' ) || ( c >= 'A' && c <= 'Z' ) || ( c >= 'a' && c <= 'z' ) || c == '.' || c == '_' )
                {
                    sb.Append( c );
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// Gets the property value.
        /// </summary>
        /// <param name="rootObj">The root object.</param>
        /// <param name="propertyPathName">Name of the property path.</param>
        /// <returns></returns>
        public static object GetPropertyValue( this object rootObj, string propertyPathName )
        {
            var propPath = propertyPathName.Split( new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries ).ToList<string>();

            object obj = rootObj;
            Type objType = rootObj.GetType();

            while ( propPath.Any() && obj != null )
            {
                PropertyInfo property = objType.GetProperty( propPath.First() );
                if ( property != null )
                {
                    obj = property.GetValue( obj );
                    objType = property.PropertyType;
                    propPath = propPath.Skip( 1 ).ToList();
                }
                else
                {
                    obj = null;
                }
            }

            return obj;
        }
    }
}
