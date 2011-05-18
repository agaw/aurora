using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using DeliveryCo.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;
using System.Configuration;
using System.Messaging;

namespace DeliveryCo.Process
{
    class Program
    {
        private static readonly String sDeliveryQueuePath = ".\\private$\\DeliveryMessageQueueTransacted";

        static void Main(string[] args)
        {
            EnsureMessageQueuesExists();
            ResolveDependencies();
            using (ServiceHost lHost = new ServiceHost(typeof(DeliveryService)))
            {
                lHost.Open();
                Console.WriteLine("Delivery Service started. Press Q to quit");
                while (Console.ReadKey().Key != ConsoleKey.Q) ;
            }

        }

        private static void EnsureMessageQueuesExists()
        {
            // Create the transacted MSMQ queue if necessary.
            if (!MessageQueue.Exists(sDeliveryQueuePath))
                MessageQueue.Create(sDeliveryQueuePath, true);
        }

        private static void ResolveDependencies()
        {

            UnityContainer lContainer = new UnityContainer();
            UnityConfigurationSection lSection
                    = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            lSection.Containers["containerOne"].Configure(lContainer);
            UnityServiceLocator locator = new UnityServiceLocator(lContainer);
            ServiceLocator.SetLocatorProvider(() => locator);
        }
    }
}
