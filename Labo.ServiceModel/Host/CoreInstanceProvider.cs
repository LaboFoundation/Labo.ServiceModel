namespace Labo.ServiceModel.Host
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.ServiceModel.Dispatcher;

    using Labo.Common.Ioc;

    public class CoreInstanceProvider : IInstanceProvider
    {
        private readonly IIocContainer m_IocContainer;

        public Type ServiceType { set; private get; }

        public CoreInstanceProvider(IIocContainer iocContainer)
            : this(iocContainer, null)
        {
        }

        public CoreInstanceProvider(IIocContainer iocContainer, Type type)
        {
            m_IocContainer = iocContainer;
            ServiceType = type;
        }

        public object GetInstance(InstanceContext instanceContext, Message message)
        {
            return m_IocContainer.GetInstance(ServiceType);
        }

        public object GetInstance(InstanceContext instanceContext)
        {
            return GetInstance(instanceContext, null);
        }

        public void ReleaseInstance(InstanceContext instanceContext, object instance)
        {
        }
    }
}