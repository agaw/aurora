using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DeliveryCo.Services.Interfaces;
using System.ServiceModel;

/**
 * Facotry not being used anymore.
namespace DeliveryCo.Business.Components
{
    public class DeliveryNotificationServiceFactory
    {
        public static INotificationService GetDeliveryNotificationService(String pAddress)
        {
            ChannelFactory<INotificationService> lChannelFactory = new ChannelFactory<INotificationService>(new NetTcpBinding(), new EndpointAddress(pAddress));
            return lChannelFactory.CreateChannel();
        }
    }
}
**/
