namespace Labo.ServiceModel.Host
{
    using Labo.Common.Ioc;
    using Labo.ServiceModel.Behavior;
    using Labo.ServiceModel.MessageInspector;

    /// <summary>
    /// The service host configuration.
    /// </summary>
    public sealed class ServiceHostConfiguration
    {
        private readonly IIocContainer m_IocContainer;

        private DispatchMessageInspectorCollection m_DispatchMessageInspectors;

        private ServiceBehaviourCollection m_ServiceBehaviours;

        public IIocContainer IocContainer
        {
            get
            {
                return m_IocContainer;
            }
        }

        public DispatchMessageInspectorCollection DispatchMessageInspectors
        {
            get
            {
                return m_DispatchMessageInspectors ?? (m_DispatchMessageInspectors = new DispatchMessageInspectorCollection());
            }
        }

        public ServiceBehaviourCollection ServiceBehaviours
        {
            get
            {
                return m_ServiceBehaviours ?? (m_ServiceBehaviours = new ServiceBehaviourCollection());
            }
        }

        public ServiceHostConfiguration(IIocContainer iocContainer, DispatchMessageInspectorCollection dispatchMessageInspectors, ServiceBehaviourCollection serviceBehaviours)
        {
            m_IocContainer = iocContainer;
            m_DispatchMessageInspectors = dispatchMessageInspectors;
            m_ServiceBehaviours = serviceBehaviours;
        }
    }
}