using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using EmailService.Services;
using System.Messaging;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.Unity.ServiceLocatorAdapter;
using Microsoft.Practices.ServiceLocation;

namespace EmailService.Process
{
    class Program
    {
		private static readonly String sEmailQueuePath = ".\\private$\\EmailMessageQueueTransacted";

        static void Main(string[] args)
        {
			EnsureMessageQueuesExists();
			//ResolveDependencies();
			HostServices();
        }

		private static void HostServices()
		{
			using (ServiceHost lHost = new ServiceHost(typeof(EmailService.Services.EmailService)))
			{
				lHost.Open();
				Console.WriteLine("Email Service Started");
				while (Console.ReadKey().Key != ConsoleKey.Q) ;
			}
		}

		private static void EnsureMessageQueuesExists()
		{
			// Create the transacted MSMQ queue if necessary.
			if (!MessageQueue.Exists(sEmailQueuePath))
				MessageQueue.Create(sEmailQueuePath, true);
		}
    }
}
