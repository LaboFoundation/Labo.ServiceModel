namespace Labo.ServiceModel.Client
{
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using Labo.ServiceModel.MessageInspector;

    public sealed class ServiceRequestLogClientMessageInspector : IClientMessageInspector, IEndpointBehavior
    {
        public void AfterReceiveReply(ref Message reply, object correlationState)
        {
        }

        public object BeforeSendRequest(ref Message request, IClientChannel channel)
        {
            OperationContext operationContext = OperationContext.Current;
            if (operationContext != null)
            {
                MessageHeaders incomingMessageHeaders = operationContext.IncomingMessageHeaders ?? request.Headers;
                if (incomingMessageHeaders.FindHeader(Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME, Constants.ServiceMessageHeaders.HEADER_NAME_SPACE) > -1)
                {
                    ServiceRequestInfo serviceRequestInfo = incomingMessageHeaders.GetHeader<ServiceRequestInfo>(Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME, Constants.ServiceMessageHeaders.HEADER_NAME_SPACE);
                    if (serviceRequestInfo != null)
                    {
                        MessageHeader messageHeader = MessageHeader.CreateHeader(Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME, Constants.ServiceMessageHeaders.HEADER_NAME_SPACE, serviceRequestInfo);
                        // operationContext.OutgoingMessageHeaders.Add(messageHeader);

                        if (request.Headers.FindHeader(Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME, Constants.ServiceMessageHeaders.HEADER_NAME_SPACE) < 0)
                        {
                            request.Headers.Add(messageHeader);                            
                        }
                    }
                }
            }

            return null;
        }

        public void AddBindingParameters(ServiceEndpoint endpoint, BindingParameterCollection bindingParameters)
        {
        }

        public void ApplyClientBehavior(ServiceEndpoint endpoint, ClientRuntime clientRuntime)
        {
            clientRuntime.MessageInspectors.Add(new ServiceRequestLogClientMessageInspector());
        }

        public void ApplyDispatchBehavior(ServiceEndpoint endpoint, EndpointDispatcher endpointDispatcher)
        {
        }

        public void Validate(ServiceEndpoint endpoint)
        {
        }
    }
}
