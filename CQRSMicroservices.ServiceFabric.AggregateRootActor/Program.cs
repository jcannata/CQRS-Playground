using System;
using System.Threading;
using CQRSMicroservices.Application;
using CQRSMicroservices.Framework;

namespace CQRSMicroservices.ServiceFabric.AggregateRootActor
{
    using Microsoft.ServiceFabric.Actors.Runtime;

    internal static class Program
    {
        /// <summary>
        /// This is the entry point of the service host process.
        /// </summary>
        private static void Main()
        {
            try
            {
                // Creating a FabricRuntime connects this host process to the Service Fabric runtime on this node.
                CqrsApplication.SetService<IDeserializer>(new Deserializer());

                // This line registers your actor class with the Fabric Runtime.
                // The contents of your ServiceManifest.xml and ApplicationManifest.xml files
                // are automatically populated when you build this project.
                // For information, see http://aka.ms/servicefabricactorsplatform

                ActorRuntime.RegisterActorAsync<AggregateRootActor>(
                        (context, actorType) => new ActorService(context, actorType, () => new AggregateRootActor()))
                    .GetAwaiter()
                    .GetResult();

                Thread.Sleep(Timeout.Infinite);
                    // Prevents this host process from terminating so services keeps running.
            }
            catch (Exception e)
            {
                ActorEventSource.Current.ActorHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
