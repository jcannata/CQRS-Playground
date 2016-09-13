using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;

namespace CQRSMicroservices.ServiceFabric.WebService
{
    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // Creating a FabricRuntime connects this host process to the Service Fabric runtime.
                ServiceRuntime.RegisterServiceAsync("WebServiceType", context => new WebService(context))
                    .GetAwaiter()
                    .GetResult();
                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(WebService).Name);

                Thread.Sleep(Timeout.Infinite);
                    // Prevents this host process from terminating so services keeps running.
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}