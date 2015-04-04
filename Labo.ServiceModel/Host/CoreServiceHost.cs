namespace Labo.ServiceModel.Host
{
    using System;
    using System.ServiceModel;

    using Labo.ServiceModel.Behavior;

    public sealed class CoreServiceHost : ServiceHost
    {
        private readonly ServiceHostConfiguration m_ServiceHostConfiguration;

        public CoreServiceHost(ServiceHostConfiguration serviceHostConfiguration, object singletonInstance, params Uri[] baseAddresses)
            : base(singletonInstance, baseAddresses)
        {
            m_ServiceHostConfiguration = serviceHostConfiguration;
        }

        public CoreServiceHost(ServiceHostConfiguration serviceHostConfiguration, Type serviceType, params Uri[] baseAddresses)
            : base(serviceType, baseAddresses)
        {
            m_ServiceHostConfiguration = serviceHostConfiguration;
        }

        protected override void OnOpening()
        {
            if (Description.Behaviors.Find<CoreServiceBehavior>() == null)
            {
                Description.Behaviors.Add(new CoreServiceBehavior(m_ServiceHostConfiguration.IocContainer, m_ServiceHostConfiguration.DispatchMessageInspectors));
            }

            base.OnOpening();
        }
    }
}