using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Services.Interfaces;
using VideoStore.Business.Components.Interfaces;
using Microsoft.Practices.ServiceLocation;
using VideoStore.Business.Entities;

namespace VideoStore.Services
{
    public class NotificationService : INotificationService
    {
        INotificationProvider Provider
        {
            get { return ServiceLocator.Current.GetInstance<INotificationProvider>(); }
        }

        public void NotifyDeliveryCompletion(Guid pDeliveryId, DeliveryCo.Business.Entities.DeliveryStatus pStatus)
        {
            Provider.NotifyDeliveryCompletion(pDeliveryId, GetDeliveryStatusFromDeliveryCoDeliveryStatus(pStatus));
        }

		public void NotifyBankTransactionCompleted(Guid pOrderNumber, Bank.Business.Entities.OperationOutcome pOperationOutcome)
		{
			Provider.NotifyBankTransactionCompleted(pOrderNumber, GetPaymentTransactionOutcomeFromOperationOutcome(pOperationOutcome));
		}

		private DeliveryStatus GetDeliveryStatusFromDeliveryCoDeliveryStatus(DeliveryCo.Business.Entities.DeliveryStatus pStatus)
        {
			if (pStatus == DeliveryCo.Business.Entities.DeliveryStatus.Delivered)
            {
                return DeliveryStatus.Delivered;
            }
			else if (pStatus == DeliveryCo.Business.Entities.DeliveryStatus.Failed)
            {
                return DeliveryStatus.Failed;
            }
			else if (pStatus == DeliveryCo.Business.Entities.DeliveryStatus.Submitted)
            {
                return DeliveryStatus.Submitted;
            }
            else
            { 
                throw new Exception("Unexpected delivery pStatus received");
            }
        }

		private PaymentTransactionOutcome GetPaymentTransactionOutcomeFromOperationOutcome(Bank.Business.Entities.OperationOutcome pOperationOutcome)
		{
			switch (pOperationOutcome)
			{
				case  Bank.Business.Entities.OperationOutcome.Successful:
					return PaymentTransactionOutcome.Successful;
				default:
					return PaymentTransactionOutcome.Failure;
			}

		}

    }
}
