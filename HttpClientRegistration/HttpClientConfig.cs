using System;

namespace HttpClientCheckExample.HttpClientRegistration
{
    public sealed class HttpClientConfig<T> where T : class
    {
        /// <summary>
        /// Base url to api endpoint
        /// </summary>
        public Uri? Url { get; set; }

        /// <summary>
        /// Authorization key for api service
        /// </summary>
        public string? AuthToken { get; set; }

        /// <summary>
        /// Authorization schema
        /// </summary>
        public string? AuthSchema { get; set; }
        
        //Can contain other needed options
    }

}