﻿using System;
using System.Threading.Tasks;
using CQRSMicroservices.Articles;
using CQRSMicroservices.Framework;
using CQRSMicroservices.ServiceFabric.Interfaces;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Newtonsoft.Json.Linq;

namespace CQRSMicroservices.ServiceFabric.WebService
{
    using Microsoft.ServiceFabric.Services.Client;

    public class ServiceFabricQueryRepository : QueryRepository
    {
        public override Task Add(Guid id, JObject jObject)
        {
            throw new NotImplementedException("This repository is only meant for reading.");
        }

        public override Task Update(Guid id, JObject jObject)
        {
            throw new NotImplementedException("This repository is only meant for reading.");
        }

        public override async Task<JObject> Get(Guid id)
        {
            // This is a very simple implementation. We let the QueryModelBuilder service maintain the data
            // Normally this would be a database, or a seperate data service.

            var partition =
                new ServicePartitionKey(
                    $"{typeof(ArticleQueryModelBuilder).FullName}, {typeof(ArticleQueryModelBuilder).Assembly.GetName().Name}");
            // In this case, of course we should select the right partition for this query.
            var queryModelBuilderService = ServiceProxy.Create<IQueryModelBuilderService>(
                new Uri("fabric:/CQRSMicroservices.ServiceFabric.Application/QueryModelBuilderService"),
                partition);

            var result = await queryModelBuilderService.Get(id);
            return result == null ? null : JObject.Parse(result);
        }
    }
}