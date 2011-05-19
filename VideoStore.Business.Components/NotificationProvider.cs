using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Components.Interfaces;
using VideoStore.Business.Entities;
using Microsoft.Practices.ServiceLocation;
using System.Transactions;

namespace VideoStore.Business.Components
{
	public class NotificationProvider : INotificationProvider
	{
		public IEmailProvider EmailProvider
		{
			get { return ServiceLocator.Current.GetInstance<IEmailProvider>(); }
		}

		public void NotifyDeliveryCompletion(Guid pDeliveryId, Entities.DeliveryStatus pStatus)
		{
			Order lAffectedOrder = RetrieveDeliveryOrderDeliveryId(pDeliveryId);
			UpdateDeliveryStatus(pDeliveryId, pStatus);
			if (pStatus == Entities.DeliveryStatus.Delivered)
			{
				EmailProvider.SendMessage(new EmailMessage()
				{
					ToAddress = lAffectedOrder.Customer.Email,
					Message = "Our records show that your order" + lAffectedOrder.OrderNumber + " has been delivered. Thank you for shopping at video store"
				});
			}
			if (pStatus == Entities.DeliveryStatus.Failed)
			{
				EmailProvider.SendMessage(new EmailMessage()
				{
					ToAddress = lAffectedOrder.Customer.Email,
					Message = "Our records show that there was a problem" + lAffectedOrder.OrderNumber + " delivering your order. Please contact Video Store"
				});
			}
		}

		public void NotifyBankTransactionCompleted(Guid pOrderNumber, PaymentTransactionOutcome pPaymentTransactionOutcome)
		{
			OrderProvider pOrderProvider = new OrderProvider();
            Order Order = RetrieveDeliveryOrderByOrderNumber(pOrderNumber);
            if (pPaymentTransactionOutcome == PaymentTransactionOutcome.Successful)
			{
				pOrderProvider.SubmitDelivery(Order);
            }
            else if (pPaymentTransactionOutcome == PaymentTransactionOutcome.Failure)
            {
                pOrderProvider.CancelOrder(Order);
            }
		}

		private void UpdateDeliveryStatus(Guid pDeliveryId, DeliveryStatus status)
		{
			using (TransactionScope lScope = new TransactionScope())
			using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
			{
				Delivery lDelivery = lContainer.Deliveries.Where((pDel) => pDel.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();
				if (lDelivery != null)
				{
					lDelivery.DeliveryStatus = status;
					lContainer.SaveChanges();
				}
				lScope.Complete();
			}
		}
		private Order RetrieveDeliveryOrderDeliveryId(Guid pDeliveryId)
		{
			using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
			{
				Delivery lDelivery = lContainer.Deliveries.Include("Order.Customer").Where((pDel) => pDel.ExternalDeliveryIdentifier == pDeliveryId).FirstOrDefault();
				return lDelivery.Order;
			}
		}
		private Order RetrieveDeliveryOrderByOrderNumber(Guid pOrderNumber)
		{
			using (VideoStoreEntityModelContainer lContainer = new VideoStoreEntityModelContainer())
			{
				return lContainer.Orders.Include("Customer").Where((aOrder) => aOrder.OrderNumber == pOrderNumber).FirstOrDefault();
			}
		}

     }


}
