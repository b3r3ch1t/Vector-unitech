using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using vector_unitech_core.Utils;

namespace vector_unitech_service
{
    public class HttpHelper<T>
    {
        public static async Task<OperationResult<T>> PostByDictionaryAsync<C>(
            string baseAddress,
            string method,
            C content,
            string accessToken = null,
            bool bearerMode = true,
            List<KeyValuePair<string, string>> headers = null,
            params string[] contentType
            )
        {
            HttpContent encodedContent = null;
            using ( var client = new HttpClient() )
            {
                client.BaseAddress = new Uri( baseAddress );


                if ( !string.IsNullOrEmpty( accessToken ) )
                {
                    client.DefaultRequestHeaders.Add( "Authorization", $"{( bearerMode ? "Bearer" : "" )} {accessToken}" );
                }

                if ( contentType != null )
                {
                    foreach ( var item in contentType )
                    {
                        client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( item ) );
                    }
                }

                if ( headers != null )
                {
                    foreach ( var header in headers )
                    {
                        client.DefaultRequestHeaders.Add( header.Key, header.Value );
                    }
                }

                using ( var request = new HttpRequestMessage( HttpMethod.Post, method ) )
                {
                    if ( encodedContent != null )
                    {
                        request.Content = encodedContent;
                    }
                    var response = client.SendAsync( request, HttpCompletionOption.ResponseHeadersRead ).Result;
                    return response.IsSuccessStatusCode ?
                      await GetObjectResultAsync( response ) :
                          await HandleErrorAsync( response );
                }
            }
        }

        public static async Task<OperationResult<T>> PutAsync<C>(
            string baseAddress,
            string method,
            C content,
            long id,
            string accessToken = null,
            bool bearerMode = true,
            List<KeyValuePair<string, string>> headers = null,
            params string[] contentType
            )
        {
            using ( var client = new HttpClient() )
            {
                client.BaseAddress = new Uri( baseAddress );

                if ( !string.IsNullOrEmpty( accessToken ) )
                {
                    client.DefaultRequestHeaders.Add( "Authorization", $"{( bearerMode ? "Bearer" : "" )} {accessToken}" );
                }

                if ( headers != null )
                {
                    foreach ( var header in headers )
                    {
                        client.DefaultRequestHeaders.Add( header.Key, header.Value );
                    }
                }
                var response = await client.PutAsJsonAsync<C>( string.Format( "{0}/{1}", method, id ), content );

                return response.IsSuccessStatusCode ?
                    await GetObjectResultAsync( response ) :
                    await HandleErrorAsync( response );

            }
        }

        public static async Task<OperationResult<T>> PostAsync<C>(
            string baseAddress,
            string method,
            C content,
            string accessToken = null,
            bool bearerMode = true,
            List<KeyValuePair<string, string>> headers = null,
            params string[] contentType
            )
        {
            using ( var client = new HttpClient() )
            {
                client.BaseAddress = new Uri( baseAddress );

                if ( !string.IsNullOrEmpty( accessToken ) )
                {
                    client.DefaultRequestHeaders.Add( "Authorization", $"{( bearerMode ? "Bearer" : "" )} {accessToken}" );
                }

                if ( headers != null )
                {
                    foreach ( var header in headers )
                    {
                        client.DefaultRequestHeaders.Add( header.Key, header.Value );
                    }
                }
                var response = client.PostAsJsonAsync<C>( method, content ).Result;

                return response.IsSuccessStatusCode ?
                    await GetObjectResultAsync( response ) :
                    await HandleErrorAsync( response );

            }
        }

        public static OperationResult<T> GetByDictionary( string baseAddress, string method, object content = null, string accessToken = null, bool bearerMode = true )
        {
            using ( var client = new HttpClient() )
            {
                var builder = new UriBuilder( $"{baseAddress}{method}" );

                if ( !string.IsNullOrEmpty( accessToken ) )
                {
                    client.DefaultRequestHeaders.Add( "Authorization", $"{( bearerMode ? "Bearer" : "" )} {accessToken}" );
                }
                using ( var request = new HttpRequestMessage( HttpMethod.Get, builder.Uri ) )
                {
                    ServicePointManager.SecurityProtocol = ( SecurityProtocolType ) 768 | ( SecurityProtocolType ) 3072;

                    var response = client.SendAsync( request, HttpCompletionOption.ResponseHeadersRead ).Result;


                    if ( response.IsSuccessStatusCode )
                    {
                        var result = GetObjectResult( response );
                        return result;
                    }
                    else
                    {
                        var result = HandleError( response );
                        return result;
                    }



                }
            }
        }

        private static async Task<OperationResult<T>> HandleErrorAsync( HttpResponseMessage response )
        {
            var message = await response.Content.ReadAsStringAsync();
            //if ( logContext != null )
            //    "<<< | **ERROR**".Log( logContext, new Exception( message ) );

            return new OperationResult<T>( statusCode: response.StatusCode, message: message );

        }

        private static OperationResult<T> GetObjectResult( HttpResponseMessage response )
        {
            var content = response.Content.ReadAsStringAsync().Result;

            content = content.Replace( "original~", "originalDecoded" ).Replace( "$", "_" );
            var desearilizedContent = JsonConvert.DeserializeObject<T>( content );
            var result = new OperationResult<T>( desearilizedContent )
            {
                HttpStatusCode = HttpStatusCode.OK
            };


            return result;
        }


        private static async Task<OperationResult<T>> GetObjectResultAsync( HttpResponseMessage response )
        {
            var content = await response.Content.ReadAsStringAsync();

            content = content.Replace( "original~", "originalDecoded" ).Replace( "$", "_" );
            var desearilizedContent = JsonConvert.DeserializeObject<T>( content );
            var result = new OperationResult<T>( desearilizedContent )
            {
                HttpStatusCode = HttpStatusCode.OK
            };


            return result;
        }

        public static async Task<OperationResult<T>> GetByDictionaryAsync( string baseAddress, string method, object content = null, string accessToken = null, bool bearerMode = true )
        {
            using ( var client = new HttpClient() )
            {
                var builder = new UriBuilder( $"{baseAddress}{method}" );
                if ( content != null )
                    builder.Query = ObjectToDictionaryHelper.ToQueryString( content );

                if ( !string.IsNullOrEmpty( accessToken ) )
                {
                    client.DefaultRequestHeaders.Add( "Authorization", $"{( bearerMode ? "Bearer" : "" )} {accessToken}" );
                }
                using ( var request = new HttpRequestMessage( HttpMethod.Get, builder.Uri ) )
                {
                    ServicePointManager.SecurityProtocol = ( SecurityProtocolType ) 768 | ( SecurityProtocolType ) 3072;

                    var response = client.SendAsync( request, HttpCompletionOption.ResponseHeadersRead ).Result;


                    if ( response.IsSuccessStatusCode )
                    {
                        var result = await GetObjectResultAsync( response );
                        return result;
                    }
                    else
                    {
                        var result = await HandleErrorAsync( response );
                        return result;
                    }



                }
            }
        }


        private static OperationResult<T> HandleError( HttpResponseMessage response )
        {
            var message = response.Content.ReadAsStringAsync().Result;
            //if ( logContext != null )
            //    "<<< | **ERROR**".Log( logContext, new Exception( message ) );

            return new OperationResult<T>( statusCode: response.StatusCode, message: message );


        }

    }
}
