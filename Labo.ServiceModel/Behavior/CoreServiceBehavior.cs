namespace Labo.ServiceModel.Behavior
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Description;
    using System.ServiceModel.Dispatcher;

    using Labo.Common.Ioc;
    using Labo.ServiceModel.Host;
    using Labo.ServiceModel.MessageInspector;

    public class CoreServiceBehavior : IServiceBehavior
    {
        private readonly IIocContainer m_IocContainer;

        private readonly DispatchMessageInspectorCollection m_DispatchMessageInspectors;

        public CoreServiceBehavior(IIocContainer iocContainer, DispatchMessageInspectorCollection dispatchMessageInspectors)
        {
            m_IocContainer = iocContainer;
            m_DispatchMessageInspectors = dispatchMessageInspectors;
        }

        public void ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            ChannelDispatcherCollection channelDispatchers = serviceHostBase.ChannelDispatchers;
            for (int i = 0; i < channelDispatchers.Count; i++)
            {
                ChannelDispatcherBase cdb = channelDispatchers[i];
                ChannelDispatcher cd = cdb as ChannelDispatcher;
                if (cd != null)
                {
                    SynchronizedCollection<EndpointDispatcher> endpoints = cd.Endpoints;
                    for (int j = 0; j < endpoints.Count; j++)
                    {
                        EndpointDispatcher ed = endpoints[j];

                        for (int k = 0; k < m_DispatchMessageInspectors.Count; k++)
                        {
                            IDispatchMessageInspector dispatchMessageInspector = m_DispatchMessageInspectors[k];
                            ed.DispatchRuntime.MessageInspectors.Add(dispatchMessageInspector);
                        }

                        ed.DispatchRuntime.InstanceProvider = new CoreInstanceProvider(m_IocContainer, serviceDescription.ServiceType);
                    }
                }
            }
        }

        public void Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
        }

        public void AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {
        }
    }
}