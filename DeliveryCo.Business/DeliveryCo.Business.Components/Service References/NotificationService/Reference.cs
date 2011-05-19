﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.1
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DeliveryCo.Business.Components.NotificationService {
    using System.Runtime.Serialization;
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0")]
    [System.Runtime.Serialization.DataContractAttribute(Name="OperationOutcome", Namespace="http://schemas.datacontract.org/2004/07/Bank.Business.Entities")]
    public enum OperationOutcome : int {
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Successful = 0,
        
        [System.Runtime.Serialization.EnumMemberAttribute()]
        Failure = 1,
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(ConfigurationName="NotificationService.INotificationService")]
    public interface INotificationService {
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/INotificationService/NotifyDeliveryCompletion")]
        void NotifyDeliveryCompletion(System.Guid pDeliveryId, DeliveryCo.Business.Entities.DeliveryStatus pStatus);
        
        [System.ServiceModel.OperationContractAttribute(IsOneWay=true, Action="http://tempuri.org/INotificationService/NotifyBankTransactionCompleted")]
        void NotifyBankTransactionCompleted(System.Guid pOrderNumber, DeliveryCo.Business.Components.NotificationService.OperationOutcome pOperationOutcome);
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface INotificationServiceChannel : DeliveryCo.Business.Components.NotificationService.INotificationService, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class NotificationServiceClient : System.ServiceModel.ClientBase<DeliveryCo.Business.Components.NotificationService.INotificationService>, DeliveryCo.Business.Components.NotificationService.INotificationService {
        
        public NotificationServiceClient() {
        }
        
        public NotificationServiceClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public NotificationServiceClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NotificationServiceClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public NotificationServiceClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        public void NotifyDeliveryCompletion(System.Guid pDeliveryId, DeliveryCo.Business.Entities.DeliveryStatus pStatus) {
            base.Channel.NotifyDeliveryCompletion(pDeliveryId, pStatus);
        }
        
        public void NotifyBankTransactionCompleted(System.Guid pOrderNumber, DeliveryCo.Business.Components.NotificationService.OperationOutcome pOperationOutcome) {
            base.Channel.NotifyBankTransactionCompleted(pOrderNumber, pOperationOutcome);
        }
    }
}