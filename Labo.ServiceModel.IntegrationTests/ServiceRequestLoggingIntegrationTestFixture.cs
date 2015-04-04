namespace Labo.ServiceModel.IntegrationTests
{
    using System;
    using System.ServiceModel;

    using Labo.Common.Ioc.SimpleInjector;
    using Labo.ServiceModel.Behavior;
    using Labo.ServiceModel.Host;
    using Labo.ServiceModel.IntegrationTests.ServiceClients;
    using Labo.ServiceModel.IntegrationTests.Services;
    using Labo.ServiceModel.MessageInspector;

    using NUnit.Framework;

    [TestFixture]
    public sealed class ServiceRequestLoggingIntegrationTestFixture
    {
        private ServiceHost m_TestServiceHost;
        private ServiceHost m_TestService1Host;

        [SetUp]
        public void Setup()
        {
            ServiceHostConfiguration serviceHostConfiguration = new ServiceHostConfiguration(
                new SimpleInjectorIocContainer(),
                new DispatchMessageInspectorCollection
                    {
                        new ServiceRequestLogMessageInspector(new CustomOutgoingMessageHeaderCreator(Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME, Constants.ServiceMessageHeaders.HEADER_NAME_SPACE))
                    },
                new ServiceBehaviourCollection());
            
            m_TestServiceHost = new ServiceHostBuilder(serviceHostConfiguration, typeof(TestService)).Build();
            m_TestServiceHost.Open();

            m_TestService1Host = new ServiceHostBuilder(serviceHostConfiguration, typeof(TestService1)).Build();
            m_TestService1Host.Open();
        }

        [TearDown]
        public void Teardown()
        {
            m_TestServiceHost.Close();
            m_TestService1Host.Close();
        }

        [Test]
        public void AA()
        {
            ICustomOutgoingMessageHeaderCreator customOutgoingMessageHeaderCreator =
                new CustomOutgoingMessageHeaderCreator(
                    Constants.ServiceMessageHeaders.SERVICE_REQUEST_INFO_HEADER_NAME,
                    Constants.ServiceMessageHeaders.HEADER_NAME_SPACE,
                    () => new ServiceRequestInfo { RequestId = "xxx", SessionId = "yyy" });

            TestServiceClient testServiceClient =
                new TestServiceClient(
                    new CustomOutgoingMessageHeaderCreatorCollection { customOutgoingMessageHeaderCreator },
                    "TestServiceBasicHttpBinding");
            testServiceClient.Sum(1, 2);
        }
    }
}
