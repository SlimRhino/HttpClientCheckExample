using System;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HttpClientCheckExample.HttpClientRegistration
{
    public static class ServiceCollectionExtensions
    {
        public static IHttpClientBuilder AddHttpClient<TClient, TImplementation>(this IServiceCollection services, IConfiguration configuration)
            where TClient : class
            where TImplementation : class, TClient
        {
            var clientType = typeof(TImplementation);
            
            var clientConfig = configuration.GetSection(clientType.Name).Get<HttpClientConfig<TImplementation>>();

            //Check for availability our auth attribute in this client class
            var needAuthData = Attribute.GetCustomAttribute(clientType, typeof(RequiredAuthorizationAttribute)) != null;
            
            //The place why this was all started
            CheckConfig(clientConfig, needAuthData, clientType.Name);
            
            return services.AddHttpClient<TClient, TImplementation>(x =>
            {
                x.BaseAddress = clientConfig.Url;
                if (!needAuthData) return;
                
                if (clientConfig.AuthSchema == "Some-Custom-Header")
                    x.DefaultRequestHeaders.Add(clientConfig.AuthSchema, clientConfig.AuthToken);
                else
                    x.DefaultRequestHeaders.Authorization 
                        = new AuthenticationHeaderValue(clientConfig.AuthSchema, clientConfig.AuthToken);
            });
        }

        private static void CheckConfig<TClient>(HttpClientConfig<TClient> config, bool needAuthData, string clientName)
            where TClient : class
        {
            if (config == null)
                throw new ArgumentNullException(nameof(HttpClientConfig<TClient>), 
                    $"{clientName} configuration not found in {nameof(IConfiguration)} provider. Check application settings.");
            
            if (config.Url == null) 
                throw new ArgumentNullException(nameof(config.Url), 
                    $"{clientName} configuration not contains base client Url field");
            
            if (!needAuthData) return;

            if (config.AuthToken == null)
                throw new ArgumentNullException(nameof(config.AuthToken),
                    $"{clientName} configuration not contains authorization token field");
            
            if (config.AuthSchema == null)
                throw new ArgumentNullException(nameof(config.AuthSchema),
                    $"{clientName} configuration not contains authorization schema field");
        }
    }
}