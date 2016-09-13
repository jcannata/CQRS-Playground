using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using CQRSMicroservices.Application;
using CQRSMicroservices.Framework;

namespace CQRSMicroservices.ServiceFabric.QueryModelBuilderService
{
    using Microsoft.ServiceFabric.Services.Runtime;

    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                CqrsApplication.SetService<IDeserializer>(new Deserializer());
                CqrsApplication.SetService(new QueryRepository());
                ServiceRuntime.RegisterServiceAsync("QueryModelBuilderServiceType", context => new QueryModelBuilderService(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id,
                    typeof(QueryModelBuilderService).Name);

                Thread.Sleep(Timeout.Infinite);
                    // Prevents this host process from terminating to keep the service host process running.
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}