using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace VideoStore.Services.Interfaces
{
    [ServiceContract]
    public interface INotificationService
    {
        [OperationContract(IsOneWay=true)]
		void NotifyDeliveryCompletion(Guid pDeliveryId, DeliveryCo.Business.Entities.DeliveryStatus pStatus);

		[OperationContract(IsOneWay = true)]
		void NotifyBankTransactionCompleted(Guid pOrderNumber, Bank.Business.Entities.OperationOutcome pOperationOutcome);
    }
}
