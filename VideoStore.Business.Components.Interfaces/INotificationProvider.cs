using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VideoStore.Business.Entities;

namespace VideoStore.Business.Components.Interfaces
{
    public interface INotificationProvider
    {
        void NotifyDeliveryCompletion(Guid pDeliveryId, DeliveryStatus pStatus);

		void NotifyBankTransactionCompleted(Guid pOrderNumber, PaymentTransactionOutcome pPaymentTransactionOutcome);
    }
}
