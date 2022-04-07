using System;
using System.Threading.Tasks;

namespace ORMFund
{
    public interface IApiClient : IDisposable
    {
            /// <summary>
            /// Send GET request for API.
            /// </summary>
            /// <typeparam name="T">Type of object to get</typeparam>
            /// <param name="requestRelativeAddress">Request URL.</param>
            /// <param name="correlationId">Correlation ID.</param>
            Task<T> GetApiObject<T>(string requestRelativeAddress, string correlationId = null);

            /// <summary>
            /// Send POST request to API.
            /// </summary>
            /// <typeparam name="TInput">Type of object to put.</typeparam>
            /// <typeparam name="TOutput">Type of output object.</typeparam>
            /// <param name="requestRelativeAddress">Request URL.</param>
            /// <param name="apiObject">Object to post.</param>
            /// <param name="correlationId">Correlation ID.</param>
            /// <param name="messageId">Message ID.</param>
            Task<TOutput> PostApiObject<TInput, TOutput>(string requestRelativeAddress, TInput apiObject, string correlationId = null, string messageId = null);

            /// <summary>
            /// Send PUT request to API.
            /// </summary>
            /// <typeparam name="TInput">Type of object to put.</typeparam>
            /// <typeparam name="TOutput">Type of output object.</typeparam>
            /// <param name="requestRelativeAddress">Request URL.</param>
            /// <param name="apiObject">Object to post.</param>
            /// <param name="correlationId">Correlation ID.</param>
            Task<TOutput> PutApiObject<TInput, TOutput>(string requestRelativeAddress, TInput apiObject, string correlationId = null);

            /// <summary>
            /// Send DELETE request to API.
            /// </summary>
            /// <param name="requestRelativeAddress">Request URL.</param>
            /// <param name="requestInfo">Request details (user info).</param>
            /// <param name="correlationId">Correlation Id.</param>
            Task<bool> DeleteApiObject<TInput>(string requestRelativeAddress, TInput requestInfo, string correlationId = null);
    }
}
