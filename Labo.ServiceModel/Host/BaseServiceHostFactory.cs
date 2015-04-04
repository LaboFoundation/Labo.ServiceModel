namespace Labo.ServiceModel.Host
{
    using System;
    using System.ServiceModel;
    using System.ServiceModel.Activation;

    using Labo.Common.Ioc;

    public abstract class BaseServiceHostFactory : ServiceHostFactory
    {
        protected override ServiceHost CreateServiceHost(Type serviceType, Uri[] baseAddresses)
        {
            IIocContainer iocContainer = CreateIocContainer();
            
            ServiceHostBuilder serviceHostBuilder = new ServiceHostBuilder(new ServiceHostConfiguration(iocContainer, null, null), serviceType, baseAddresses);

            return serviceHostBuilder.Build();
        }

        protected abstract IIocContainer CreateIocContainer();
    }
}