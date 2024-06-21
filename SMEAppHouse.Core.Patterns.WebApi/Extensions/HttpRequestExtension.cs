using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using RestSharp;

namespace SMEAppHouse.Core.Patterns.WebApi.Extensions
{
    public static class HttpRequestExtension
    {
        public static async Task<T> GetRawBodyTypeFormaterAsync<T>(this HttpRequest httpRequest, ILogger logger = null, Encoding encoding = null)
        {
            try
            {
                var json = await httpRequest.GetRawBodyStringFormaterAsync(encoding);
                if (json == null)
                    return default(T);

                if (logger!=null)
                    logger.LogInformation($"Raw body in http request:{json}");
                return JsonConvert.DeserializeObject<T>(json);
            }
            catch (JsonReaderException )
            {
                return default(T);
            }
            catch (JsonSerializationException)
            {
                return default(T);
            }
        }

        public static async Task<string> GetRawBodyStringFormaterAsync(this HttpRequest httpRequest, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            httpRequest.EnableBuffering();
            //httpRequest.EnableRewind();
            using (var reader = new StreamReader(httpRequest.Body, encoding, false, 30000, true))
            {
                var body = await reader.ReadToEndAsync();
                httpRequest.Body.Position = 0;
                return body;
            }
        }

        public static async Task<string> GetFormValueAsync(this HttpRequest httpRequest,string parameterName)
        {
            if (!httpRequest.HasFormContentType)
                return null;

            try
            {
                if (httpRequest.Form.TryGetValue(parameterName, out StringValues values))
                    return values;
            }
            catch (InvalidOperationException)
            {
                return null;
            }

            return null;
        }

        //public static async Task<byte[]> GetRawBodyBinaryFormater(this HttpRequest httpRequest, Encoding encoding)
        //{
        //    if (encoding == null)
        //    {
        //        encoding = Encoding.UTF8;
        //    }

        //    using (StreamReader reader = new StreamReader(httpRequest.Body, encoding))
        //    {
        //        using (var ms = new MemoryStream(2048))
        //        {
        //            await httpRequest.Body.CopyToAsync(ms);
        //            return ms.ToArray(); // returns base64 encoded string JSON result
        //        }
        //    }
        //}

        /// <summary>
        /// The version of the RestSharp package does not provide a direct method to remove parameters 
        /// from the RestRequest based on a condition. This extension method achieve similar by manipulating 
        /// the Parameters collection of the RestRequest manually.
        /// </summary>
        /// <param name="restRequest"></param>
        /// <param name="condition"></param>
        public static void RemoveParameters(this RestRequest restRequest, Func<Parameter, bool> condition)
        {
            // Create a list to store parameters to be removed
            List<Parameter> parametersToRemove = new List<Parameter>();

            // Identify parameters to be removed based on the condition
            foreach (var parameter in restRequest.Parameters)
            {
                if (condition(parameter))
                    parametersToRemove.Add(parameter);
            }

            // Remove identified parameters from the request
            foreach (var parameterToRemove in parametersToRemove)
            {
                restRequest.Parameters.RemoveParameter(parameterToRemove);
            }
        }
    }
}
