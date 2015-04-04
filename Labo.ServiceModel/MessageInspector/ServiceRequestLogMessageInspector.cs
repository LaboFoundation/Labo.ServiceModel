namespace Labo.ServiceModel.MessageInspector
{
    using System;
    using System.Diagnostics;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    public sealed class ServiceRequestLogMessageInspector : IDispatchMessageInspector
    {
        private readonly ICustomOutgoingMessageHeaderCreator m_CustomOutgoingMessageHeaderCreator;

        public ServiceRequestLogMessageInspector(ICustomOutgoingMessageHeaderCreator customOutgoingMessageHeaderCreator)
        {
            m_CustomOutgoingMessageHeaderCreator = customOutgoingMessageHeaderCreator;
        }

        public object AfterReceiveRequest(ref Message request, IClientChannel channel, InstanceContext instanceContext)
        {
            ServiceRequestInfo serviceRequestInfo = null;
            if (request.Headers.FindHeader(Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME, Constants.ServiceMessageHeaders.HEADER_NAME_SPACE) > -1)
            {
                serviceRequestInfo = request.Headers.GetHeader<ServiceRequestInfo>(Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME, Constants.ServiceMessageHeaders.HEADER_NAME_SPACE);
                if (serviceRequestInfo == null)
                {
                    serviceRequestInfo = new ServiceRequestInfo
                    {
                        RequestId = Guid.NewGuid().ToString("D")
                    };
                }
            }
            else
            {
                serviceRequestInfo = new ServiceRequestInfo
                {
                    RequestId = Guid.NewGuid().ToString("D")
                };
            }

            Debug.WriteLine(serviceRequestInfo.RequestId);

            OperationContext.Current.OutgoingMessageHeaders.Add(m_CustomOutgoingMessageHeaderCreator.CreateMessageHeader(serviceRequestInfo));

            return null;
        }

        public void BeforeSendReply(ref Message reply, object correlationState)
        {
        }
    }
}
