﻿using HwInfoWorker.Infrastructure.Connectors.ElasticSearchDbStores.Configuration;
using HwInfoWorker.Infrastructure.Connectors.ElasticSearchDbStores.HwInfoElements;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nest;
using System;

namespace HwInfoWorker.Infrastructure.Connectors.ElasticSearchDbStores
{
    public static class ElasticSearchStoreConstants
    {
        public const string Name = "ElasticSearchStore";
    }

    public static class StoreBootstrapper
    {
        public static IServiceCollection AddStores(this IServiceCollection services, IConfiguration configuration)
        {
            var config = new ElasticSearchStoreConfiguration();
            configuration.GetSection(ElasticSearchStoreConstants.Name).Bind(config);

            services
                .Configure<ElasticSearchStoreConfiguration>(configuration.GetSection(ElasticSearchStoreConstants.Name))
                .AddSingleton<IElasticClient>(new ElasticClient(ConnectionSettings(config.EndpointAddress, config.IndexName, config.TimeoutMs, config.MaxRetries)))
                .AddTransient<IHwInfoElementsStore, HwInfoElementsStore>();

            return services;
        }

        private static ConnectionSettings ConnectionSettings(string endpointAddress, string defaultIndex, int timeoutMs, int maxRetries) =>
            new ConnectionSettings(new Uri(endpointAddress))
                .DefaultIndex(defaultIndex)
                .RequestTimeout(TimeSpan.FromMilliseconds(timeoutMs))
                .MaximumRetries(maxRetries)
                .ThrowExceptions(true);
    }
}
