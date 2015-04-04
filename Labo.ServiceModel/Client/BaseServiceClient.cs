namespace Labo.ServiceModel.Client
{
    using System;
    using System.Collections.Generic;
    using System.ServiceModel;
    using System.ServiceModel.Channels;

    public abstract class BaseServiceClient<TService>
    {
        private readonly CustomOutgoingMessageHeaderCreatorCollection m_CustomOutgoingMessageHeaderCreators;

        private readonly string m_EndPointConfigurationName;

        protected BaseServiceClient(CustomOutgoingMessageHeaderCreatorCollection customOutgoingMessageHeaderCreators)
            : this(customOutgoingMessageHeaderCreators, null)
        {
        }

        protected BaseServiceClient(CustomOutgoingMessageHeaderCreatorCollection customOutgoingMessageHeaderCreators, string endPointConfigurationName)
        {
            m_EndPointConfigurationName = endPointConfigurationName;
            m_CustomOutgoingMessageHeaderCreators = customOutgoingMessageHeaderCreators;
        }

        protected TResult CallServiceOperation<TResult>(Func<TService, TResult> func)
        {
            // IList<IncomingMessageHeaderInfo> incomingMessageHeaders = GetIncomingMessageHeaders();

            using (ChannelFactory<TService> channelFactory = CreateChannelFactory())
            {
                TService service = channelFactory.CreateChannel();
                IClientChannel clientChannel = (IClientChannel)service;
                using (clientChannel)
                {
                    if (OperationContext.Current == null)
                    {
                        using (new OperationContextScope(clientChannel))
                        {
                            // AddCustomOutgoingMessageHeaders(incomingMessageHeaders);
                            AddCustomOutgoingMessageHeaders();
                            return func(service);
                        }
                    }
                    else
                    {
                        // AddCustomOutgoingMessageHeaders(incomingMessageHeaders);
                        AddCustomOutgoingMessageHeaders();
                        return func(service);
                    }               
                }
            }
        }

        protected void CallServiceOperation(Action<TService> action)
        {
            // IList<IncomingMessageHeaderInfo> incomingMessageHeaders = GetIncomingMessageHeaders();

            using (ChannelFactory<TService> channelFactory = CreateChannelFactory())
            {
                TService service = channelFactory.CreateChannel();
                IClientChannel clientChannel = (IClientChannel)service;
                using (clientChannel)
                {
                    if (OperationContext.Current == null)
                    {
                        using (new OperationContextScope(clientChannel))
                        {
                            // AddCustomOutgoingMessageHeaders(incomingMessageHeaders);
                            AddCustomOutgoingMessageHeaders();
                            action(service);
                        }
                    }
                    else
                    {
                        using (new OperationContextScope(clientChannel))
                        {
                            // AddCustomOutgoingMessageHeaders(incomingMessageHeaders);
                            AddCustomOutgoingMessageHeaders();
                            action(service);
                        }
                    }
                }
            }
        }

        private static IList<IncomingMessageHeaderInfo> GetIncomingMessageHeaders()
        {
            IList<IncomingMessageHeaderInfo> incomingMessageHeaders = new List<IncomingMessageHeaderInfo>();

            OperationContext operationContext = OperationContext.Current;
            if (operationContext != null && operationContext.IncomingMessageHeaders != null)
            {
                MessageHeaders messageHeaders = operationContext.IncomingMessageHeaders;
                foreach (MessageHeaderInfo incomingMessageHeader in messageHeaders)
                {
                    if (incomingMessageHeader.Namespace == Constants.ServiceMessageHeaders.HEADER_NAME_SPACE)
                    {
                        object headerValue = HandleIncomingMessageHeaderValue(messageHeaders, incomingMessageHeader.Name, incomingMessageHeader.Namespace);
                        if (headerValue != null)
                        {
                            incomingMessageHeaders.Add(new IncomingMessageHeaderInfo(incomingMessageHeader.Name,incomingMessageHeader.Namespace, headerValue));
                        }
                    }
                }
            }

            return incomingMessageHeaders;
        }

        private static object HandleIncomingMessageHeaderValue(MessageHeaders messageHeaders, string name, string ns)
        {
            return messageHeaders.GetHeader<object>(name, ns);
        }

        private void AddCustomOutgoingMessageHeaders()
        {
            for (int i = 0; i < m_CustomOutgoingMessageHeaderCreators.Count; i++)
            {
                ICustomOutgoingMessageHeaderCreator customOutgoingMessageHeaderCreator = m_CustomOutgoingMessageHeaderCreators[i];
                OperationContext.Current.OutgoingMessageHeaders.Add(customOutgoingMessageHeaderCreator.CreateMessageHeader());
            }
        }

        private void AddCustomOutgoingMessageHeaders(IList<IncomingMessageHeaderInfo> incomingMessageHeaders)
        {
            for (int i = 0; i < incomingMessageHeaders.Count; i++)
            {
                IncomingMessageHeaderInfo incomingMessageHeaderInfo = incomingMessageHeaders[i];
                OperationContext.Current.OutgoingMessageHeaders.Add(MessageHeader.CreateHeader(incomingMessageHeaderInfo.Name, incomingMessageHeaderInfo.Namespace, incomingMessageHeaderInfo.Value));
            }

            for (int i = 0; i < m_CustomOutgoingMessageHeaderCreators.Count; i++)
            {
                ICustomOutgoingMessageHeaderCreator customOutgoingMessageHeaderCreator = m_CustomOutgoingMessageHeaderCreators[i];
                OperationContext.Current.OutgoingMessageHeaders.Add(customOutgoingMessageHeaderCreator.CreateMessageHeader());
            }
        }

        private ChannelFactory<TService> CreateChannelFactory()
        {
            ChannelFactory<TService> channelFactory = m_EndPointConfigurationName == null ? new ChannelFactory<TService>() : new ChannelFactory<TService>(m_EndPointConfigurationName);
            channelFactory.Endpoint.Behaviors.Add(new ServiceRequestLogClientMessageInspector());
            return channelFactory;
        }
    }
}
