﻿using HwInfoWorker.Domain.AggregatesModel.HwInfoElementAggregate;
using Microsoft.Extensions.Logging;
using Nest;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HwInfoWorker.Infrastructure.Connectors.ElasticSearchDbStores.HwInfoElements
{
    public class HwInfoElementsStore : IHwInfoElementsStore
    {
        private readonly ILogger<HwInfoElementsStore> _logger;
        private readonly IElasticClient _client;

        public HwInfoElementsStore(ILogger<HwInfoElementsStore> logger, IElasticClient client)
        {
            _logger = logger;
            _client = client;
        }

        public async Task Store(HwInfoElement element, CancellationToken cancellationToken)
        {
            try
            {
                await _client.CreateDocumentAsync(element, cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Could not store HwInfoElement document");
            }            
        }        
    }
}
