namespace Labo.ServiceModel.Host
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Description;

    using Labo.Common.Ioc;
    using Labo.Common.Utils;
    using Labo.ServiceModel.Behavior;

    public sealed class ServiceHostBuilder
    {
        private readonly ServiceHostConfiguration m_ServiceHostConfiguration;

        private readonly Type m_ServiceType;

        private readonly Uri[] m_BaseAddresses;

        public ServiceHostBuilder(ServiceHostConfiguration serviceHostConfiguration, Type serviceType, params Uri[] baseAddresses)
        {
            m_ServiceHostConfiguration = serviceHostConfiguration;
            m_ServiceType = serviceType;
            m_BaseAddresses = baseAddresses;
        }

        public ServiceHost Build()
        {
            IIocContainer iocContainer = m_ServiceHostConfiguration.IocContainer;

            CoreServiceHost serviceHost;
            ServiceBehaviorAttribute serviceBehaviorAttribute = ReflectionUtils.GetCustomAttribute<ServiceBehaviorAttribute>(m_ServiceType);
            if (serviceBehaviorAttribute != null && serviceBehaviorAttribute.InstanceContextMode == InstanceContextMode.Single)
            {
                if (!iocContainer.IsRegistered(m_ServiceType))
                {
                    iocContainer.RegisterSingleInstance(m_ServiceType);
                }

                serviceHost = new CoreServiceHost(m_ServiceHostConfiguration, iocContainer.GetInstance(m_ServiceType), m_BaseAddresses);
            }
            else
            {
                if (!iocContainer.IsRegistered(m_ServiceType))
                {
                    iocContainer.RegisterInstance(m_ServiceType);
                }

                serviceHost = new CoreServiceHost(m_ServiceHostConfiguration, m_ServiceType, m_BaseAddresses);
            }

            AddServiceBehaviors(serviceHost);

            return serviceHost;
        }

        private void AddServiceBehaviors(ServiceHostBase serviceHost)
        {
            ServiceBehaviourCollection serviceBehaviours = m_ServiceHostConfiguration.ServiceBehaviours;
            for (int i = 0; i < serviceBehaviours.Count; i++)
            {
                IServiceBehavior serviceBehaviour = serviceBehaviours[i];
                serviceHost.Description.Behaviors.Add(serviceBehaviour);
            }
        }
    }
}
