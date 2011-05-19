using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using System.Transactions;
using VideoStore.Business.Components.TransferService;
using VideoStore.Business.Components.DeliveryService;
using Microsoft.Practices.ServiceLocation;

namespace VideoStore.Business.Components
{
    public class OrderProvider : IOrderProvider
    {
        public IEmailProvider EmailProvider
        {
            get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
        }

        public void SubmitOrder(Entities.Order pOrder)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                try
                {
                    pOrder.OrderNumber = Guid.NewGuid();
					pOrder.UpdateStockLevels();
					lContainer.Orders.ApplyChanges(pOrder);

                    TransferFundsFromCustomer(pOrder.Customer.BankAccountNumber, pOrder.Total ?? 0.0, pOrder.OrderNumber);
					
					lContainer.SaveChanges();
					lScope.Complete();
                }
                catch (Exception lException)
                {
                    SendOrderErrorMessage(pOrder, lException);
                    throw;
                }
            }
        }

		public void SubmitDelivery(Entities.Order pOrder)
		{
			using (TransactionScope lScope = new TransactionScope())
			using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
			{
				try
				{
					PlaceDeliveryForOrder(pOrder);
					lContainer.Orders.ApplyChanges(pOrder);
					
					SendOrderPlacedConfirmation(pOrder);

					lContainer.SaveChanges();
					lScope.Complete();
				}
				catch (Exception lException)
				{
					SendOrderErrorMessage(pOrder, lException);
					throw;
				}
			}
		}

        public void CancelOrder(Entities.Order pOrder)
        {
            using (TransactionScope lScope = new TransactionScope())
            using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
            {
                try
                {
                    pOrder.RevertStockLevels();
                    lContainer.Orders.ApplyChanges(pOrder);
                    SendOrderDeclinedEmail(pOrder);
                    lContainer.SaveChanges();
                    lScope.Complete();
                }
                catch (Exception lException)
                {
                    SendOrderErrorMessage(pOrder, lException);
                    throw;
                }
            }
        }

        private void SendOrderErrorMessage(Order pOrder, Exception pException)
        {
            EmailProvider.SendMessage(new EmailMessage()
            {
                ToAddress = pOrder.Customer.Email,
                Message = "There was an error in processsing your order " + pOrder.OrderNumber + ": "+ pException.Message +". Please contact Video Store"
            });
        }

        private void SendOrderPlacedConfirmation(Order pOrder)
        {
            EmailProvider.SendMessage(new EmailMessage()
            {
                ToAddress = pOrder.Customer.Email,
                Message = "Your order " + pOrder.OrderNumber + " has been placed"
            });
        }

        private void SendOrderDeclinedEmail(Order pOrder)
        {
            EmailProvider.SendMessage(new EmailMessage()
            {
                ToAddress = pOrder.Customer.Email,
                Message = "There was an error in processsing your order " + pOrder.OrderNumber + ": The bank transfer was declined."
            });
        }

        private void PlaceDeliveryForOrder(Order pOrder)
        {
            Delivery lDelivery = new Delivery() { DeliveryStatus = DeliveryStatus.Submitted, SourceAddress = "Video Store Address", DestinationAddress = pOrder.Customer.Address, Order = pOrder };
            DeliveryServiceClient lClient = new DeliveryServiceClient();
            /**
			Guid lDeliveryIdentifier = lClient.SubmitDelivery(new DeliveryInfo()
            { 
                OrderNumber = lDelivery.Order.OrderNumber.ToString(),  
                SourceAddress = lDelivery.SourceAddress,
                DestinationAddress = lDelivery.DestinationAddress,
                DeliveryNotificationAddress = "net.tcp://localhost:9010/DeliveryNotificationService"
            });
			**/
			Guid lDeliveryIdentifier = Guid.NewGuid();
			lClient.SubmitDelivery(new DeliveryInfo()
			{
				DeliveryIdentifier = lDeliveryIdentifier,
				OrderNumber = lDelivery.Order.OrderNumber.ToString(),
				SourceAddress = lDelivery.SourceAddress,
				DestinationAddress = lDelivery.DestinationAddress,
				DeliveryNotificationAddress = "net.tcp://localhost:9010/DeliveryNotificationService/mex"
			});

            lDelivery.ExternalDeliveryIdentifier = lDeliveryIdentifier;
            pOrder.Delivery = lDelivery;
            
        }

        private void TransferFundsFromCustomer(int pCustomerAccountNumber, double pTotal, Guid pOrderNumber)
        {
            TransferServiceClient lClient = new TransferServiceClient();
            lClient.Transfer(pTotal, pCustomerAccountNumber, RetrieveVideoStoreAccountNumber(), pOrderNumber, "net.msmq://localhost/private/NotificationMessageQueueTransacted");
        }

        private int RetrieveVideoStoreAccountNumber()
        {
            return 123;
        }

        


    }
}
